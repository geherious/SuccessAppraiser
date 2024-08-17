namespace SuccessAppraiser.Api.Goal.Contracts
{
    public record CreateGoalDto(string Name, string? Description, int DaysNumber, DateOnly DateStart, Guid TemplateId);
}
