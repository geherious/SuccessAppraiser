using FluentValidation;
using FluentValidation.Results;

namespace SuccessAppraiser.BLL.Goal.Exceptions
{
    public class InvalidDateException : ValidationException
    {
        public InvalidDateException(string message, IEnumerable<ValidationFailure> failures)
            : base(message, failures)
        {
            
        }
    }
}
