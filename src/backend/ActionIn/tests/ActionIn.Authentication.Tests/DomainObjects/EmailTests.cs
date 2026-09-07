namespace ActionIn.Authentication.Tests.DomainObjects;

using Domain.ValueObjects;
using Xunit;

public class EmailTests
{
    [Fact]
    public void Constructor_ValidEmail_SetsValue()
    {
        var email = new Email("vitor@gmail.com");
        Assert.Equal("vitor@gmail.com", email.Value);
    }

    [Fact]
    public void Constructor_EmailIsNull_ThrowsException()
    {
        Assert.Throws<Exception>(() => new Email(null!));

    }

    [Theory]
    [InlineData("@gmail.com")]
    [InlineData("a@gmail.com@gmail")]
    [InlineData("a.com@gmail")]
    [InlineData("a.com")]
    [InlineData("a@gmail")]
    [InlineData(" ")]
    public void Constructor_EmailIsInvalid_ThrowsException(string email)
    {
        Assert.Throws<Exception>(() => new Email(email));
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        var a = new Email("vitor@gmail.com");
        var b = new Email("vitor@gmail.com");

        Assert.Equal(a, b);
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        var a = new Email("vitor@gmail.com");
        var b = new Email("manzano@gmail.com");

        Assert.NotEqual(a, b);
    }
}
