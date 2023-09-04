using MediatR;
using Quizza.Common.Results;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.Application.Commands;

public class LoginCommand : IRequest<Result<LoginResponse>>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
