using FluentValidation;
using MediatR;
using Quizza.Common.Results;

namespace Quizza.Common.PipelineBehaviours;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result, new()
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators?.ToList() ?? new();
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var errors = new Dictionary<string, List<string>>();
        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            var data = validationResult.ToDictionary();
            foreach (var item in data)
            {
                if (errors.ContainsKey(item.Key))
                {
                    errors[item.Key].AddRange(item.Value);
                }
                else
                {
                    errors[item.Key] = new List<string>(item.Value);
                }
            }
        }

        if (errors.Count > 0)
        {
            return new TResponse { Errors = errors, Message = Failure.DefaultValidationMessage };
        }

        return await next();
    }
}
