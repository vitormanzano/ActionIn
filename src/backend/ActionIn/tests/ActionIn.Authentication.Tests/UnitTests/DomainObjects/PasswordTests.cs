using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Authentication.Tests.UnitTests.Infrastructure;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Tests.UnitTests.DomainObjects;

public class PasswordTests
{
    private readonly FakePasswordHasher _hasher = new();

    [Fact]
    public void Create_ValidPassword_StoresHash()
    {
        const string password = "example123";

        Result<Password> result = Password.Create(password, _hasher);

        Assert.True(result.IsSuccess);
        Assert.Equal("hashed:example123", result.Value.Hash);
        Assert.NotEqual(password, result.Value.Hash);
    }

    [Fact]
    public void Create_PasswordIsNull_ReturnsFailure()
    {
        Result<Password> result = Password.Create(null, _hasher);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Password.Empty.Code, result.Error.Code);
        Assert.Equal(0, _hasher.HashCalls);
    }

    [Fact]
    public void Create_PasswordIsEmpty_ReturnsFailure()
    {
        Result<Password> result = Password.Create("", _hasher);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Password.Empty.Code, result.Error.Code);
        Assert.Equal(0, _hasher.HashCalls);
    }

    [Fact]
    public void Create_PasswordShorterThanMinLength_ReturnsFailure()
    {
        string password = new('a', Password.MinLength - 1);

        Result<Password> result = Password.Create(password, _hasher);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Password.TooShort.Code, result.Error.Code);
        Assert.Equal(0, _hasher.HashCalls);
    }

    [Fact]
    public void Create_PasswordWithMinLengthCharacters_ReturnsSuccess()
    {
        string password = new('a', Password.MinLength);
        Result<Password> result = Password.Create(password, _hasher);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_PasswordWithMaxLengthCharacters_ReturnsSuccess()
    {
        string password = new('a', Password.HashMaxLength);
        Result<Password> result = Password.Create(password, _hasher);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Create_PasswordLongerThanMaxLengthCharacters_ReturnsFailure()
    {
        string password = new('a', Password.HashMaxLength + 1);
        Result<Password> result = Password.Create(password, _hasher);
        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Password.TooLong.Code, result.Error.Code);
        Assert.Equal(0, _hasher.HashCalls);
    }

    [Theory]
    [InlineData("    ")]
    [InlineData("   a   ")]
    public void Create_PasswordWithEmptySpaces_ReturnsSuccessWithSpaces(string password)
    {
        Result<Password> result = Password.Create(password, _hasher);
        Assert.True(result.IsSuccess);
        Assert.Contains("hashed:" + password, result.Value.Hash);
        Assert.Equal(1, _hasher.HashCalls);
    }
}
