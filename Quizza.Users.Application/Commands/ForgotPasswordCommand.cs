using MediatR;
using Quizza.Common.Results;

namespace Quizza.Users.Application.Commands;

public record ForgotPasswordCommand : IRequest<Result>
{
    public string? Email { get; set; }
}
