using System;

namespace SuccessAppraiser.BLL.Goal.Contracts
{
    public record UpdateGoalDateCoomand(DateOnly Date, string? Comment, Guid StateId)
    {
        public Guid GoalId { get; set; }
    }
}
