namespace SuccessAppraiser.Contracts.Goal
{
    public record AddGoalDto(string Name, string? Description, int DaysNumber, DateOnly dateStart, Guid TemplateId);
}
