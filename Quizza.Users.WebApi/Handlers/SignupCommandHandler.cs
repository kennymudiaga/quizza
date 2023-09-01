using MediatR;
using Quizza.Common.Results;
using Quizza.Users.Domain.Commands;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.WebApi.Handlers;

public class SignupCommandHandler : IRequestHandler<SignUpCommand, Result<UserProfile>>
{
    public Task<Result<UserProfile>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
