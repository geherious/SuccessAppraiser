using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Goal.DTO;

namespace SuccessAppraiser.Services.Goal.Interfaces
{
    public interface IGoalService
    {
        Task<List<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<bool> UserhasGoalAsync(Guid userId, Guid goalId, CancellationToken ct = default);
        Task DeleteGoalAsync(Guid goalId, CancellationToken ct = default);
        Task<GoalItem> AddGoalAsync(Guid userId, AddGoalDto addGoalDto, CancellationToken ct = default);
    
    }
}
