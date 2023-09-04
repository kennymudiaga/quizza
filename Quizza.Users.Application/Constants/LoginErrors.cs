namespace Quizza.Users.Application.Constants;

public static class LoginErrors
{
    public const string AccountLockedFormat = "Account locked. Try again in {0:N0} minutes.";
    public const string InvalidLoginAttempt = "Invalid email or password.";
    public const string FailedLoginCountFormat = "Invalid email or password. {0} attempts remaining.";
    public const string PasswordSetupRequired = "Login failed. Password setup or reset required.";
}
