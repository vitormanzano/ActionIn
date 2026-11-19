using ActionIn.Core.DomainObjects;
using ActionIn.Authentication.Domain.ValueObjects;

namespace ActionIn.Authentication.Domain;

public class Account : Entity, IAggregateRoot
{
    public Username Username { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;

    private Account() { }

    public static Account Register(string username, string email, Password password)
    {

        return new Account
        {
            Username = new Username(username),
            Email = Email.Create(email).Value,
            Password = password
        };
    }
}
