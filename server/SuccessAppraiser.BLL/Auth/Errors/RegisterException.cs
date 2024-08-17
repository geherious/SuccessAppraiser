using Microsoft.AspNetCore.Identity;

namespace SuccessAppraiser.BLL.Auth.Errors
{
    public class RegisterException : Exception
    {
        public RegisterException(IEnumerable<IdentityError> errors) : base(errors.First().Description)
        {
            Errors = errors;
        }

        public IEnumerable<IdentityError> Errors { get; }
    }
}
