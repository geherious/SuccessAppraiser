using SuccessAppraiser.Contracts.Auth;
using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(NewRegisterDto registerDto);

        Task<bool> UserAlreadyExistAsync(NewRegisterDto userDto);
    }
}
