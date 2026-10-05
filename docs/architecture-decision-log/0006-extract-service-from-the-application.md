# 1.  Extract Services From the Application

Date: 2026-10-05

## Status

Accepted

## Context

At some time feels strange the `AuthenticationService` be in the Application, because this class is doing so many things that are business rules, that are from the domain. And doesn't seem to be just a class that orchestrate the flow of data between the outside world and the core business logic.

## Decision

Implement the `AuthenticationService` in the domain layer. 


## Consequences

The `Domain` layer holds the business rules inside.


