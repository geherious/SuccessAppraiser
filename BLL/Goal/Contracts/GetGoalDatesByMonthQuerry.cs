using System;

namespace SuccessAppraiser.BLL.Goal.Contracts
{
    public record GetGoalDatesByMonthQuerry(DateOnly Date, Guid GoalId);
}
