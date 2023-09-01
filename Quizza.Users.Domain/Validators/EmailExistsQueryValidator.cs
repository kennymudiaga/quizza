using FluentValidation;
using Quizza.Users.Domain.Queries;

namespace Quizza.Users.Domain.Validators;

public class EmailExistsQueryValidator : AbstractValidator<EmailExistsQuery>
{
    public EmailExistsQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(50);
    }
}
