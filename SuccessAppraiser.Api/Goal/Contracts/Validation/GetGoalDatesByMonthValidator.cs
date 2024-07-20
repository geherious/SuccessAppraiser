using FluentValidation;

namespace SuccessAppraiser.Api.Goal.Contracts.Validation
{
    public class GetGoalDatesByMonthValidator : AbstractValidator<GetGoalDatesByMonthDto>
    {
        public GetGoalDatesByMonthValidator()
        {
            RuleFor(x => x.GoalId).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
        }
    }
}
