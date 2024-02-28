using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        Task<RefreshToken> AddRefreshTokenAsync(Guid userId);
        Task RemoveRefreshTokenAsync(string token);
        Task<RefreshToken?> GetValidTokenEntityAsync(string token);
    }
}
