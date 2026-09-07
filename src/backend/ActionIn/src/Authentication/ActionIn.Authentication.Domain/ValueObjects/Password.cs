namespace ActionIn.Authentication.Domain.ValueObjects;

using ActionIn.Authentication.Domain.Hasher;

public record Password
{
    public string Value { get; private set; } = string.Empty;

    private Password() { }

    public static Password Create(string rawPassword, IPasswordHasher hasher)
    {
        Validate(rawPassword);
        return new Password { Value = hasher.Hash(rawPassword) };
    }

    public static Password FromHash(string hash) => new Password { Value = hash };

    public bool Verify(string rawPassword, IPasswordHasher hasher) => hasher.Verify(rawPassword, Value);

    public static void Validate(string password)
    {
        if (password.Length < 4)
            throw new Exception("Password must be at least 4 characters!");
    }

}
