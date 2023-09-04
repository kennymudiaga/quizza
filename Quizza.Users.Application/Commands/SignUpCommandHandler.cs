using MediatR;
using Microsoft.AspNetCore.Identity;
using Quizza.Common.Results;
using Quizza.Users.Domain.Models.Entities;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Domain.Models;
using AutoMapper;
using JwtFactory;
using Quizza.Users.Application.Contracts;
using Quizza.Users.Application.Options;

namespace Quizza.Users.Application.Commands;

public class SignUpCommandHandler : LoginManager, IRequestHandler<SignUpCommand, Result<LoginResponse>>
{
    private readonly UserDbContext dbContext;
    private readonly IPasswordHasher<UserProfile> passwordHasher;
    private readonly IMapper mapper;
    private readonly JwtProvider jwtProvider;

    public SignUpCommandHandler(UserDbContext dbContext,
        IPasswordHasher<UserProfile> passwordHasher,
        IMapper mapper,
        JwtProvider jwtProvider,
        UserPolicyOptions userPolicyOptions)
        : base(jwtProvider, mapper, userPolicyOptions)
    {
        this.dbContext = dbContext;
        this.passwordHasher = passwordHasher;
        this.mapper = mapper;
        this.jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponse>> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = new UserProfile(mapper.Map<SignUpRequest>(request));
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.SetPassword(passwordHasher.HashPassword(user, request.Password));
        }

        dbContext.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new Success<LoginResponse>(CreateLogin(user));
    }
}
