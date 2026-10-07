using ActionIn.Authentication.Domain;
using ActionIn.Authentication.Domain.ValueObjects;

namespace ActionIn.Authentication.Tests.UnitTests;

public class AccountTests
{

    [Fact]
    public void Register_ValidData_CreatesAccount()
    {
        var password = Password.FromHash("senha123");

        var account = Account.Register("vitor", "vitor@gmail.com", password);

        Assert.Equal("vitor", account.Username.Value);
        Assert.Equal("vitor@gmail.com", account.Email.Value);
        Assert.Equal("senha123", account.Password.Value);
    }

    [Fact]
    public void Register_InvalidUsername_ThrowsException()
    {
        var password = Password.FromHash("senha123");

        Assert.Throws<Exception>(() => Account.Register("abc", "vitor@gmail.com", password));
    }

}
