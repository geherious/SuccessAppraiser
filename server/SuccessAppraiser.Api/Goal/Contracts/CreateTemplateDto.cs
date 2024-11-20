using SuccessAppraiser.Api.Goal.Contracts.Validation;

namespace SuccessAppraiser.Api.Goal.Contracts;

public record CreateTemplateDto
{
    public required string Name { get; init; }

    public required List<CreateDayStateDto> States { get; init; } = new List<CreateDayStateDto>();

}
