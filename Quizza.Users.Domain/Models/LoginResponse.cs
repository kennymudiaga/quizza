namespace Quizza.Users.Domain.Models;

public record LoginResponse
{
   public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Roles { get; set; }
    public DateTime ExpiryDate { get; set; }
};