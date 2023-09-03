using FluentValidation;
using Quizza.Users.Application.Commands;

namespace Quizza.Users.Application.Validators;

public class SignupCommandValidator : AbstractValidator<SignUpCommand>
{
    private static readonly string[] genders = { "F", "M" };
    public SignupCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
            .EmailAddress().MaximumLength(100);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.OtherNames).MaximumLength(50);
        RuleFor(x => x.Phone).MaximumLength(20);
        RuleFor(x => x.DateOfBirth).LessThanOrEqualTo(DateTime.UtcNow.Date.AddYears(-10))
            .WithMessage("A user must be at least 10 years old.")
            .When(x => x.DateOfBirth.HasValue);
        RuleFor(x => x.Password).NotEmpty().Length(8, 20);
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
            .WithMessage("Passwords do not match.");
        RuleFor(x => x.Gender).Must(x => genders.Contains(x, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Invalid Gender: must be one of 'F' or 'M'")
            .When(x => !string.IsNullOrEmpty(x.Gender));
    }
}
