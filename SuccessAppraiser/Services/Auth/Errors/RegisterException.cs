namespace SuccessAppraiser.Services.Auth.Errors
{
    public class RegisterException : Exception
    {
        public RegisterException(string message) : base(message) { }
    }
}
