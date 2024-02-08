using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        Task<Guid> AddRefreshTokenAsync(Guid userId);
        Task RemoveRefreshTokenAsync(Guid userId, Guid token);
        void RemoveAllExpiredRefreshTokens(Guid userId);
    }
}
