# 9.  Define the responsibilities of each layer inside a module

Date: 2026-11-23

## Status

Accepted

## Context

ADR 0004 divides the system into modules, and each module is split into projects (Domain, Application, Data, AntiCorruption), but no document says what belongs to each one. Without a rule, the placement of a class depends on who writes it.

The result is already visible. `NewAccountRegisteredEventHandler` lives in the Domain project, sends an e-mail through `IEmailSender` and writes errors with `Console.WriteLine`. Sending a welcome e-mail is not a business rule of an account; it is a reaction of the application to a domain event. Because the handler is in the Domain, `IEmailSender` and `EmailMessage` are in the Domain too.

ADR 0008 also introduces code that is shared by all modules and depends on EF Core and `IMediator` (the `SaveChangesInterceptor`). It does not belong to any module, and putting it in `ActionIn.Core` would make every Domain project depend on EF Core.

## Decision

Each layer has one responsibility and a fixed set of allowed dependencies:

| Layer | Responsibility | May depend on |
|---|---|---|
| `ActionIn.Core` | Shared kernel: `Entity`, `AggregateRoot`, `DomainEvent`, `Result`, messages, repository and unit of work interfaces | Nothing from the solution |
| `ActionIn.Core.Infrastructure` | Shared technical code that depends on EF Core, Npgsql, FluentValidation or `IMediator` (interceptors, pipeline behaviors, later the outbox) | `ActionIn.Core` |
| Domain | Business rules: aggregates, value objects, domain services, domain events and the ports the domain needs (repository interfaces, `IPasswordHasher`) | `ActionIn.Core` |
| Application | Use cases: command and query handlers, domain event handlers, DTOs, and output ports for side effects (`IEmailSender`, `EmailMessage`) | Domain, `ActionIn.Core` |
| Data | Persistence: `DbContext`, mappings, migrations, repository implementations | Domain, `ActionIn.Core`, `ActionIn.Core.Infrastructure` |
| AntiCorruption | Adapters to what is external to the module (SMTP, other modules, third-party APIs) | Application, Domain, `ActionIn.Core` |
| WebApp | Controllers and the composition root | Everything |

As a consequence of this decision:

- `NewAccountRegisteredEventHandler` moves to the Application project and logs with `ILogger`.
- `IEmailSender` and `EmailMessage` move to the Application project with it.
- Domain projects never reference `ActionIn.Core.Infrastructure`.

## Consequences

Positive:

- The Domain only contains business rules, and its dependencies are the shared kernel and nothing else.
- There is one answer to "where does this class go?", which keeps modules consistent with each other.
- Shared technical code lives in one place, outside the Domain.
- The rules can be enforced by architecture tests instead of reviews.

Negative:

- **The kernel still references MediatR.** `Event` implements `INotification` so that MediatR can publish domain events, which means `ActionIn.Core`, and so every Domain project, depends on the MediatR package. Removing it would need a wrapper type in the infrastructure, adding a mapping step for every event. That cost is accepted for now. To keep the dependency limited to the marker interface, an architecture test forbids Domain projects from using `IMediator`, `IPublisher` or `ISender`.
- More projects and more references to maintain for each new module.
- Classes that are already in the wrong place must be moved, which changes namespaces and breaks the DI registrations that point to them.
