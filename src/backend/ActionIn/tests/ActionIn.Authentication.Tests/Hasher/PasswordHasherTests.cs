namespace ActionIn.Authentication.Tests.Hasher;

using Domain.Hasher;
using Xunit;

public class PasswordHasherTests
{
    [Fact]
    public void Hash_CreatesAValidHash_withHyphen()
    {
        var hasher = new PasswordHasher();

        var hashedPassword = hasher.Hash("vitor@123");

        Assert.Contains("-", hashedPassword);
    }

    [Fact]
    public void Hash_SamePasswords_ShouldGenerateDifferentHashes()
    {
        var hasher = new PasswordHasher();

        var hashedPassword1 = hasher.Hash("vitor@123");
        var hashedPassword2 = hasher.Hash("vitor@123");

        Assert.NotEqual(hashedPassword1, hashedPassword2);
    }

    [Fact]
    public void Hash_DifferentPasswords_ShouldGenerateDifferentHashes()
    {
        var hasher = new PasswordHasher();

        var hashedPassword1 = hasher.Hash("vitor@123");
        var hashedPassword2 = hasher.Hash("manzano@123");

        Assert.NotEqual(hashedPassword1, hashedPassword2);
    }

    [Fact]
    public void Verify_CorrectPassword_ShouldReturnTrue()
    {
        var hasher = new PasswordHasher();

        var hashedPassword1 = hasher.Hash("vitor@123");
        var correctPassword = hasher.Verify("vitor@123", hashedPassword1);

        Assert.True(correctPassword);
    }

    [Fact]
    public void Verify_WrongPassword_ShouldReturnFalse()
    {
        var hasher = new PasswordHasher();

        var hashedPassword1 = hasher.Hash("vitor@123");
        var wrongPassword = hasher.Verify("manzano@123", hashedPassword1);

        Assert.False(wrongPassword);
    }
}
