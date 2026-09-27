
using ActionIn.Core.DomainObjects;

namespace ActionIn.Authentication.Domain.Events.NewAccountRegistered;

public class NewAccountRegisteredEvent : DomainEvent
{
    public string Email { get; private set; }
    public string Username { get; private set; }

    public NewAccountRegisteredEvent(Guid aggregateId, string email, string username) : base(aggregateId)
    {
        Email = email;
        Username = username;
    }
}
