namespace Api.Goal.Contracts
{
    public record GetGoalDatesByMonthDto(DateOnly Date, Guid GoalId);
}
