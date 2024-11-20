namespace SuccessAppraiser.Api.Goal.Contracts.Validation;

public record CreateDayStateDto
{
    public required string Name { get; init; }
    public required string Color { get; init; }
}
