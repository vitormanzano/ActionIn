using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Tests.UnitTests.DomainObjects;

public class UsernameTests
{
    [Fact]
    public void Create_ValidUsername_ReturnsSuccess()
    {
        Result<Username> result = Username.Create("vitor");

        Assert.True(result.IsSuccess);
        Assert.Equal("vitor", result.Value.Text);
    }

    [Theory]
    [InlineData("  vitor   ")]
    [InlineData("vitor  ")]
    [InlineData("  vitor")]
    public void Create_UsernameWithSurroundingSpaces_TrimsSpaces(string username)
    {
        Result<Username> result = Username.Create(username);

        Assert.True(result.IsSuccess);
        Assert.Equal("vitor", result.Value.Text);
    }

    [Theory]
    [InlineData("vitor_123")]
    [InlineData("vitor.m")]
    [InlineData("vitor-m")]
    [InlineData("vitor123")]
    [InlineData("1234")]
    [InlineData("VITOR")]
    public void Create_UsernameWithAllowedCharacters_ReturnsSuccess(string username)
    {
        Result<Username> result = Username.Create(username);

        Assert.True(result.IsSuccess);
        Assert.Equal(username, result.Value.Text);
    }

    [Theory]
    [InlineData("vitor#")]
    [InlineData("vitor$")]
    [InlineData("vi   tor")]
    [InlineData("joão")]
    [InlineData("vitor@m")]
    public void Create_UsernameWithInvalidCharacters_ReturnsFailure(string username)
    {
        Result<Username> result = Username.Create(username);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Username.InvalidCharacter.Code, result.Error.Code);
    }

    [Fact]
    public void Create_UsernameTooShortWithInvalidCharacter_ReturnsTooShort()
    {
        string username = new string('a', Username.MinLength - 2) + '#';

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Username.TooShort.Code, result.Error.Code);
    }

    [Fact]
    public void Create_UsernameTooLongWithInvalidCharacter_ReturnsTooLong()
    {
        string username = new string('a', Username.MaxLength) + '#';

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Username.TooLong.Code, result.Error.Code);
    }


    [Fact]
    public void Create_MixedCaseUsername_PreservesCase()
    {
        string username = "Vitor";

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsSuccess);
        Assert.Equal(username, result.Value.Text);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData(null)]
    [InlineData("")]
    public void Create_UsernameIsEmpty_ReturnsFailure(string? username)
    {
        Result<Username> result = Username.Create(username);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Username.Empty.Code, result.Error.Code);
    }

    [Fact]
    public void Create_UsernameShorterThanMinLength_ReturnsFailure()
    {
        string username = new('a', Username.MinLength - 1);

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Username.TooShort.Code, result.Error.Code);
    }

    [Fact]
    public void Create_UsernameWithMinLength_ReturnsSuccess()
    {
        string username = new('a', Username.MinLength);

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsSuccess);
        Assert.Equal(username, result.Value.Text);
    }

    [Fact]
    public void Create_UsernameWithMaxLength_ReturnsSuccess()
    {
        string username = new('a', Username.MaxLength);

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsSuccess);
        Assert.Equal(username, result.Value.Text);
    }

    [Fact]
    public void Create_UsernameLargerThanMaxLength_ReturnsFailure()
    {
        string username = new('a', Username.MaxLength + 1);

        Result<Username> result = Username.Create(username);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Username.TooLong.Code, result.Error.Code);
    }

    [Fact]
    public void Equals_SameValue_ReturnsTrue()
    {
        Result<Username> resultA = Username.Create("vitor");
        Result<Username> resultB = Username.Create("vitor");

        Assert.Equal(resultA.Value, resultB.Value);
    }

    [Fact]
    public void Equals_DifferentValue_ReturnsFalse()
    {
        Result<Username> resultA = Username.Create("vitor");
        Result<Username> resultB = Username.Create("manzano");

        Assert.NotEqual(resultA.Value, resultB.Value);
    }
}
