# 10.  Keep MediatR under the Community license

Date: 2026-11-28

## Status

Accepted

## Context

MediatR dispatches every command, query and domain event in the system. `ICommandHandler` and `IQueryHandler` in `ActionIn.Core` extend MediatR's `IRequestHandler`, `Event` implements `INotification` (ADR 0009), and the validation and logging pipeline behaviors in `ActionIn.Core.Infrastructure` are MediatR behaviors.

Since version 13.0.0, MediatR is distributed under a commercial license by Lucky Penny Software. Earlier versions stay under their original open-source license (Apache 2.0) but receive no more updates or security fixes. The project already uses MediatR 14.2.0.

The commercial license has a free Community edition. According to the Lucky Penny licensing FAQ, it is available to individuals and organizations that meet all of these criteria:

- Annual gross revenue (or non-profit budget) below 5,000,000 USD.
- No more than 10,000,000 USD received in outside capital.
- Not a government or quasi-government entity.
- Not a university or higher-education institution using the library for institutional or operational software.

The license key is self-service, valid for 12 months, and must be renewed by confirming eligibility again every year. Only production deployments need the key; development, CI, staging and integration tests do not. A missing, expired or invalid key produces log messages only: the library keeps working, with no runtime limits.

ActionIn is a personal portfolio project with no revenue and no outside capital, so it qualifies for the Community edition. Without a decision, the key would either be committed to `appsettings.json` or not configured at all, and the dependency on a commercial license would go unrecorded.

## Alternatives Considered

- **Write an in-house dispatcher.** ADR 0007 chose to write the Result Pattern instead of using a library, because the surface was small and owning it was the point. A dispatcher is different: it needs handler discovery through DI scanning, open generic pipeline behaviors, request/response and notification dispatch, and handler resolution per scope. That is infrastructure, not the domain this project is meant to study, and it would replace a library that already works. Rejected for now.
- **Stay on MediatR 12.x (Apache 2.0).** Free and with no license key, but frozen: no bug or security fixes, and it would force a downgrade of the version already in use. Rejected.
- **Use MediatR under the Reciprocal Public License 1.5.** Free, but it is a copyleft license with obligations that go beyond publishing this repository, and those obligations would apply to everything built on it. Rejected.
- **Switch to another library**, such as the source-generated `Mediator` (MIT) or Wolverine. Both are reasonable choices, but switching means rewriting the handler interfaces in `ActionIn.Core`, the pipeline behaviors and the domain event interceptor, with no functional gain today. Kept as the first option if this decision is reevaluated.

## Decision

Keep MediatR 14 under the Community license.

- The key is read from configuration under `MediatR:LicenseKey` and set on `MediatRServiceConfiguration.LicenseKey` in the single shared `AddMediatR` call.
- Locally, the key lives in user secrets. In deployed environments, it comes from the environment variable `MediatR__LicenseKey` or a secret store.
- `appsettings.json` only declares the setting with an empty value, so the key never reaches the repository.
- The application does not fail to start when the key is missing. Tests and CI run without it, as the license allows.

## Consequences

Positive:

- No migration: handlers, behaviors and the domain event interceptor stay as they are.
- The key stays out of the repository and can change per environment without changing code.
- The dependency on a commercial license is recorded, together with the conditions that make it free.

Negative:

- **The project depends on terms set by a third party.** They can change, and the Community key must be renewed every 12 months. An expired key does not break the application, but production logs will report it.
- Every Domain project still references the MediatR package through `ActionIn.Core` (ADR 0009), which makes a future replacement more expensive.

Reevaluate this decision when any of these happens:

- The project stops meeting the Community criteria (revenue, outside capital, government or institutional use).
- Lucky Penny changes the Community terms or ends the edition.
- The yearly key renewal becomes a burden.
- A non-functional requirement, such as the performance work planned in phase 6, shows that the reflection-based dispatch is a bottleneck. In that case, the source-generated `Mediator` library is the first alternative to evaluate.

## References

- https://luckypennysoftware.com/faq
- https://luckypennysoftware.com/license
- https://docs.mediatr.io/en/latest/License-Key-Configuration.html
