using FluentValidation.Results;

namespace Quizza.Common.Results
{
    public record Result
    {
        public virtual bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public bool IsFailure => !IsSuccess;

        public static Result Success() => new() { IsSuccess = true };

        public static Result Failure(string error) => new() { Message = error };

        public static Result Failure(Dictionary<string, List<string>> errors, string? message = null)
            => new()
            {
                IsSuccess = false,
                Message = message ?? "One or more validation errors occured.",
                Errors = errors,
            };

        public Result<TOut> ToResult<TOut>() => new()
        {
            Errors = Errors,
            IsSuccess = IsSuccess,
            Message = Message,
        };
    }

    public record Result<T> : Result
    {
        public T? Value { get; set; }

        public static implicit operator Result<T>(Failure failure)
            => new()
            {
                Errors = failure.Errors,
                Message = failure.Message,
            };
    }
}
