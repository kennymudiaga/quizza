namespace Quizza.Common.Results;

public record Success : Result
{
    public override bool IsSuccess => true;
}

public record Success<T> : Result<T>
{
    public override bool IsSuccess => true;
}
