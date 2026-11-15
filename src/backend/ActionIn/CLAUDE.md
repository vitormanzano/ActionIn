# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.
Never gives me the answer until i ask

## Repository layout

Monorepo (git root is `/home/vimanzanovd/dev/ActionIn`, three levels above this directory):

- `src/backend/ActionIn/` — .NET 10 backend (solution `ActionIn.slnx`). This is the working directory.
- `src/frontend/ActionIn/` — Angular 22 frontend (`npm start`, `npm run build`, `npm test`), only auth pages so far.
- `docker-compose.yml` (repo root) — only Postgres (`postgres`/`postgres`, db `ActionIn`, port 5432).
- `docs/` — ADRs, functional requirements (FR01–FR11), git conventions, `httpRequests.txt` (xh examples).

ActionIn is a social network where users start an "action" and friends see what they're doing and for how long. Only the Authentication module (FR01 register account) is implemented.

## Commands (run from `src/backend/ActionIn`)

```bash
docker compose -f ../../../docker-compose.yml up -d   # Postgres
dotnet build ActionIn.slnx
dotnet run --project src/WebApp/ActionIn.WebApp       # http://localhost:5199, https://localhost:7271
dotnet test ActionIn.slnx
dotnet test tests/ActionIn.Authentication.Tests --filter "FullyQualifiedName~AccountTests"   # single class/test
```

Requests can be tried with `src/WebApp/ActionIn.WebApp/ActionIn.WebApp.http`. EF migrations live in `ActionIn.Authentication.Data`:
`dotnet ef migrations add <Name> -p src/Services/Authentication/ActionIn.Authentication.Data -s src/WebApp/ActionIn.WebApp`.

Configuration: `ConnectionStrings:DefaultConnection` is empty in `appsettings.json` and `Smtp:Password` is not set — supply both via user-secrets/env vars (`ConnectionStrings__DefaultConnection`, `Smtp__Password`). Without SMTP username+password the sender skips authentication.

NuGet versions are managed centrally in `Directory.Packages.props` (`ManagePackageVersionsCentrally`), so `PackageReference`s in csproj files carry no version.

## Architecture

Modular monolith (ADR 0002/0004) using DDD + CQRS-style commands over MediatR. Per-module projects under `src/Services/<Module>/`, plus a shared kernel and a single REST host (ADR 0005):

- `ActionIn.Core` — shared kernel: `Entity`, `IAggregateRoot`, `IRepository`/`IUnitOfWork`, `Message`/`Command`/`Event`, `ICommandHandler<T>`, `IQuery*`, and `IMediatrHandler` (thin wrapper over `IMediator` exposing `SendCommand` / `PublishEvent`).
- `ActionIn.Authentication.Domain` — aggregate `Account`, value objects (`Email`, `Password`, `Username`), `AuthenticationService`, `IPasswordHasher`, repository and `IEmailSender` interfaces, and domain events + handlers (`NewAccountRegistered` sends the welcome email).
- `ActionIn.Authentication.Application` — commands/handlers (`RegisterAccountCommandHandler`), DTOs, mappers.
- `ActionIn.Authentication.Data` — EF Core/Npgsql `AuthenticationContext`, mappings, migrations, repository implementation.
- `ActionIn.Authentication.AntiCorruption` — MailKit `EmailSender` behind `IEmailSender` (keeps SMTP out of the domain).
- `ActionIn.WebApp` — controllers + composition root.

Request flow for register: `AuthenticationController` → `IMediatrHandler.SendCommand(RegisterAccountCommand)` → `RegisterAccountCommandHandler` → `AuthenticationService.RegisterAsync` (validation/hashing) → repository + `UnitOfWork.Commit()` → `PublishEvent(NewAccountRegisteredEvent)` → event handler sends email (the handler has a TODO to raise the event from the entity instead).

### Gotchas

- **DI registration is manual** in `WebApp/Extensions/ServiceExtensions.cs`. Each new command handler must be added to `AddCustomCommands()` (`ICommandHandler<T>` → implementation) in addition to MediatR assembly scanning in `Mediator()`; new services/repositories go in `AddCustomServices()`. A new module's assemblies must also be added to the MediatR scan.
- Layer rules are enforced by NetArchTest in `tests/ActionIn.Authentication.Tests/ArchTests/Modules/LayerTests.cs`: Domain must not reference Application/Data; Application must not reference Data; Data must reference Domain. Keep new code consistent with this.
- Tests use xUnit; unit tests are under `UnitTests/` (value objects, `Account`, hasher), `ActionIn.Core.Tests` covers the shared kernel.

## Conventions (from `docs/formatting`)

- Branches: `feature/<ticketId>`, `bugfix/<ticketId>`; merged to `main` with `--no-ff`.
- Commit messages: `prefix: subject line`, then a blank line, the motivation, and what the commit does (prefixes in history: `feat:`, `docs:`).
- Per project memory: when asked to "escrever commits", only propose messages — never run `git commit`.
