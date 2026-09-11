namespace ActionIn.Authentication.Domain.Hasher;

public interface IPasswordHasher
{
    public string Hash(string password);
    public bool Verify(string password, string hashedPassword);
}
