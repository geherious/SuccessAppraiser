using SuccessAppraiser.Contracts.Goal;

namespace SuccessAppraiser.Services.Goal.Interfaces
{
    public interface IGoalDateService
    {
        bool DateAlreadyExists(DateOnly date);
        void AddGoalDate(GoalDateDto dateDto);
    }
}
