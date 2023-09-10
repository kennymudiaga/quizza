using FluentValidation;
using Quizza.Users.Application.Commands;

namespace Quizza.Users.Application.Validators;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    }
}
