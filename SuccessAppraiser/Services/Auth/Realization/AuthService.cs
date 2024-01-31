using Microsoft.AspNetCore.Identity;
using SuccessAppraiser.Contracts.Auth;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Auth.Errors;
using SuccessAppraiser.Services.Auth.Interfaces;

namespace SuccessAppraiser.Services.Auth.Realization
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task RegisterAsync(NewRegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                Email = registerDto.Email,
                UserName = registerDto.Username
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                throw new RegisterException(result.Errors.First().Description);
            }
            
        }

        public async Task<bool> UserAlreadyExistAsync(NewRegisterDto userDto)
        {
            return await _userManager.FindByEmailAsync(userDto.Email) != null ||
                await _userManager.FindByNameAsync(userDto.Username) != null;
        }
    }
}
