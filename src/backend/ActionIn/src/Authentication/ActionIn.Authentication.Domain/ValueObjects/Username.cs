namespace ActionIn.Authentication.Domain.ValueObjects;

public record Username
{
    public string Value { get; private set; } = string.Empty;

    public Username(string username)
    {
        SetUsername(username);
    }

    public void SetUsername(string username)
    {
        ValidateUsername(username);
        Value = username;
    }

    private void ValidateUsername(string username)
    {
        if (username is null)
            throw new Exception("Username cannot be null"); // Use notifications + result pattern
        if (username.Length < 4)
            throw new Exception("Username must be at least 4 characters");
    }
}
