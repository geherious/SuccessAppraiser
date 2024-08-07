﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SuccessAppraiser.Api.Filters
{
    public class DtoValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var arguments = context.ActionArguments.Values.ToList();

            foreach (var argument in arguments)
            {
                if (argument == null)
                {
                    continue;
                }
                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                if (validatorType == null)
                {
                    continue;
                }
                IValidator? validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

                if (validator == null)
                {
                    continue;
                }

                var validationResult = validator.Validate(new ValidationContext<object>(argument));
                if (!validationResult.IsValid || !context.ModelState.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        context.ModelState.TryAddModelError(error.PropertyName, error.ErrorMessage);
                    }
                    context.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }
            }

        }
    }
}
