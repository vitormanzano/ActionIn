namespace ActionIn.Authentication.Tests;

using Domain;
using Domain.Hasher;
using Domain.ValueObjects;
public class FakePasswordHahser : IPasswordHasher
{
    public string Hash(string password) => $"hashed:{password}";
    public bool Verify(string password, string hashedPassword) => hashedPassword == $"hashed:{password}";
}


public class AccountTests
{
    private readonly IPasswordHasher _hasher = new FakePasswordHahser();

    [Fact]
    public void Register_ValidData_CreatesAccount()
    {
        var password = Password.Create("senha123", _hasher);
        var account = Account.Register("vitor", "vitor@gmail.com", password);

        Assert.Equal("vitor", account.Username.Value);
        Assert.Equal("vitor@gmail.com", account.Email.Value);
    }

    [Fact]
    public void Register_InvalidUsername_ThrowsException()
    {
        var password = Password.Create("senha123", _hasher);
        Assert.Throws<Exception>(() => Account.Register("abc", "vitor@gmail.com", password));
    }
}
