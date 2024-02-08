namespace SuccessAppraiser.Services.Auth.Errors
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message = "There is no user with provided Id") : base(message)
        {
            
        }
    }
}
