using SuccessAppraiser.BLL.Auth.Contracts;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using SuccessAppraiser.Data.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace SuccessAppraiser.BLL.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task RegisterAsync(RegisterCommand registerCommand)
        {
            var user = new ApplicationUser
            {
                Email = registerCommand.Email,
                UserName = registerCommand.Username
            };

            var result = await _userManager.CreateAsync(user, registerCommand.Password);

            if (!result.Succeeded)
            {
                var message = $"A user with provided data already exists";
                var errors = result.Errors
                    .Select(x => new ValidationFailure(nameof(ApplicationUser), x.Description))
                    .ToList();
                throw new ValidationException(message, errors);
            }

        }

        public async Task<bool> UserAlreadyExistAsync(RegisterCommand registerCommand)
        {
            return await _userManager.FindByEmailAsync(registerCommand.Email) != null ||
                await _userManager.FindByNameAsync(registerCommand.Username) != null;
        }

        public async Task<ApplicationUser?> Login(LoginQuerry loginQuerry)
        {
            var user = await _userManager.FindByEmailAsync(loginQuerry.Email);

            if (user == null)
            {
                return null;
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginQuerry.Password, false);

            if (!signInResult.Succeeded)
            {
                return null;
            }

            return user;
        }
    }
}
