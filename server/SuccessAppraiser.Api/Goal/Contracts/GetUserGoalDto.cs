namespace SuccessAppraiser.Api.Goal.Contracts
{
    public record GetUserGoalDto(Guid Id, string Name, string? Description, int DaysNumber,
        DateOnly DateStart, List<GetGoalDateDto> Dates, GetTemplateDto Template);
}
