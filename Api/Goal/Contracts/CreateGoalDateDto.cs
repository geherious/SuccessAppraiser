using SuccessAppraiser.Data.Entities;

namespace Api.Goal.Contracts
{
    public record CreateGoalDateDto(DateOnly Date, string? Comment, Guid StateId, Guid GoalId);
}
