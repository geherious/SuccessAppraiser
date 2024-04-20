namespace SuccessAppraiser.Services.Goal.Errors
{
    public class GoalNotFoundException : Exception
    {
        public GoalNotFoundException(string message = "There is no such goal with provided ID.") : base(message) { }
    }
}
