using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.Data.Repositories.Base
{
    public interface IRepositoryWrapper
    {
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
