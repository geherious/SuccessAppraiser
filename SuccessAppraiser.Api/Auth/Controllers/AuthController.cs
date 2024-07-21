using SuccessAppraiser.Api.Auth.Contracts;
using AutoMapper;
using SuccessAppraiser.Data.Enums;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.BLL.Auth.Contracts;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using System.Security.Claims;
using SuccessAppraiser.Api.Filters;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.Api.Auth.Controllers
{
    [ApiController]
    [Route("auth/[action]")]
    [ValidationExceptionFilter]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IJwtService jwtService,
            ITokenService tokenService, IMapper mapper)
        {
            _authService = authService;
            _jwtService = jwtService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        [HttpPost]
        [DtoValidationFilter]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = _mapper.Map<RegisterCommand>(dto);

            if (await _authService.UserAlreadyExistAsync(command))
            {
                return Conflict(new { message = "User already exists" });
            }

            await _authService.RegisterAsync(command);

            return Ok();

        }

        [HttpPost]
        [DtoValidationFilter]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            LoginQuerry loginQuerry = _mapper.Map<LoginQuerry>(dto);

            var user = await _authService.Login(loginQuerry);

            if (user == null)
            {
                string message = "Invalid login credentials";
                return Unauthorized(new {Message = message});
            }

            List<Claim> userClaims =
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ];

            string accessToken = _jwtService.GenerateToken(userClaims, TokenType.AccessToken);
            var refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id);

            AppendAuthCookies(Response.Cookies, accessToken, refreshToken);
            AuthDto response = new(accessToken, user.UserName!);

            return Ok(response);

        }

        private void AppendAuthCookies(IResponseCookies cookies, string accessToken, RefreshToken refreshToken)
        {
            cookies.Append("X-Refresh-Token", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = refreshToken.Expires
            });
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
            await _tokenService.RemoveRefreshTokenAsync(oldRefreshToken, ct);

            AppendAuthCookies(Response.Cookies, accessToken, refreshToken);
            AuthDto response = new(accessToken, refreshToken.User.UserName);

            return Ok(response);
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
