namespace SuccessAppraiser.Api.Goal.Contracts
{
    public record CreateGoalDateDto(DateOnly Date, string? Comment, Guid StateId);
}
