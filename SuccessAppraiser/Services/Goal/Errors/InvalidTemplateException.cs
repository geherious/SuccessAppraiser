namespace SuccessAppraiser.Services.Goal.Errors
{
    public class InvalidTemplateException : Exception
    {
        public InvalidTemplateException(string message = "Invalid template for provided goal.") : base(message)
        {
            
        }
    }
}
