namespace SuccessAppraiser.Services.Goal.Errors
{
    public class GoalDateAlreadyExistsException : Exception
    {
        public GoalDateAlreadyExistsException(string message = "Goal date with provided date already exists")
            : base(message)
        {
            
        }
    }
}
