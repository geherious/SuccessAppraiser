using FluentValidation;
using SuccessAppraiser.Contracts.Auth;

namespace SuccessAppraiser.Validation.Auth
{
    public class NewRegisterValidator : AbstractValidator<NewRegisterDto>
    {
        public NewRegisterValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(3).WithMessage("Username minimum length is 3")
                .MaximumLength(25).WithMessage("Username maximum length is 25");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password minimum length is 6")
                .Matches(@"[0-9]+").WithMessage("Password should contain at least one digit")
                .Matches(@"[a-zA-z]").WithMessage("Password should contain at least one letter");
        }
    }
}
