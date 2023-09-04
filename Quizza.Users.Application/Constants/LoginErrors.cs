namespace Quizza.Users.Application.Constants;

public static class LoginErrors
{
    public const string AccountLockedFormat = "Account locked. Try again in {0:N0} minutes.";
    public const string InvalidLoginAttempt = "Invalid login credentials.";
    public const string FailedLoginCountFormat = "Invalid login credentials. {0} attempts remaining.";
    public const string PasswordSetupRequired = "Login failed. Password setup or reset required.";
}
