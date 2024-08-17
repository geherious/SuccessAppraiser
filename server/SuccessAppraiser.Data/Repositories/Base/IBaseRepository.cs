using System.Linq.Expressions;

namespace SuccessAppraiser.Data.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
        Task<T?> GetByIdAsync(Guid guid, CancellationToken ct = default);
        Task AddAsync(T entity, CancellationToken ct = default);
        void Delete(T entity);
        void Update(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    }
}
