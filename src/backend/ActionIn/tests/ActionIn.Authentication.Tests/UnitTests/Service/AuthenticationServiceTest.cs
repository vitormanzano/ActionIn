using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.Hasher;
using ActionIn.Authentication.Domain.Services;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Authentication.Tests.UnitTests.Infrastructure;

namespace ActionIn.Authentication.Tests.UnitTests.Service;

public class AuthenticationServiceTest
{
    private readonly AuthenticationRepositoryInMemory _repository = new();
    private readonly PasswordHasher _passwordHasher = new();

    private readonly AuthenticationService _service;

    public AuthenticationServiceTest()
    {
        _service = new AuthenticationService(_repository, _passwordHasher);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsTrue()
    {
        var email = "vitor@example.com";
        var password = "Secret123";
        var hashedPassword = _passwordHasher.Hash(password);


        var account = Account.Register("vitor", email, Password.FromHash(hashedPassword));
        _repository.Register(account);

        var exists = await _service.LoginAsync(email, password);
        Assert.True(exists);
    }
}


