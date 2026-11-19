using System.Text.RegularExpressions;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.ValueObjects;

public sealed partial record Username
{
    public const int MinLength = 4;
    public const int MaxLength = 30;
    public string Text { get; private init; } = string.Empty;

    private Username() { }

    private Username(string username)
    {
        Text = username;
    }

    public static Result<Username> Create(string? username)
    {
        string normalizedUsername = Normalize(username);

        Result validation = Validate(normalizedUsername);
        if (validation.IsFailure)
            return Result<Username>.Failure(validation.Error);

        return Result<Username>.Success(new Username(normalizedUsername));
    }

    private static string Normalize(string? username)
        => username?.Trim() ?? string.Empty;

    private static Result Validate(string username)
    {
        if (username.Length == 0)
            return Result.Failure(AuthenticationErrors.Username.Empty);

        if (username.Length < MinLength)
            return Result.Failure(AuthenticationErrors.Username.TooShort);

        if (username.Length > MaxLength)
            return Result.Failure(AuthenticationErrors.Username.TooLong);

        if (!AllowedCharacters().IsMatch(username))
            return Result.Failure(AuthenticationErrors.Username.InvalidCharacter);

        return Result.Success();
    }

    [GeneratedRegex(@"^[a-zA-Z0-9._-]+\z", RegexOptions.None, matchTimeoutMilliseconds: 100)]
    private static partial Regex AllowedCharacters();
}
