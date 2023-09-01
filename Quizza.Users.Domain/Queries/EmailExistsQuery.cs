using MediatR;
using Quizza.Common.Results;

namespace Quizza.Users.Domain.Queries;

public record EmailExistsQuery : IRequest<Result<bool>>
{
    public string? Email { get; set; }
}
