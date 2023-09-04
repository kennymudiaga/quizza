using Quizza.Users.Domain.Models.Entities;
using System.Security.Claims;

namespace Quizza.Users.Application.Extensions;

public static class AuthExtensions
{
    public static List<Claim> GetClaims(this UserProfile userProfile)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, userProfile.Name),
            new(ClaimTypes.Email, userProfile.Email),
            new(ClaimTypes.Sid, userProfile.Id.ToString()),
            new(ClaimTypes.GivenName, userProfile.FirstName ?? ""),
            new(ClaimTypes.Surname, userProfile.LastName ?? ""),
        };

        var roleClaims = userProfile.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role));
        claims.AddRange(roleClaims);

        return claims;
    }
}
