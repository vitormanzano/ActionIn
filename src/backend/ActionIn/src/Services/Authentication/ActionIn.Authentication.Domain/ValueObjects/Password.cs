namespace ActionIn.Authentication.Domain.ValueObjects;

public record Password
{
    public string Value { get; private set; } = string.Empty;

    private Password() { }

    public static Password FromHash(string hash) => new Password { Value = hash };


    public static void Validate(string password)
    {
        if (password is null)
            throw new Exception("Password cannot be null");

        if (password.Length < 4)
            throw new Exception("Password must be at least 4 characters!");
    }

}
