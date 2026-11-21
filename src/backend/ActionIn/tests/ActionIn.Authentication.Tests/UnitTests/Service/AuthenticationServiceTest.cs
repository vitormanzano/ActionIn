using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Errors;
using ActionIn.Authentication.Domain.Services;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Authentication.Tests.UnitTests.Infrastructure;
using ActionIn.Core.Results;

namespace ActionIn.Authentication.Tests.UnitTests.Service;

public class AuthenticationServiceTest
{
    private const string RegisteredEmail = "vitor@example.com";
    private const string RegisteredPassword = "Secret123";

    private readonly AuthenticationRepositoryInMemory _repository = new();
    private readonly FakePasswordHasher _hasher = new();

    private readonly AuthenticationService _service;

    public AuthenticationServiceTest()
    {
        _service = new AuthenticationService(_repository, _hasher);
    }

    private void RegisterAccount()
    {
        Result<Account> result = Account.Register("vitor", RegisteredEmail, RegisteredPassword, _hasher);

        Assert.True(result.IsSuccess, "Test setup: the account data must be valid");
        _repository.Register(result.Value);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsSuccess()
    {
        RegisterAccount();

        Result loggedInResult = await _service.LoginAsync(RegisteredEmail, RegisteredPassword);

        Assert.True(loggedInResult.IsSuccess);
    }

    [Theory]
    [InlineData("vit@example.com", RegisteredPassword)]
    [InlineData(RegisteredEmail, "Secret1234")]
    public async Task Login_InvalidCredentials_ReturnsFailure(
            string attemptedEmail, string attemptedPassword)
    {
        RegisterAccount();

        Result loggedInResult = await _service.LoginAsync(attemptedEmail, attemptedPassword);

        Assert.True(loggedInResult.IsFailure);
        Assert.Equal(AuthenticationErrors.Account.InvalidCredentials, loggedInResult.Error);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("vi@gmail")]
    [InlineData("vi.com")]
    public async Task Login_InvalidEmail_ReturnsFailure(string email)
    {
        RegisterAccount();

        Result loggedInResult = await _service.LoginAsync(email, RegisteredPassword);

        Assert.True(loggedInResult.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.InvalidFormat, loggedInResult.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public async Task Login_EmptyEmail_ReturnsFailure(string email)
    {
        RegisterAccount();

        Result loggedInResult = await _service.LoginAsync(email, RegisteredPassword);

        Assert.True(loggedInResult.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.Empty, loggedInResult.Error);
    }

    [Fact]
    public async Task Login_ValidCredentialsWithNoAccountCreated_ReturnsFailure()
    {
        Result loggedInResult = await _service.LoginAsync(RegisteredEmail, RegisteredPassword);

        Assert.True(loggedInResult.IsFailure);
        Assert.Equal(AuthenticationErrors.Account.InvalidCredentials, loggedInResult.Error);
    }

    [Theory]
    [InlineData("VITOR@EXAMPLE.COM")]
    [InlineData("vitor@example.COM")]
    public async Task Login_UppercaseValidEmail_ReturnsSuccess(string email)
    {
        RegisterAccount();

        Result loggedInResult = await _service.LoginAsync(email, RegisteredPassword);

        Assert.True(loggedInResult.IsSuccess);
    }

    [Theory]
    [InlineData("      vitor@example.com      ")]
    [InlineData("vitor@example.com      ")]
    [InlineData("    vitor@example.com")]
    public async Task Login_EmailWithSurroundingWhitespace_ReturnsSuccess(string email)
    {
        RegisterAccount();

        Result loggedInResult = await _service.LoginAsync(email, RegisteredPassword);

        Assert.True(loggedInResult.IsSuccess);
    }

    [Fact]
    public async Task Login_TooLongEmail_ReturnsFailure()
    {
        const string domain = "@example.com";
        string tooLongEmail = new string('a', Email.MaxLength + 1 - domain.Length) + domain;

        Result loggedInResult = await _service.LoginAsync(tooLongEmail, RegisteredPassword);

        Assert.True(loggedInResult.IsFailure);
        Assert.Equal(AuthenticationErrors.Email.TooLong, loggedInResult.Error);
    }
}


