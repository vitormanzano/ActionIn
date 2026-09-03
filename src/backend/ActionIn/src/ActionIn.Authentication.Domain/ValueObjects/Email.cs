using System.Text.RegularExpressions;

namespace ActionIn.Authentication.Domain.ValueObjects;

public record Email
{
    public string Value { get; private set; } = string.Empty;

    public Email(string email)
    {
        SetEmail(email);
    }

    public void SetEmail(string email)
    {
        if (!EmailIsValid(email))
            throw new Exception("Email is not valid");
        Value = email.Trim().ToLowerInvariant();

    }

    private static bool EmailIsValid(string email)
    {
        if (!string.IsNullOrWhiteSpace(email))
            return false;

        return Regex.IsMatch(email,
                   @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
