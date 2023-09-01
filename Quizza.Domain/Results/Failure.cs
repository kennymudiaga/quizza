namespace Quizza.Common.Results;

public record Failure : Result
{
    public const string DefaultValidationMessage = "One or more validation errors occured.";
    public Failure(string message)
    {
        Message = message;
    }

    public Failure(Dictionary<string, List<string>> failures, string? message = null)
    {
        Errors = failures;
        Message = message ?? DefaultValidationMessage;
    }
}
