namespace Quizza.Users.Domain.Models;

public class UserRole
{
    protected UserRole() { }
    public UserRole(Guid userId, string role)
    {
        UserId = userId;
        Role = role;
    }

    public Guid UserId { get; protected set; }
    public string Role { get; protected set; } = string.Empty;
}
