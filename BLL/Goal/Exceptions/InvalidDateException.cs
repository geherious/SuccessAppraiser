using FluentValidation;
using FluentValidation.Results;
using System;

namespace BLL.Goal.Exceptions
{
    public class InvalidDateException : ValidationException
    {
        public InvalidDateException(string message, IEnumerable<ValidationFailure> failures)
            : base(message, failures)
        {
            
        }
    }
}
