using System.Text.RegularExpressions;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.ValueObjects;

public sealed partial record Email
{
    public const int MaxLength = 254;
    public string Value { get; private init; } = string.Empty;

    private Email() { }

    private Email(string email)
    {
        Value = email;
    }

    public static Result<Email> Create(string? email)
    {
        string normalizedEmail = Normalize(email);

        Result validation = Validate(normalizedEmail);
        if (validation.IsFailure)
            return Result<Email>.Failure(validation.Error);

        return Result<Email>.Success(new Email(normalizedEmail));
    }

    private static string Normalize(string? email)
        => email?.Trim().ToLowerInvariant() ?? string.Empty;
    private static Result Validate(string email)
    {
        if (email.Length == 0)
            return Result.Failure(AuthenticationErrors.Email.Empty);

        if (email.Length > MaxLength)
            return Result.Failure(AuthenticationErrors.Email.TooLong);

        if (!EmailFormat().IsMatch(email))
            return Result.Failure(AuthenticationErrors.Email.InvalidFormat);

        return Result.Success();
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.None, matchTimeoutMilliseconds: 100)]
    private static partial Regex EmailFormat();

}
