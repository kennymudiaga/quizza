using Microsoft.AspNetCore.Identity;
using Quizza.Users.Application.Constants;
using Quizza.Users.Application.Options;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Extensions;

internal static class PasswordHasherExtensions
{
    internal static (bool success, string message) CheckPassword(
        this IPasswordHasher<UserProfile> passwordHasher,
        UserProfile userProfile,
        UserPolicyOptions userPolicy,
        string providedPassword)
    {
        if (string.IsNullOrWhiteSpace(userProfile.PasswordHash))
            return (false, LoginErrors.PasswordSetupRequired);

        (bool, string) getLockoutResponse()
        {
            var lockoutExpiry = userProfile.LockoutExpiry 
                ?? DateTime.UtcNow.AddMinutes(userPolicy.PasswordLockoutDuration);
            var lockoutMinutes = Math.Ceiling((lockoutExpiry - DateTime.UtcNow).TotalMinutes);
            return (false, string.Format(LoginErrors.AccountLockedFormat, lockoutMinutes));
        }

        if (userProfile.IsAccountLocked && userProfile.LockoutExpiry > DateTime.UtcNow)
            return getLockoutResponse();

        var passwordResult = passwordHasher.VerifyHashedPassword(
            userProfile, userProfile.PasswordHash, providedPassword);
        if (passwordResult != PasswordVerificationResult.Failed)
        {
            userProfile.LogAccessSuccess();
            return (true, "");
        }            

        if (userPolicy.EnableLockout is false)
            return (false, LoginErrors.InvalidCredentials);

        userProfile.LogAccessFailure(true, userPolicy.MaxPasswordFailCount, userPolicy.PasswordLockoutDuration);

        if (userProfile.IsAccountLocked)
            return getLockoutResponse();

        var attemptsRemaining = userPolicy.MaxPasswordFailCount - userProfile.AccessFailedCount;
        return (false, string.Format(LoginErrors.FailedLoginCountFormat, attemptsRemaining));
    }
}
