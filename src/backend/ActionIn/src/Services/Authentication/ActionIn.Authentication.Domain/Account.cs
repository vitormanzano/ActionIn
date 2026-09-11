namespace ActionIn.Authentication.Domain;

using ActionIn.Core.DomainObjects;
using ActionIn.Authentication.Domain.ValueObjects;
using ActionIn.Authentication.Domain.Hasher;

public class Account : Entity, IAggregateRoot
{
    public Username Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;

    private Account() { }

    public static Account Register(string username, string email, string rawPassword, IPasswordHasher hasher)
    {
        Password.Validate(rawPassword);

        return new Account
        {
            Username = new Username(username),
            Email = new Email(email),
            Password = Password.FromHash(hasher.Hash(rawPassword))
        };
    }

    public bool VerifyPassword(string rawPassword, IPasswordHasher hasher) => hasher.Verify(rawPassword, Password.Value);

}
