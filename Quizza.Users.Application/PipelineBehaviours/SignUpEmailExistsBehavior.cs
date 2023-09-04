using MediatR;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Users.Application.Commands;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.Application.PipelineBehaviours;

public class SignUpEmailExistsBehavior : IPipelineBehavior<SignUpCommand, Result<LoginResponse>>
{
    private readonly UserDbContext dbContext;

    public SignUpEmailExistsBehavior(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<LoginResponse>> Handle(SignUpCommand request, RequestHandlerDelegate<Result<LoginResponse>> next, CancellationToken cancellationToken)
    {
        var emailExists = await dbContext.Users.AnyAsync(u => u.Email ==  request.Email, cancellationToken);
        return emailExists ?
            new Result<LoginResponse> { Message = $"{request.Email} is already in use" } :
            await next();
    }
}
