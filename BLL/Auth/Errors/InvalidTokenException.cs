using FluentValidation;
using System;

namespace BLL.Auth.Errors
{
    public class InvalidTokenException : ValidationException
    {
        public InvalidTokenException() : base(getErrorMesage())
        {
            
        }

        private static string getErrorMesage()
        {
            return "Provided token is invalid.";
        }
    }
}
