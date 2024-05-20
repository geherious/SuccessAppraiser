using SuccessAppraiser.Data.Entities;

namespace Api.Goal.Contracts
{
    public record GetGoalDateDto(DateOnly Date, string? Comment, Guid StateId);
}
