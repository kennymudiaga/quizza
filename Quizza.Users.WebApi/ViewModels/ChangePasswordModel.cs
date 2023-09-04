namespace Quizza.Users.WebApi.ViewModels;

public record ChangePasswordModel
{
    public string? OldPassword { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
}
