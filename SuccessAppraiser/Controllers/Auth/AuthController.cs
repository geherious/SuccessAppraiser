using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.Contracts.Auth;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Auth.Interfaces;
using SuccessAppraiser.Validation;

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
        private readonly IValidator<NewRegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IAuthService authService, IJwtService jwtService,
            IValidator<NewRegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _jwtService = jwtService;
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
                ModelState.AddModelError("Login", "Doesn't exist");
                return Unauthorized(ModelState);
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Login", "Invalid");
                return Unauthorized(ModelState);
            }

            var userClaims = await _userManager.GetClaimsAsync(user);


            var jwt = _jwtService.GetAccessToken(user, userClaims);

            return Ok(new { AccessToken = jwt });

        }
    }
}
