using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Auth.Interfaces
{
    public interface ITokenService
    {
        Task<RefreshToken> AddRefreshTokenAsync(Guid userId, CancellationToken ct = default);
        Task RemoveRefreshTokenAsync(string token, CancellationToken ct = default);
        Task RemoveRefreshTokenAsync(RefreshToken token, CancellationToken ct = default);
        Task<RefreshToken?> GetValidTokenEntityAsync(string token, CancellationToken ct = default);
    }
}
