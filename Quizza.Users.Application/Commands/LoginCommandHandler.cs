using AutoMapper;
using JwtFactory;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Users.Application.Contracts;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Application.Options;
using Quizza.Users.Domain.Models;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Commands;

public class LoginCommandHandler : LoginManager, IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly UserDbContext userDbContext;
    private readonly IPasswordHasher<UserProfile> passwordHasher;

    public LoginCommandHandler(
        JwtProvider jwtProvider,
        IMapper mapper,
        UserPolicyOptions userPolicy,
        UserDbContext userDbContext,
        IPasswordHasher<UserProfile> passwordHasher)
        : base(jwtProvider, mapper, userPolicy)
    {
        this.userDbContext = userDbContext;
        this.passwordHasher = passwordHasher;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await userDbContext.Users.Include("_roles")
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        if (userProfile == null)
        {
            return new Failure("Invalid email or password");
        }
        else if(userProfile.IsAccountLocked && userProfile.LockoutExpiry > DateTime.UtcNow)
        {
            var lockoutMinutes = (userProfile.LockoutExpiry!.Value - DateTime.UtcNow).TotalMinutes;
            return new Failure($"Account Locked: please retry in {Math.Ceiling(lockoutMinutes):N0} minutes");
        }
        else if (string.IsNullOrWhiteSpace(userProfile.PasswordHash))
        {
            return new Failure("Password setup or reset required");
        }

        var passwordResult = passwordHasher.VerifyHashedPassword(userProfile, userProfile.PasswordHash, request.Password!);
        if (passwordResult == PasswordVerificationResult.Failed)
        {
            userProfile.LogAccessFailure(UserPolicy.EnableLockout, UserPolicy.MaxPasswordFailCount, UserPolicy.PasswordLockoutDuration);
            userDbContext.Update(userProfile);
            await userDbContext.SaveChangesAsync(CancellationToken.None);
            if (UserPolicy.EnableLockout)
            {
                if (userProfile.IsAccountLocked)
                {
                    var lockoutMinutes = (userProfile.LockoutExpiry!.Value - DateTime.UtcNow).TotalMinutes;
                    return new Failure($"Account Locked: please retry in {Math.Ceiling(lockoutMinutes):N0} minutes");
                }

                return new Failure($"Invalid email or password: {UserPolicy.MaxPasswordFailCount - userProfile.AccessFailedCount} attempts remaining.");
            }

            return new Failure("Invalid email or password");
        }

        return new Success<LoginResponse>(CreateLogin(userProfile));
    }
}
