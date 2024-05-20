using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Goal.Services.Interfaces
{
    public interface IGoalService
    {
        Task<List<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<bool> UserhasGoalAsync(Guid userId, Guid goalId, CancellationToken ct = default);
        Task DeleteGoalAsync(Guid goalId, CancellationToken ct = default);
        Task<GoalItem> CreateGoalAsync(Guid userId, CreateGoalCommand createCommand, CancellationToken ct = default);

    }
}
