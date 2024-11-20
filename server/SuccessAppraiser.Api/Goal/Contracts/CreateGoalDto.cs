namespace SuccessAppraiser.Api.Goal.Contracts;

public record CreateGoalDto()
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required int DaysNumber { get; init; }
    public required DateOnly DateStart { get; init; }
    public required Guid TemplateId {  get; init; }
}