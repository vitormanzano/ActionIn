using ActionIn.Authentication.Domain.Hasher;

namespace ActionIn.Authentication.Tests.UnitTests.Infrastructure;

public sealed class FakePasswordHasher : IPasswordHasher
{
    private const string HashPrefix = "hashed:";

    public int HashCalls { get; private set; }

    public string Hash(string password)
    {
        HashCalls++;
        return HashPrefix + password;
    }

    public bool Verify(string password, string hashedPassword) => hashedPassword == HashPrefix + password;
}


