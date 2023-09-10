using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Users.Application.Constants;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Commands;

public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, Result>
{
    private readonly UserDbContext dbContext;
    private readonly IPasswordHasher<UserProfile> passwordHasher;

    public SetPasswordCommandHandler(UserDbContext dbContext, IPasswordHasher<UserProfile> passwordHasher)
    {
        this.dbContext = dbContext;
        this.passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await dbContext.Users
            .FirstOrDefaultAsync(
                x => x.Email == request.Email && x.PasswordToken == request.Token,
                cancellationToken);
        if (userProfile is null)
            return new Failure(LoginErrors.InvalidCredentials);
        if (userProfile.IsPasswordTokenExpired)
            return new Failure(LoginErrors.TokenExpired);

        var passwordHash = passwordHasher.HashPassword(userProfile, request.Password ?? throw new InvalidOperationException("Password cannot be null"));
        userProfile.SetPassword(passwordHash);
        await dbContext.SaveChangesAsync(CancellationToken.None);

        return new Success();
    }
}
