using SuccessAppraiser.BLL.Auth.Contracts;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterCommand registerCommand);

        Task<bool> UserAlreadyExistAsync(RegisterCommand registerCommand);

        Task<ApplicationUser?> Login(LoginQuerry loginQuerry);
    }
}
