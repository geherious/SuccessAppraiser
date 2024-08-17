using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;

namespace SuccessAppraiser.Data.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByToken(string token, CancellationToken ct = default);
    }
}
