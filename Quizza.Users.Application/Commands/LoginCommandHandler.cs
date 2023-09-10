using AutoMapper;
using JwtFactory;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Users.Application.Constants;
using Quizza.Users.Application.Contracts;
using Quizza.Users.Application.Extensions;
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

        if (userProfile is null)
            return new Failure(LoginErrors.InvalidCredentials);

        var (success, message) = passwordHasher.CheckPassword(userProfile, UserPolicy, request.Password ?? "");

        if (userDbContext.Entry(userProfile).State == EntityState.Modified)
            await userDbContext.SaveChangesAsync(CancellationToken.None);

        return success?  
            new Success<LoginResponse>(CreateLogin(userProfile)) :
            new Failure(message);
    }
}
