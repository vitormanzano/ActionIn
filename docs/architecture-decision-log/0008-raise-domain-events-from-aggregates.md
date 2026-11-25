# 8.  Raise domain events from aggregates and dispatch them on commit

Date: 2026-11-23

## Status

Accepted

## Context

`RegisterAccountCommandHandler` builds `NewAccountRegisteredEvent` and publishes it through `IMediatrHandler` after the commit. A comment in the handler says the entity will raise the event later.

When the use case owns the event, the fact "an account was registered" depends on every caller remembering to publish it. A second use case that creates an account (an import, a seed, an admin endpoint) can create the account and forget the event, and nothing fails: the welcome e-mail is just never sent.

The event describes something that happened to the aggregate. The aggregate root is the only object that guarantees the invariants of its group of entities, so it is the only one that can state, with certainty, that the change happened. It is also the unit that the repository loads and saves, and the unit of a transaction.

The moment of publication also matters. If the event is published before the data is saved and the save fails, the handlers act on something that does not exist: the user receives a welcome e-mail for an account that was never created.

## Alternatives Considered

- **Keep publishing from the command handler.** Simple and explicit, but the event depends on the discipline of each use case, and the domain does not announce its own changes.
- **Raise the event from the aggregate and publish it immediately, before the commit** (for example, inside `Account.Register` or the repository). Handlers would run for changes that may still be rolled back, and the domain would need to know about MediatR.
- **Transactional Outbox now.** Save the events in an `outbox_messages` table in the same transaction and publish them from a background processor. It removes the loss of events, but needs a table per schema, a processor, retries and idempotent handlers. That is too much for one event with one handler, and it is planned for phase 3 (ticket 038).

## Decision

Aggregates raise domain events and dispatch them only after the commit:

- An `AggregateRoot` base class in `ActionIn.Core` (inheriting from `Entity`) keeps a private list of `DomainEvent`s. Only the aggregate itself can add an event (protected method). The list is exposed as a read-only collection and has a public method to clear it.
- `Account.Register` raises `NewAccountRegisteredEvent`. Command handlers no longer publish domain events.
- A `SaveChangesInterceptor` in `ActionIn.Core.Infrastructure` reads the events from the tracked `AggregateRoot`s, clears them, and publishes them through MediatR after `SaveChanges` succeeds.
- The event list is not persisted: EF Core ignores it in the mapping.

## Consequences

Positive:

- Any use case that registers an account raises the event; it cannot be forgotten.
- The domain describes what happened to it, without depending on MediatR or on the database.
- Handlers run only for changes that were saved.
- `Account.Register` can be tested in isolation by checking its raised events.
- Command handlers become smaller: they orchestrate and commit, nothing else.

Negative:

- **Events can be lost.** If the process crashes after the commit and before (or during) the publication, the data is saved but the events are never handled. No retry exists. The Transactional Outbox removes this limitation.
- Handlers run outside the transaction. If a handler fails (for example, the SMTP server is down), the account stays created and the failure must be handled and logged by the handler itself; it does not roll back the use case.
- Publication is implicit: reading the command handler no longer shows which events are published. The flow must be found through the aggregate and the interceptor.
- Events are only published by code that saves through an EF Core context with the interceptor registered. Any other persistence path would skip them.
