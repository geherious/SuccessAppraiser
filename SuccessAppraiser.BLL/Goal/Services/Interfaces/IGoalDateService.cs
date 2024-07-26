using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.Data.Entities;

namespace SuccessAppraiser.BLL.Goal.Services.Interfaces
{
    public interface IGoalDateService
    {
        Task<GoalDate> CreateGoalDateAsync(CreateGoalDateCommand dateDto, CancellationToken ct = default);
        Task<IEnumerable<GoalDate>> GetGoalDatesByMonthAsync(GetGoalDatesByMonthQuerry dateDto, CancellationToken ct = default);
        Task<GoalDate> UpdateGoaldateAsync(UpdateGoalDateCoomand updateCommand, CancellationToken ct = default);
    }
}
