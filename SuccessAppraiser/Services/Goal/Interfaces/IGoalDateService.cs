using SuccessAppraiser.Contracts.Goal;
using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Goal.Interfaces
{
    public interface IGoalDateService
    {
        Task<GoalDate> AddGoalDateAsync(AddGoalDateDto dateDto, CancellationToken ct = default);
        Task<IList<GetGoalDateDto>> GetGoalDatesByMonth(DateOnly date, Guid goalId, CancellationToken ct = default);
    }
}
