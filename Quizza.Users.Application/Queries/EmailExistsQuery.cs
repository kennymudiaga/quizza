using MediatR;
using Quizza.Common.Results;

namespace Quizza.Users.Application.Queries;

public record EmailExistsQuery : IRequest<Result<bool>>
{
    public string? Email { get; set; }
}
