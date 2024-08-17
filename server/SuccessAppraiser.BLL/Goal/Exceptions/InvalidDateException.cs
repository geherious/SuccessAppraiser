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

        public InvalidDateException(DateOnly date) : base(GetErrorMessage(date),
            new[] { new ValidationFailure("Date", GetErrorMessage(date)) })
        {

        }

        private static string GetErrorMessage(DateOnly date)
        {
            return $"Provided date {date} doesn't exist.";
        }
    }
}
