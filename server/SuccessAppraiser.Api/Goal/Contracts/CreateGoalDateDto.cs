namespace SuccessAppraiser.Api.Goal.Contracts;

public record CreateGoalDateDto
{
    public required DateOnly Date { get; init; }
    public string? Comment { get; init; }
    public required Guid StateId { get; init; }
}