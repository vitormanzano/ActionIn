namespace ActionIn.Authentication.Tests.DomainObjects;

using Domain.ValueObjects;
using Xunit;

public class UsernameTests
{
    [Fact]
    public void Constructor_ValidUsername_SetsValue()
    {
        var username = new Username("vitor");

        Assert.Equal("vitor", username.Value);
    }

    [Fact]
    public void Constructor_UsernameIsNull_ThrowsException()
    {
        Assert.Throws<Exception>(() => new Username(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    public void Constructor_UsernameShorterThanFourCharacters_ThrowsException(string username)
    {
        Assert.Throws<Exception>(() => new Username(username));
    }

    [Fact]
    public void Constructor_UsernameWithFourCharacters_DoesNotThrow()
    {
        var exception = Record.Exception(() => new Username("abcd"));

        Assert.Null(exception);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var a = new Username("vitor");
        var b = new Username("vitor");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var a = new Username("vitor");
        var b = new Username("manzano");

        Assert.NotEqual(a, b);
    }
}
