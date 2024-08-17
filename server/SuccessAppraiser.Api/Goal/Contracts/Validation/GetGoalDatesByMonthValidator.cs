using FluentValidation;

namespace SuccessAppraiser.Api.Goal.Contracts.Validation
{
    public class GetGoalDatesByMonthValidator : AbstractValidator<GetGoalDatesByMonthDto>
    {
        public GetGoalDatesByMonthValidator()
        {
            RuleFor(x => x.DateOfMonth).NotEmpty();
        }
    }
}
