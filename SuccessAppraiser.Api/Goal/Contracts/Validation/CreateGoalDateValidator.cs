﻿using FluentValidation;

namespace SuccessAppraiser.Api.Goal.Contracts.Validation
{
    public class CreateGoalDateValidator : AbstractValidator<CreateGoalDateDto>
    {
        public CreateGoalDateValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.StateId).NotEmpty();
            RuleFor(x => x.Comment).MaximumLength(1024);
        }
    }
}
