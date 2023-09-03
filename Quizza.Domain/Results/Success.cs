namespace Quizza.Common.Results;

public record Success : Result
{
    public override bool IsSuccess => true;
}

public record Success<T> : Result<T>
{
    public Success(T value)
    {
        Value = value;
    }

    public override bool IsSuccess => true;
}
