# Context Map

 - Authentication (U) is Customer-Supplier to Profile (D): Authentication creates the identity; Profile depends on it.
  - Action and Friends are Partners: mutual, symmetric dependency, no upstream/downstream.
  - Authentication, Profile, Action and Friends all use the Kernel (Shared Kernel) for the shared UserId type and base classes.
