namespace SuccessAppraiser.Services.Goal.DTO
{
    public record GetUserGoalDto(Guid Id, string Name, string? Description, int DaysNumber,
        DateOnly DateStart, List<GoalDateDto> Dates);
}
