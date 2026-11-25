using ActionIn.Core.DomainObjects;

namespace ActionIn.Core.Tests.UnitTests;

public class AggregateRootTests
{
    private class FakeAggregateRoot : AggregateRoot
    {
        public void RaiseDomainEvent(DomainEvent domainEvent)
        {
            AddDomainEvent(domainEvent);
        }
    }

    [Fact]
    public void AddDomainEvent_WithDomainEvent_ExposesEventInDomainEvents()
    {
        FakeAggregateRoot fakeAggregateRoot = new();
        DomainEvent domainEvent = new(fakeAggregateRoot.Id);

        fakeAggregateRoot.RaiseDomainEvent(domainEvent);

        DomainEvent domainEventFromList = Assert.Single(fakeAggregateRoot.DomainEvents);

        Assert.Same(domainEvent, domainEventFromList);
    }

    [Fact]
    public void ClearDomainEvents_WithMultipleEvents_EmptiesDomainEvents()
    {
        FakeAggregateRoot fakeAggregateRoot = new();

        fakeAggregateRoot.RaiseDomainEvent(new DomainEvent(fakeAggregateRoot.Id));
        fakeAggregateRoot.RaiseDomainEvent(new DomainEvent(fakeAggregateRoot.Id));

        fakeAggregateRoot.ClearDomainEvents();

        Assert.Empty(fakeAggregateRoot.DomainEvents);
    }

    [Fact]
    public void ClearDomainEvents_WithNoEvents_StaysEmpty()
    {
        FakeAggregateRoot fakeAggregateRoot = new();

        fakeAggregateRoot.ClearDomainEvents();

        Assert.Empty(fakeAggregateRoot.DomainEvents);
    }

    [Fact]
    public void Constructor_WhenCalled_HasNoDomainEvents()
    {
        FakeAggregateRoot fakeAggregateRoot = new();

        Assert.Empty(fakeAggregateRoot.DomainEvents);
    }

    [Fact]
    public void DomainEvents_CastoToCollectionAndAdd_ThrowsNotSupportedException()
    {
        FakeAggregateRoot fakeAggregateRoot = new();
        DomainEvent domainEvent = new(fakeAggregateRoot.Id);

        fakeAggregateRoot.RaiseDomainEvent(domainEvent);

        ICollection<DomainEvent> collection =
            Assert.IsAssignableFrom<ICollection<DomainEvent>>(fakeAggregateRoot.DomainEvents);

        Assert.Throws<NotSupportedException>(() => collection.Add(new DomainEvent(fakeAggregateRoot.Id)));
        Assert.Single(fakeAggregateRoot.DomainEvents);

    }

}


