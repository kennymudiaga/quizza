using MediatR;
using Quizza.Common.Results;

namespace Quizza.Users.Domain.Commands;

public record ResetPasswordCommand : IRequest<Result<bool>>
{
    public string? Email { get; set; }
}
