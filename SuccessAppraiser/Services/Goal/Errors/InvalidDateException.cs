namespace SuccessAppraiser.Services.Goal.Errors
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException(string message = "Provided date is invalid") : base(message)
        {
            
        }
    }
}
