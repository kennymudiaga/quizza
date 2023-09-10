namespace Quizza.Users.Application.Constants;

public static class LoginErrors
{
    public const string AccountLockedFormat = "Account locked. Try again in {0:N0} minutes.";
    public const string InvalidCredentials = "Invalid credentials.";
    public const string FailedLoginCountFormat = "Invalid credentials. {0} attempts remaining.";
    public const string PasswordSetupRequired = "Login failed. Password setup or reset required.";
    public const string TokenExpired = "The token provided has expired.";
}
