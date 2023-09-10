using FluentValidation;
using Quizza.Users.Application.Commands;
using Quizza.Users.Application.Constants;

namespace Quizza.Users.Application.Validators;

public class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
{
    public SetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(100)
            .Matches(ValidationConstants.PasswordRegex).WithMessage(ValidationConstants.PasswordStrengthError);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage(ValidationConstants.PasswordConfirmationError);
        RuleFor(x => x.Token).NotEmpty().MaximumLength(100);
    }
}
