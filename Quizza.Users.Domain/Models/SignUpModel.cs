namespace Quizza.Users.Domain.Models;

public record SignUpModel
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
