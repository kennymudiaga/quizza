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
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Commands
{
    public class ChangePasswordCommandHandler : LoginManager, IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly UserDbContext dbContext;
        private readonly IPasswordHasher<UserProfile> passwordHasher;

        public ChangePasswordCommandHandler(
            JwtProvider jwtProvider,
            IMapper mapper,
            UserPolicyOptions userPolicy,
            UserDbContext dbContext,
            IPasswordHasher<UserProfile> passwordHasher)
            : base(jwtProvider, mapper, userPolicy)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userProfile = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);
            if (userProfile == null)
                return new Failure(LoginErrors.InvalidCredentials);

            var (success, message) = passwordHasher.CheckPassword(userProfile, UserPolicy, request.OldPassword ?? "");
            
            if (success)
            {
                userProfile.SetPassword(passwordHasher.HashPassword(userProfile, request.Password!));
            }

            if (dbContext.Entry(userProfile).State == EntityState.Modified)
                await dbContext.SaveChangesAsync(CancellationToken.None);

            return success ? new Success() : new Failure(message);
        }
    }
}
