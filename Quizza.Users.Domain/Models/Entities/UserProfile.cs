using Quizza.Users.Domain.Constants;

namespace Quizza.Users.Domain.Models.Entities;

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

    public UserProfile(SignUpRequest model)
        : this()
    {
        FirstName = model.FirstName?.Trim().ToLower();
        LastName = model.LastName?.Trim().ToLower();
        if (string.IsNullOrEmpty(model.Email))
            throw new ArgumentException(ModelErrors.EmptyEmail);
        Email = model.Email.Trim().ToLower();
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
    public int AccessFailedCount { get; protected set; }    
    public DateTime? LockoutExpiry { get; protected set; }
    public DateTime DateCreated { get; protected set; }
    public DateTime? LastPasswordChange { get; protected set; }
    public string? PasswordToken { get; protected set; }
    public DateTime? PasswordTokenExpiry { get; protected set; }
    public Guid? CreatorId { get; set; }
    public string? Gender { get; protected set; }
    public DateTime? LastLogin { get; protected set; }
    public DateTime? DateOfBirth { get; set; }

    public bool IsAccountLocked => LockoutExpiry.HasValue && LockoutExpiry.Value > DateTime.UtcNow;

    public string Name => $"{FirstName}{MiddleNameOrSpace}{LastName}";
    private string MiddleNameOrSpace => string.IsNullOrWhiteSpace(OtherNames) ? " " : $" {OtherNames} ";

    private List<UserRole> _roles;

    public IReadOnlyList<UserRole> Roles => _roles ?? new List<UserRole>();

    public bool IsPasswordTokenExpired =>
        !string.IsNullOrEmpty(PasswordToken) &&
        PasswordTokenExpiry.HasValue &&
        DateTime.UtcNow > PasswordTokenExpiry;

    public void SetPassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        PasswordToken = null;
        PasswordTokenExpiry = null;
        LastPasswordChange = DateTime.UtcNow;

        // Unlock account
        LockoutExpiry = null;
    }

    public void SetPasswordToken(string token, int expiryMinutes)
    {
        PasswordToken = token;
        PasswordTokenExpiry = DateTime.UtcNow.AddMinutes(expiryMinutes);
    }

    public UserRole AddRole(string roleName)
    {
        var role = new UserRole(Id, roleName);
        if (_roles.Any(r => r.Role.Equals(roleName, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"User already in role '{roleName}'");
        _roles.Add(role);
        return role;
    }

    public void LogAccessFailure(bool lockoutEnabled, int maxFailCount, int lockoutMinutes)
    {
        AccessFailedCount++;
        if (AccessFailedCount >= maxFailCount)
        {
            LockoutExpiry = lockoutEnabled ? DateTime.UtcNow.AddMinutes(lockoutMinutes) : null;
        }
    }

    public void LogAccessSuccess()
    {
        LastLogin = DateTime.UtcNow;
        AccessFailedCount = 0;
        LockoutExpiry = null;
    }
}
