using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Auth.Dto;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(NewRegisterDto registerDto);

        Task<bool> UserAlreadyExistAsync(NewRegisterDto userDto);
    }
}
