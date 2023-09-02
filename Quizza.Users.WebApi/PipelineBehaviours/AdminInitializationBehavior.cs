using MediatR;
using Quizza.Common.Results;
using Quizza.Users.Domain.Commands;
using Quizza.Users.Domain.Models;
using Quizza.Users.WebApi.Config;
using Quizza.Users.WebApi.Constants;
using Quizza.Users.WebApi.Infrastructure;

namespace Quizza.Users.WebApi.PipelineBehaviours
{
    public class AdminInitializationBehavior : IPipelineBehavior<SignUpCommand, Result<UserProfile>>
    {
        private readonly InitializationOptions options;
        private readonly UserDbContext dbContext;

        public AdminInitializationBehavior(InitializationOptions options, UserDbContext dbContext)
        {
            this.options = options;
            this.dbContext = dbContext;
        }

        public async Task<Result<UserProfile>> Handle(SignUpCommand request, RequestHandlerDelegate<Result<UserProfile>> next, CancellationToken cancellationToken)
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
            // TODO: Save changes to dbContext
            userResult.Value.AddRole(Roles.Admin);
            return userResult;
        }
    }
}
