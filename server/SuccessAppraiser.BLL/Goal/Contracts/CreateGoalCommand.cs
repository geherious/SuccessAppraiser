namespace SuccessAppraiser.BLL.Goal.Contracts
{
    public record CreateGoalCommand
    {
        public required string Name { get; init; } 
        public string? Description { get; init; }
        public required int DaysNumber { get; init; }
        public required DateOnly DateStart { get; init; }
        public required Guid TemplateId { get; init; }
        public Guid UserId { get; set; }
    }
}
