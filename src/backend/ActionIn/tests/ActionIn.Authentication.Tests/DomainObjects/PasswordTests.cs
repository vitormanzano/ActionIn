namespace ActionIn.Authentication.Tests.DomainObjects;

using Domain.ValueObjects;
using Xunit;

public class PasswordTests
{
    [Fact]
    public void Validate_ValidPassword_DoesNotThrow()
    {
        var exception = Record.Exception(() => Password.Validate("senha123"));

        Assert.Null(exception);
    }

    [Fact]
    public void Validate_PasswordShorterThanFourCharacters_ThrowsException()
    {
        Assert.Throws<Exception>(() => Password.Validate("abc"));
    }

    [Fact]
    public void Validate_NullPassword_ThrowsException()
    {
        Assert.Throws<Exception>(() => Password.Validate(null!));
    }

    [Fact]
    public void FromHash_StoresValueAsIs()
    {
        var password = Password.FromHash("hashed:senha123");

        Assert.Equal("hashed:senha123", password.Value);
    }
}
