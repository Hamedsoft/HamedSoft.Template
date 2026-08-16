using FluentValidation;
using HamedSoft.Template.Domain.SeedWork;
using MediatR;

namespace HamedSoft.Template.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(
        IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(
                validator => validator.ValidateAsync(
                    context,
                    cancellationToken)));

        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count == 0)
            return await next();

        var error = string.Join(
            Environment.NewLine,
            failures.Select(failure =>
                $"{failure.PropertyName}: {failure.ErrorMessage}"));

        if (typeof(TResponse) == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var failureMethod = typeof(TResponse)
                .GetMethod(
                    nameof(Result<object>.Failure),
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Static);

            if (failureMethod is not null)
            {
                return (TResponse)failureMethod.Invoke(
                    null,
                    [error])!;
            }
        }

        throw new ValidationException(failures);
    }
}