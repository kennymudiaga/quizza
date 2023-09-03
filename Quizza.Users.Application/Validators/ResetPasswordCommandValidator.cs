using FluentValidation;
using Quizza.Users.Application.Commands;

namespace Quizza.Users.Application.Validators;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    }
}
