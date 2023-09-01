namespace Quizza.Users.WebApi.ViewModels;

public record LoginResponse(string Name, string Email, string Username, string Token, string Roles);
