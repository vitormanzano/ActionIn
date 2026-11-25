using ActionIn.Core.Messages;

namespace ActionIn.Core.DomainObjects;

public class DomainEvent : Event
{
    public DomainEvent(Guid aggregateId)
    {
       AggregateId = aggregateId; 
    }
}