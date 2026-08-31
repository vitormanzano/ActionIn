# Bounded contexts

  - Authentication (Generic Subdomain) — owns credentials, mints user identity.
  - Profile (Supporting) — owns username and action history display.
  - Action (Core Domain) — start/finish an action.
  - Friends (Core Domain) — friend requests, relations, listing.
  - Kernel (Shared Kernel) — base classes/interfaces and the shared UserId type.
