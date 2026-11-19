using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Domain.ValueObjects;

public sealed record Password
{
    public const int MinLength = 4;
    public const int MaxLength = 128; // Password larger than that will run with 100000 iterations
    public string Hash { get; private init; } = string.Empty;

    private Password() { }

    private Password(string hash)
    {
        Hash = hash;
    }

    public static Result<Password> Create(string? password, IPasswordHasher hasher)
    {
        string plainText = password ?? string.Empty;

        Result validation = Validate(plainText);
        if (validation.IsFailure)
            return Result<Password>.Failure(validation.Error);

        string hashed = hasher.Hash(plainText);
        return Result<Password>.Success(new Password(hashed));
    }

    private static Result Validate(string password)
    {
        if (password.Length == 0)
            return Result.Failure(AuthenticationErrors.Password.Empty);

        if (password.Length < MinLength)
            return Result.Failure(AuthenticationErrors.Password.TooShort);

        if (password.Length > MaxLength)
            return Result.Failure(AuthenticationErrors.Password.TooLong);

        return Result.Success();
    }

    public override string ToString()
    {
        return "Password:  *** ";
    }
}

