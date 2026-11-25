using ActionIn.Core.Results;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ActionIn.Core.Infrastructure.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResultType<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        ValidationResult[] validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(request, cancellationToken)));

        List<FieldError> fieldErrors = validationResults
            .SelectMany(validationResult => validationResult.Errors)
            .Select(failure => new FieldError(
                failure.PropertyName,
                failure.ErrorCode,
                failure.ErrorMessage))
            .ToList();

        if (fieldErrors.Count == 0)
            return await next(cancellationToken);

        Error error = Error.Validation(
            code: "Validation.Failed",
            message: "One or more validation errors occurred.",
            fieldErrors: fieldErrors);

        return TResponse.Failure(error);
    }
}
