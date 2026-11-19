using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.ValueObjects;

namespace ActionIn.Authentication.Tests.UnitTests.DomainObjects;

public class EmailTests
{
    [Fact]
    public void Create_ValidEmail_CreateNewEmail()
    {
        var result = Email.Create("vitor@example.com");
        Assert.True(result.IsSuccess);
        Assert.Equal("vitor@example.com", result.Value.Value);
    }

    [Theory]
    [InlineData("         vitor@example.com   ", "vitor@example.com")]
    [InlineData("Vitor@example.com", "vitor@example.com")]
    public void Create_NormalizeEmail_ReturnsTrimAndLowerEmail(string email, string expected)
    {
        var result = Email.Create(email);
        Assert.True(result.IsSuccess);
        Assert.Equal(expected, result.Value.Value);
    }

    [Fact]
    public void Create_EmailIsNull_ReturnsFailure()
    {
        var result = Email.Create(null);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.Empty.Code, result.Error.Code);
    }

    [Fact]
    public void Create_EmailIsEmpty_ReturnsFailure()
    {
        var result = Email.Create("");
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.Empty.Code, result.Error.Code);
    }

    [Fact]
    public void Create_EmailWithMaxLengthCharacters_ReturnsSuccess()
    {
        const string domain = "@example.com";
        string email = new string('a', Email.MaxLength - domain.Length) + domain;
        var result = Email.Create(email);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_EmailLongerThanMaxLengthCharacters_ReturnsFailure()
    {
        const string domain = "@example.com";
        string email = new string('a', Email.MaxLength - domain.Length + 1) + domain;
        var result = Email.Create(email);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.TooLong.Code, result.Error.Code);
    }


    [Theory]
    [InlineData("@example.com")]
    [InlineData("a@example.com@gmail")]
    [InlineData("a.com@gmail")]
    [InlineData("a.com")]
    [InlineData("a@gmail")]
    public void Create_EmailIsInvalid_ReturnsFailure(string email)
    {
        var result = Email.Create(email);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.InvalidFormat.Code, result.Error.Code);
    }

    [Fact]
    public void Equals_SameValue_IsEqual()
    {
        var resultA = Email.Create("vitor@example.com");
        var resultB = Email.Create("vitor@example.com");

        Assert.Equal(resultA.Value, resultB.Value);
    }

    [Fact]
    public void Equals_DifferentValue_IsNotEqual()
    {
        var resultA = Email.Create("vitor@example.com");
        var resultB = Email.Create("manzano@example.com");

        Assert.NotEqual(resultA.Value, resultB.Value);
    }
}
