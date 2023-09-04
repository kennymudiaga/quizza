using MediatR;
using Quizza.Common.Results;

namespace Quizza.Users.Application.Commands;

public record ChangePasswordCommand : IRequest<Result>
{
    public Guid? UserId { get; set; }
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
