namespace ActionIn.Authentication.Tests.DomainObjects;

using Domain.ValueObjects;
using Domain.Hasher;
using Xunit;

public class FakePasswordHahser : IPasswordHasher
{
    public string Hash(string password) => $"hashed:{password}";
    public bool Verify(string password, string hashedPassword) => hashedPassword == $"hashed:{password}";
}

public class PasswordTests
{
    private readonly IPasswordHasher _hasher = new FakePasswordHahser();

    [Fact]
    public void Create_ValidPassword_StoresHashedValue()
    {
        var password = Password.Create("senha123", _hasher);

        Assert.Equal("hashed:senha123", password.Value);
    }

    [Fact]
    public void Create_PasswordShorterThanFourCharacters_ThrowsException()
    {
        Assert.Throws<Exception>(() => Password.Create("abc", _hasher));
    }

    [Fact]
    public void Verify_CorrectRawPassword_ReturnsTrue()
    {
        var password = Password.Create("senha123", _hasher);

        Assert.True(password.Verify("senha123", _hasher));
    }

    [Fact]
    public void Verify_WrongRawPassword_ReturnsFalse()
    {
        var password = Password.Create("senha123", _hasher);

        Assert.False(password.Verify("outrasenha", _hasher));
    }
}
