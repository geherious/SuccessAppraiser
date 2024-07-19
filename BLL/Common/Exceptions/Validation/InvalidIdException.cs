using FluentValidation;
using FluentValidation.Results;
using System;

namespace BLL.Common.Exceptions.Validation
{
    public class InvalidIdException : ValidationException
    {
        public string ClassName { get; set; }

        public InvalidIdException(string className, Guid id)
            : base(GetErrorMessage(className, id),
                  new[] { new ValidationFailure(className, GetErrorMessage(className, id)) })
        {
            ClassName = className;
        }

        private static string GetErrorMessage(string className, Guid id)
        {
            return $"{className} with Id {id} doesn't exist";
        }
    }
}
