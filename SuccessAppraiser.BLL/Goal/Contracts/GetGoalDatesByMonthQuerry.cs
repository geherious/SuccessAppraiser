namespace SuccessAppraiser.BLL.Goal.Contracts
{
    public record GetGoalDatesByMonthQuerry(DateOnly DateOfMonth)
    {
        public Guid GoalId { get; set; }
    }
}
