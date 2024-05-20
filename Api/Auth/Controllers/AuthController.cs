using Api.Auth.Contracts;
using AutoMapper;
using Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.BLL.Auth.Contracts;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using SuccessAppraiser.Data.Entities;
using System.Security.Claims;

namespace Api.Auth.Controllers
{
    [ApiController]
    [Route("auth/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAuthService authService, IJwtService jwtService, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _jwtService = jwtService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] NewRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = _mapper.Map<RegisterCommand>(dto);

            if (await _authService.UserAlreadyExistAsync(command))
            {
                return Conflict(new { message = "User already exists" });
            }

            await _authService.RegisterAsync(command);
            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                ModelState.AddModelError("Login", "User doesn't exist");
                return Unauthorized(ModelState);
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Login", "Invalid login attempt");
                return Unauthorized(ModelState);
            }
            List<Claim> userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ];

            string accessToken = _jwtService.GenerateToken(userClaims, TokenType.AccessToken);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id);


            Response.Cookies.Append("X-Refresh-Token", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = refreshToken.Expires
            });


            return Ok(new { AccessToken = accessToken, Username = user.UserName });

        }

        [HttpGet]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            if (!Request.Cookies.ContainsKey("X-Refresh-Token"))
            {
                return Unauthorized("No refresh token");
            }

            string token = Request.Cookies["X-Refresh-Token"]!;
            var oldRefreshToken = await _tokenService.GetValidTokenEntityAsync(token, ct);
            if (oldRefreshToken == null)
            {
                return Unauthorized("Bad refresh token");
            }

            List<Claim> userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, oldRefreshToken.UserId.ToString())
            ];

            var accessToken = _jwtService.GenerateToken(userClaims, TokenType.AccessToken);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(oldRefreshToken.UserId, ct);
            await _tokenService.RemoveRefreshTokenAsync(token, ct);

            Response.Cookies.Append("X-Refresh-Token", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = refreshToken.Expires
            });
            // check user from token and test

            return Ok(new { AccessToken = accessToken, Username = refreshToken.User.UserName });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.ContainsKey("X-Refresh-Token"))
            {
                return Forbid("No refresh token");
            }

            string token = Request.Cookies["X-Refresh-Token"]!;
            var oldRefreshToken = await _tokenService.GetValidTokenEntityAsync(token);
            if (oldRefreshToken == null)
            {
                return Forbid("Bad refresh token");
            }

            await _tokenService.RemoveRefreshTokenAsync(oldRefreshToken);

            return Ok();
        }


    }
}
