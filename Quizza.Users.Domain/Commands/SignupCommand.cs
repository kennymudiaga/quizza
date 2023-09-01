using MediatR;
using Quizza.Common.Results;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.Domain.Commands;

public record SignUpCommand : IRequest<Result<UserProfile>>
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? OtherNames { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
}
