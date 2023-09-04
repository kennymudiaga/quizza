using FluentValidation;
using Quizza.Users.Application.Commands;
using Quizza.Users.Application.Constants;

namespace Quizza.Users.Application.Validators;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();            
        RuleFor(x => x.Password).NotEmpty().MaximumLength(100)
            .Matches(ValidationConstants.PasswordRegex).WithMessage(ValidationConstants.PasswordStrengthError);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage(ValidationConstants.PasswordConfirmationError);
        RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(100);
    }
}
