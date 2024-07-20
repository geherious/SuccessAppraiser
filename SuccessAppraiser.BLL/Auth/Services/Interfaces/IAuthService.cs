using SuccessAppraiser.BLL.Auth.Contracts;

namespace SuccessAppraiser.BLL.Auth.Services.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterCommand registerCommand);

        Task<bool> UserAlreadyExistAsync(RegisterCommand registerCommand);
    }
}
