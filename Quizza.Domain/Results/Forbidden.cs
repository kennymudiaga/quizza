namespace Quizza.Common.Results;

public record Forbidden : Failure
{
    public Forbidden(string message) : base(message)
    {
    }
}
