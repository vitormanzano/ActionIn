using ActionIn.Core.DomainObjects;

namespace ActionIn.Authentication.Domain.Events;

public class NewAccountRegisteredEvent : DomainEvent
{
   public string Email { get; private set; }

   public NewAccountRegisteredEvent(Guid aggregateId, string email) : base(aggregateId)
   {
      Email = email;
   }
}