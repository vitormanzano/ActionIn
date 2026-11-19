using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Services;
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
    public async Task Login_ValidCredentials_ReturnsTrue()
    {
        RegisterAccount();

        bool loggedIn = await _service.LoginAsync(RegisteredEmail, RegisteredPassword);

        Assert.True(loggedIn);
    }

    [Theory]
    [InlineData("vit@example.com", RegisteredPassword)]
    [InlineData(RegisteredEmail, "Secret1234")]
    public async Task Login_InvalidCredentials_ThrowsException(
            string attemptedEmail, string attemptedPassword)
    {
        RegisterAccount();

        var exception = await Assert.ThrowsAsync<Exception>(
                () => _service.LoginAsync(attemptedEmail, attemptedPassword)
                );

        Assert.Equal("Wrong credentials!", exception.Message);
    }
}


