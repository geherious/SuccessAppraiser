using FluentValidation;

namespace SuccessAppraiser.Api.Goal.Contracts.Validation
{
    public class UpdateGoalDateValidator : AbstractValidator<UpdateGoalDateDto>
    {
        public UpdateGoalDateValidator()
        {
            RuleFor(x => x.Date).NotNull();
            RuleFor(x => x.Comment).MaximumLength(1024);
            RuleFor(x => x.StateId).NotNull();
        }
    }
}
