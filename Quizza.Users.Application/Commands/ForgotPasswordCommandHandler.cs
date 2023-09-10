using Feral.Mailer.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Common.Web.Configuration;
using Quizza.Users.Application.Infrastructure;
using Quizza.Users.Application.Options;

namespace Quizza.Users.Application.Commands;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly UserDbContext dbContext;
    private readonly IEmailService emailService;
    private readonly UserPolicyOptions userPolicy;
    private readonly AppInfoOptions appInfo;
    private readonly Random random;

    public ForgotPasswordCommandHandler(
        UserDbContext dbContext, 
        IEmailService emailService, 
        UserPolicyOptions userPolicy, 
        AppInfoOptions appInfo)
    {
        this.dbContext = dbContext;
        this.emailService = emailService;
        this.userPolicy = userPolicy;
        this.appInfo = appInfo;
        random = new Random();
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await dbContext.Users.Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (userProfile is not null)
        {
            var token = random.Next(100000, 999999).ToString();
            userProfile.SetPasswordToken(token, userPolicy.PasswordTokenTimeout);
            dbContext.Update(userProfile);
            var changeCount = await dbContext.SaveChangesAsync(cancellationToken);
            if (changeCount > 0)
            {
                string plainText = $"Your password reset token is {token}.";
                _ = Task.Run(() => emailService.SendFromTemplateAsync(
                    templateName: "password_reset",
                    data: new { token, username = userProfile.FirstName },
                    subject: $"{appInfo.Name} - Password Reset",
                    plainText: plainText,
                    toEmail: userProfile.Email,
                     fromName: "QuizzaBot"),
                     cancellationToken);
            }
        }

        return new Success();
    }
}
