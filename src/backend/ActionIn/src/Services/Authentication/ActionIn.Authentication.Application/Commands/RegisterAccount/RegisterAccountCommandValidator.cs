using ActionIn.Authentication.Application.Validation;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.ValueObjects;
using FluentValidation;

namespace ActionIn.Authentication.Application.Commands.RegisterAccount;

public class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
{
    public RegisterAccountCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(c => c.Email)
            .NotEmpty().WithError(AuthenticationErrors.Email.Empty)
            .MaximumLength(Email.MaxLength).WithError(AuthenticationErrors.Email.TooLong);

        RuleFor(c => c.Username)
            .NotEmpty().WithError(AuthenticationErrors.Username.Empty)
            .MinimumLength(Username.MinLength).WithError(AuthenticationErrors.Username.TooShort)
            .MaximumLength(Username.MaxLength).WithError(AuthenticationErrors.Username.TooLong);

        RuleFor(c => c.Password)
            .NotEmpty().WithError(AuthenticationErrors.Password.Empty)
            .MinimumLength(Password.MinLength).WithError(AuthenticationErrors.Password.TooShort)
            .MaximumLength(Password.MaxLength).WithError(AuthenticationErrors.Password.TooLong);
    }
}

