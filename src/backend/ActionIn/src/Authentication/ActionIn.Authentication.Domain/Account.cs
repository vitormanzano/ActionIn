namespace ActionIn.Authentication.Domain;

using ActionIn.Core.DomainObjects;
using ActionIn.Authentication.Domain.ValueObjects;

public class Account : Entity, IAggregateRoot
{
    public Username Username { get; protected set; } = null!;
    public Email Email { get; protected set; } = null!;
    public Password Password { get; protected set; } = null!;

    private Account() { }

    public static Account Register(string username, string email, Password password)
    {
        return new Account
        {
            Username = new Username(username),
            Email = new Email(email),
            Password = password
        };
    }

}
