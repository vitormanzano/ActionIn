using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Authentication.Tests.UnitTests.Infrastructure;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Tests.UnitTests;

public class AccountTests
{

    private readonly FakePasswordHasher _hasher = new();

    [Fact]
    public void Register_ValidData_ReturnsAccount()
    {
        Result<Account> result = Account.Register("vitor", "vitor@example.com", "secret123", _hasher);

        Assert.Equal("vitor", result.Value.Username.Text);
        Assert.Equal("vitor@example.com", result.Value.Email.Address);
        Assert.Equal("hashed:secret123", result.Value.Password.Hash);
    }

    [Fact]
    public void Register_InvalidUsername_ReturnsUsernameFieldError()
    {
        Result<Account> result = Account.Register("ab", "vitor@example.com", "secret123", _hasher);

        Assert.True(result.IsFailure);
        Assert.Equal(AuthenticationErrors.Account.InvalidData([]).Code, result.Error.Code);

        FieldError fieldError = Assert.Single(result.Error.FieldErrors);
        Assert.Equal(nameof(Account.Username), fieldError.Field);
        Assert.Equal(AuthenticationErrors.Username.TooShort.Code, fieldError.Code);
    }

    [Fact]
    public void Register_AllFieldsInvalid_ReturnsOneFieldErrorPerField()
    {
        Result<Account> result = Account.Register("ab", "invalid", "abc", _hasher);

        Assert.True(result.IsFailure);
        Assert.Equal(3, result.Error.FieldErrors.Count);
        Assert.Contains(result.Error.FieldErrors, f =>
            f.Field == nameof(Account.Username) && f.Code == AuthenticationErrors.Username.TooShort.Code);
        Assert.Contains(result.Error.FieldErrors, f =>
            f.Field == nameof(Account.Email) && f.Code == AuthenticationErrors.Email.InvalidFormat.Code);
        Assert.Contains(result.Error.FieldErrors, f =>
            f.Field == nameof(Account.Password) && f.Code == AuthenticationErrors.Password.TooShort.Code);
    }

    [Fact]
    public void Register_InvalidEmailWithValidPassword_StillHashesPassword()
    {
        Account.Register("vitor", "invalid", "secret123", _hasher);

        Assert.Equal(1, _hasher.HashCalls);
    }
}
