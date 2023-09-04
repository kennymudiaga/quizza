namespace Quizza.Users.Application.Constants;

public static class ValidationConstants
{
    public const string PasswordRegex = "^.*(?=.{8,})(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";

    public const string PasswordConfirmationError = "Password and password-confirmation do not match.";
    public const string PasswordStrengthError = "Password must have a minimum length of 8, with 1 each of lower-case, upper-case, numeric, and special characters.";
}
