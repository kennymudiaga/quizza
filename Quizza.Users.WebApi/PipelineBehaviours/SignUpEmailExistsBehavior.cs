using MediatR;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Users.Domain.Commands;
using Quizza.Users.Domain.Models;
using Quizza.Users.WebApi.Infrastructure;

namespace Quizza.Users.WebApi.PipelineBehaviours;

public class SignUpEmailExistsBehavior : IPipelineBehavior<SignUpCommand, Result<UserProfile>>
{
    private readonly UserDbContext dbContext;

    public SignUpEmailExistsBehavior(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<UserProfile>> Handle(SignUpCommand request, RequestHandlerDelegate<Result<UserProfile>> next, CancellationToken cancellationToken)
    {
        var emailExists = await dbContext.Users.AnyAsync(u => u.Email ==  request.Email, cancellationToken);
        return emailExists ?
            new Result<UserProfile> { Message = $"{request.Email} is already in use" } :
            await next();
    }
}
