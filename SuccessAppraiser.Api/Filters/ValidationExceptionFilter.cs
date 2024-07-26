using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SuccessAppraiser.Api.Filters
{
    public class ValidationExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var response = new ValidationProblemDetails()
                {
                    Status = 400
                };

                response.Errors = validationException.Errors.
                    GroupBy(f => f.PropertyName).
                    ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

                context.Result = new BadRequestObjectResult(response);

            }
        }
    }
}
