namespace ActionIn.Authentication.Tests;

using Domain;
using Domain.Hasher;

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
        var account = Account.Register("vitor", "vitor@gmail.com", "senha123", _hasher);

        Assert.Equal("vitor", account.Username.Value);
        Assert.Equal("vitor@gmail.com", account.Email.Value);
        Assert.Equal("hashed:senha123", account.Password.Value);
    }

    [Fact]
    public void Register_InvalidUsername_ThrowsException()
    {
        Assert.Throws<Exception>(() => Account.Register("abc", "vitor@gmail.com", "senha123", _hasher));
    }

    [Fact]
    public void VerifyPassword_CorrectRawPassword_ReturnsTrue()
    {
        var account = Account.Register("vitor", "vitor@gmail.com", "senha123", _hasher);

        Assert.True(account.VerifyPassword("senha123", _hasher));
    }

    [Fact]
    public void VerifyPassword_WrongRawPassword_ReturnsFalse()
    {
        var account = Account.Register("vitor", "vitor@gmail.com", "senha123", _hasher);

        Assert.False(account.VerifyPassword("outrasenha", _hasher));
    }
}
