using ActionIn.Core.DomainObjects;
using ActionIn.Core.Infrastructure.Mediatr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ActionIn.Core.Infrastructure.Interceptor;

public class PublishDomainEventInterceptor(IMediatrHandler mediatr) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken)
    {
        await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null)
            return;

        var aggregates = context.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(a => a.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            await mediatr.PublishEvent(domainEvent, cancellationToken);
        }
    }

}


