using MediatR;
using Quizza.Common.Results;
using Quizza.Users.Application.Commands;
using Quizza.Users.Application.Options;
using Quizza.Users.Application.Constants;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Domain.Models;
using Microsoft.EntityFrameworkCore;
using JwtFactory;
using AutoMapper;
using Quizza.Users.Application.Contracts;

namespace Quizza.Users.Application.PipelineBehaviours
{
    public class AdminInitializationBehavior : LoginManager, IPipelineBehavior<SignUpCommand, Result<LoginResponse>>
    {
        private readonly InitializationOptions options;
        private readonly UserDbContext dbContext;

        public AdminInitializationBehavior(
            UserDbContext dbContext,
            JwtProvider jwtProvider,
            IMapper mapper,                                           
            InitializationOptions initializationOptions,                                           
            UserPolicyOptions userPolicyOptions)
            :base(jwtProvider, mapper, userPolicyOptions)
        {
            this.dbContext = dbContext;
            options = initializationOptions;
        }

        public async Task<Result<LoginResponse>> Handle(SignUpCommand request, RequestHandlerDelegate<Result<LoginResponse>> next, CancellationToken cancellationToken)
        {
            // Run the normal handler and get the result
            var userResult = await next.Invoke();

            // If initialization is disabled OR the signup operation failed, THEN return
            if (options.Enabled is false || userResult is { IsFailure: true } || userResult.Value == null)
                return userResult;

            // If the user is not an ADMIN, return
            var isAdmin = options.AdminUsers?.Contains(userResult.Value.Email, StringComparer.OrdinalIgnoreCase);
            if(isAdmin is not true)
                return userResult;

            // If we got here - this user should be configured for admin roles
            var userProfile = await dbContext.Users.Include("_roles")
                .FirstOrDefaultAsync(x => x.Id == userResult.Value.Id, cancellationToken);

            if (userProfile is null)
            {
                return userResult;
            }

            userProfile.AddRole(Roles.Admin);
            await dbContext.SaveChangesAsync(cancellationToken);

            // Regenerate the auth token            
            return userResult with { Value = CreateLogin(userProfile) };
        }
    }
}
