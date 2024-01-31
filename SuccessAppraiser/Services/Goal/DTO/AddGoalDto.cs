namespace SuccessAppraiser.Services.Goal.DTO
{
    public record AddGoalDto(string Name, string? Description, int DaysNumber, DateOnly dateStart, Guid TemplateId);
}
