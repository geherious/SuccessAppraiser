using SuccessAppraiser.BLL.Auth.Contracts;
using SuccessAppraiser.BLL.Auth.Errors;
using SuccessAppraiser.BLL.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                throw new RegisterException(result.Errors);
            }

        }

        public async Task<bool> UserAlreadyExistAsync(RegisterCommand registerCommand)
        {
            return await _userManager.FindByEmailAsync(registerCommand.Email) != null ||
                await _userManager.FindByNameAsync(registerCommand.Username) != null;
        }
    }
}
