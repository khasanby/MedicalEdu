using FluentValidation;
using MediatR;
using MedicalEdu.Application.Common.Results;

namespace MedicalEdu.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            // Handle Result<T> pattern
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var validationFailureMethod = typeof(Result<>).MakeGenericType(resultType)
                    .GetMethod("ValidationFailure", new[] { typeof(List<string>) });
                
                var errorMessages = failures.Select(f => f.ErrorMessage).ToList();
                var result = validationFailureMethod!.Invoke(null, new object[] { errorMessages });
                
                return (TResponse)result!;
            }
            
            // Fallback to exception for non-Result types
            throw new ValidationException(failures);
        }

        return await next();
    }
}