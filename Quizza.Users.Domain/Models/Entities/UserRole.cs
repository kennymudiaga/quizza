namespace Quizza.Users.Domain.Models.Entities;

public class UserRole
{
    protected UserRole() { }
    public UserRole(Guid userId, string role)
    {
        UserProfileId = userId;
        Role = role;
    }

    public Guid UserProfileId { get; protected set; }
    public string Role { get; protected set; } = string.Empty;
}
