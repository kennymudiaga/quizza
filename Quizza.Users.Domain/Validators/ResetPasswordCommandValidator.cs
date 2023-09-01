using FluentValidation;
using Quizza.Users.Domain.Commands;

namespace Quizza.Users.Domain.Validators;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    }
}
