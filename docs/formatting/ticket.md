# Ticket format

# Title: type(scope): [Short imperative description]
> Example: feat(core): Create generic ICommandHandler interface

## Context
[Briefly explain why this ticket is necessary. What problem does it solve, or what architectural/business value does it add? Provide enough background so any developer reading this understands the "why".]

## Acceptance Criteria
*The following items must be verified before this ticket can be closed:*
- [ ] Criterion 1 (e.g., The interface `ICommandHandler<TCommand>` is created).
- [ ] Criterion 2 (e.g., The interface defines the `Handle` method).
- [ ] Criterion 3 (e.g., The file is placed inside the `Core` project).

## Technical Details & Notes
- [Include specific implementation instructions, architectural patterns (like DDD, SOLID), or file paths.]
- [Example: Consider passing a `CancellationToken` to the `Handle` method.]
- [Example: Ensure the interface uses proper type constraints `where TCommand : ICommand`.]

## References
useful links.
