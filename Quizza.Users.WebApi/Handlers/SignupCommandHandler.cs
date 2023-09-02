using MediatR;
using Quizza.Common.Results;
using Quizza.Users.Domain.Commands;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.WebApi.Handlers;

public class SignupCommandHandler : IRequestHandler<SignUpCommand, Result<UserProfile>>
{
    public async Task<Result<UserProfile>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        return new Success<UserProfile>();
    }
}
