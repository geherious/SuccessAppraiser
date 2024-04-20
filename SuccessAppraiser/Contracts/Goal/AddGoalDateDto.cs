using SuccessAppraiser.Entities;

namespace SuccessAppraiser.Contracts.Goal
{
    public record AddGoalDateDto(DateOnly Date, string? Comment, Guid StateId, Guid GoalId);
}
