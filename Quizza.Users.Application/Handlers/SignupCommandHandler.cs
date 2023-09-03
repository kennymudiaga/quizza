using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizza.Common.Results;
using Quizza.Users.Domain.Models.Entities;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Application.Commands;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.Application.Handlers;

public class SignupCommandHandler : IRequestHandler<SignUpCommand, Result<UserProfile>>
{
    private readonly UserDbContext dbContext;
    private readonly IPasswordHasher<UserProfile> passwordHasher;

    public SignupCommandHandler(UserDbContext dbContext, IPasswordHasher<UserProfile> passwordHasher)
    {
        this.dbContext = dbContext;
        this.passwordHasher = passwordHasher;
    }

    public async Task<Result<UserProfile>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = new UserProfile(new SignUpModel());
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.SetPassword(passwordHasher.HashPassword(user, request.Password));
        }
        
        dbContext.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Success<UserProfile>(user);
    }
}
