using System;

namespace SuccessAppraiser.BLL.Goal.Contracts
{
    public record CreateGoalDateCommand(DateOnly Date, string? Comment, Guid StateId, Guid GoalId);
}
