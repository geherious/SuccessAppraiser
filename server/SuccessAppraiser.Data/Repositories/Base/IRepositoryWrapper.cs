namespace SuccessAppraiser.Data.Repositories.Base
{
    public interface IRepositoryWrapper
    {
        Task SaveChangesAsync(CancellationToken ct = default);
    }
}
