using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.Contracts.Auth;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Auth.Interfaces;
using SuccessAppraiser.Validation;
using System.Security.Claims;

namespace SuccessAppraiser.Controllers.Auth
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
        private readonly IValidator<NewRegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAuthService authService, IJwtService jwtService, ITokenService tokenService,
            IValidator<NewRegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _jwtService = jwtService;
            _tokenService = tokenService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] NewRegisterDto dto)
        {
            ValidationResult validation = await _registerValidator.ValidateAsync(dto);
            validation.AddToModelState(ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _authService.UserAlreadyExistAsync(dto))
            {
                return Conflict(new { message = "User already exists" });
            }

            await _authService.RegisterAsync(dto);
            return Ok();

        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            ValidationResult validation = await _loginValidator.ValidateAsync(dto);
            validation.AddToModelState(ModelState);

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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            ];

            var accessToken = _jwtService.GenerateToken(userClaims);
            Guid refreshToken = await _tokenService.AddRefreshTokenAsync(user.Id);


            Response.Cookies.Append("X-Refresh-Token", refreshToken.ToString(), new CookieOptions { HttpOnly = true,
                SameSite = SameSiteMode.Strict, Secure = true });


            return Ok(new { AccessToken = accessToken });

        }


    }
}
