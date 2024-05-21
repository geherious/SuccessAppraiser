using FluentValidation;

namespace Api.Goal.Contracts.Validation
{
    public class CreateGoalDateValidator : AbstractValidator<CreateGoalDateDto>
    {
        public CreateGoalDateValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.StateId).NotEmpty();
            RuleFor(x => x.GoalId).NotEmpty();
        }
    }
}
