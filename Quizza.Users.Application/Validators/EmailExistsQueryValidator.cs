using FluentValidation;
using Quizza.Users.Application.Queries;

namespace Quizza.Users.Application.Validators;

public class EmailExistsQueryValidator : AbstractValidator<EmailExistsQuery>
{
    public EmailExistsQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    }
}
