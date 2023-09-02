using Quizza.Users.Domain.Commands;

namespace Quizza.Users.Domain.Models;

public record UserProfile
{
    /// <summary>
    /// Protected constructor for Db tools (Mongo or EF)
    /// </summary>
    protected UserProfile() : base()
    {
        Email = string.Empty;
        Id = Guid.NewGuid();
        _roles = new();
    }

    public UserProfile(SignUpCommand model)
        : this()
    {
        FirstName = model.FirstName?.Trim().ToLower();
        LastName = model.LastName?.Trim().ToLower();
        if (string.IsNullOrEmpty(model.Email))
            throw new ArgumentException("Email cannot be empty!");
        Email = model.Email!.Trim().ToLower();
        OtherNames = model.OtherNames?.Trim().ToLower();
        Phone = model.Phone;
        DateCreated = DateTime.UtcNow;
        DateOfBirth = model.DateOfBirth;
        Gender = string.IsNullOrWhiteSpace(model.Gender) ? null : model.Gender.ToUpper();
    }

    public Guid Id { get; set; }
    public string? FirstName { get; protected set; }
    public string? LastName { get; protected set; }
    public string? PasswordHash { get; protected set; }
    public string? OtherNames { get; protected set; }
    public string Email { get; protected set; }
    public string? Phone { get; protected set; }
    public int AccessFailedCount { get; set; }
    public bool IsAccountLocked { get; set; }
    public DateTime? LockoutExpiry { get; set; }
    public DateTime DateCreated { get; protected set; }
    public DateTime? LastPasswordChange { get; protected set; }
    public string? PasswordToken { get; set; }
    public DateTime? PasswordTokenExpiry { get; set; }
    public Guid? CreatorId { get; set; }
    public string? Gender { get; protected set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? DateOfBirth { get; set; }

    public string Name => $"{FirstName}{MiddleNameOrSpace}{LastName}";
    private string MiddleNameOrSpace => string.IsNullOrWhiteSpace(OtherNames) ? " " : $" {OtherNames} ";

    private List<UserRole> _roles;

    public IReadOnlyList<UserRole> Roles => _roles;    

    public bool IsPasswordTokenExpired() =>
        (!string.IsNullOrEmpty(PasswordToken)) &&
        PasswordTokenExpiry.HasValue &&
        DateTime.UtcNow > PasswordTokenExpiry;

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        PasswordToken = null;
        PasswordTokenExpiry = null;
        LastPasswordChange = DateTime.UtcNow;
    }

    public UserRole AddRole(string roleName)
    {
        var role = new UserRole(Id, roleName);
        if (_roles.Any(r => r.Role.Equals(roleName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"User already in role '{roleName}'");
        _roles.Add(role);
        return role;
    }
}
