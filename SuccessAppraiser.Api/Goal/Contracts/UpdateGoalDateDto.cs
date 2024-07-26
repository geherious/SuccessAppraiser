namespace SuccessAppraiser.Api.Goal.Contracts
{
    public record UpdateGoalDateDto(DateOnly Date, string? Comment, Guid StateId);
}
