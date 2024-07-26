using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;

namespace SuccessAppraiser.Data.Repositories.Interfaces
{
    public interface IGoalRepository : IBaseRepository<GoalItem>
    {
        Task<IEnumerable<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<bool> UserHasGoalAsync(Guid userId, Guid goalId, CancellationToken ct = default);
    }
}
