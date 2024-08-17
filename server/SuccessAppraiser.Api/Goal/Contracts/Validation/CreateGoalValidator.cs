using FluentValidation;

namespace SuccessAppraiser.Api.Goal.Contracts.Validation
{
    public class CreateGoalValidator : AbstractValidator<CreateGoalDto>
    {
        public CreateGoalValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
            RuleFor(x => x.DaysNumber).NotNull().GreaterThan(0).WithMessage("Number of days should be more than zero");
            RuleFor(x => x.DateStart).NotEmpty();
            RuleFor(x => x.TemplateId).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(256);
        }
    }
}
