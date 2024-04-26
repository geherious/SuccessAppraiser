using SuccessAppraiser.Contracts.Goal;
using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Services.Goal.Interfaces
{
    public interface IGoalDateService
    {
        Task<GetGoalDateDto> AddGoalDateAsync(AddGoalDateDto dateDto, CancellationToken ct = default);
        Task<IList<GetGoalDateDto>> GetGoalDatesByMonthAsync(GetGoalDatesByMonth dateDto, CancellationToken ct = default);
    }
}
