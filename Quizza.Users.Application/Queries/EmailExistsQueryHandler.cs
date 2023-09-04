using MediatR;
using Microsoft.EntityFrameworkCore;
using Quizza.Common.Results;
using Quizza.Users.Application.Infrastructure;

namespace Quizza.Users.Application.Queries;

public class EmailExistsQueryHandler : IRequestHandler<EmailExistsQuery, Result<bool>>
{
    private readonly UserDbContext dbContext;

    public EmailExistsQueryHandler(UserDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Result<bool>> Handle(EmailExistsQuery request, CancellationToken cancellationToken)
    {
        var emailExists = await dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        return new Success<bool>(emailExists);
    }
}
