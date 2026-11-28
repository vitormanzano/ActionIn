using ActionIn.Core.Results;
using FluentValidation;

namespace ActionIn.Authentication.Application.Validation;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule,
        Error error)
        => rule
            .WithErrorCode(error.Code)
            .WithMessage(error.Message);
}
