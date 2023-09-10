using MediatR;
using Quizza.Common.Results;

namespace Quizza.Users.Application.Commands;

public class SetPasswordCommand : IRequest<Result>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Token { get; set; }
}
