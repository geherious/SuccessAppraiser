namespace SuccessAppraiser.Services.Goal.Errors
{
    public class InvalidStateException : Exception
    {
        public InvalidStateException(string message = "Invalid state for provided goal.") : base(message)
        {
            
        }
    }
}
