namespace Quizza.Common.Results;

public record Unauthorized : Failure
{
    public Unauthorized(string message) : base(message)
    {
    }
}
