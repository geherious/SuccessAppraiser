using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Goal.Services.Interfaces
{
    public interface IGoalService
    {
        Task<IEnumerable<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task UserhasGoalOrThrowAsync(Guid userId, Guid goalId, CancellationToken ct = default);
        Task DeleteGoalAsync(Guid goalId, CancellationToken ct = default);
        Task<GoalItem> CreateGoalAsync(CreateGoalCommand createCommand, CancellationToken ct = default);

    }
}
