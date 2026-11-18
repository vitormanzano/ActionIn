# 7.  Use Result Pattern

Date: 2026-11-17

## Status

Accepted

## Context

The domain throws a generic `Exception` for expected situations, such as a username shorter than 4 characters. That is not an exceptional case: it is a known business rule that will happen in normal use.

Because business rules and bugs travel through the same mechanism, the caller cannot tell them apart. A rule violation and a `NullReferenceException` look the same to the controller, so the API answers with a 500 or an empty 400 and the client never learns what went wrong. Tests reflect the same problem: they assert `Assert.Throws<Exception>`, which passes for any failure, not for the rule being tested.

A method signature like `Task<Account> RegisterAsync(...)` also hides the fact that it can fail. The only way to know is to read the implementation.

Throwing exceptions also has a runtime cost.

 ## Alternatives Considered

- **Keep exceptions, with custom types** (`DomainException`, `NotFoundException`) and a global exception handler. Less code to change, but the signature still hides the failure,
  control flow jumps across layers and expected cases still pay the cost of an exception.
- **Domain notifications through MediatR** (a scoped `DomainNotificationHandler` collecting messages that the controller checks at the end). It accumulates errors well, but it
is a side channel: nothing in the signature says the method can fail, the caller must remember to check, and the code keeps running after a failure.
- **ErrorOr** and **FluentResults**. Both are good, mature libraries and would be a reasonable choice in a production team. They were not chosen because the goal of this project
is to understand the pattern by writing it, and the needed surface is small enough to own without a dependency.

## Decision

Implement the Result Pattern

## Consequences
Positive:

- Failures are explicit in method signatures; the compiler forces callers to deal with them.
- Business rules and bugs are separated: a `Result` failure is expected, an exception is a bug.
- Tests can assert on the error `Code` instead of `Assert.Throws<Exception>`.
- The API can map `ErrorType` to the correct HTTP status and return a useful body.

Negative:

- More verbose code: every call that can fail must check the result and propagate it.
- A caller can still ignore a returned `Result`; discipline and review are needed.
- Many signatures change (value objects, `Account`, `AuthenticationService`, command and query handlers). The migration is spread across tickets 016 to 018, and until it ends both styles coexist in the codebase.
