# ActionIn Bounded Contexts

This document details the **Bounded Contexts** of the **ActionIn** platform, the strategic and technical justification for the existence of each boundary, their responsibilities, and their classification according to Domain-Driven Design (DDD).

---

## 1. Overview of Subdomains

| Bounded Context | Classification | Central Responsibility | Autonomy and Boundary |
| :--- | :--- | :--- | :--- |
| **Authentication** | Generic Subdomain | Management of credentials, identity, authentication, and access security. | Issues the official user identity (`UserId`) and protects sensitive data (password hashes). |
| **Profile** | Supporting Subdomain | Public/presentation view of the user, biographies, activity summary, and aggregated metrics. | Queries and projects the user's history; does not interfere with the real-time execution of actions. |
| **Action** | **Core Domain** | Registration and monitoring of the action lifecycle in real time (Start/Finish/Duration). | Represents the product's competitive advantage ("what you are doing right now"). High rate of real-time writing and reading. |
| **Friends** | **Core Domain** | Social graph, lifecycle management of requests and active friendships. | Isolates relationship rules (request, accept, reject, unfriend) and visibility among peers. |
| **Kernel (`ActionIn.Core`)**| Shared Kernel | Fundamental domain abstractions and shared primitive types. | Reused by all contexts; no specific subdomain business rules. |

---

## 2. Detailed Justification of Each Bounded Context

### 2.1. Authentication Context (Generic Subdomain)
* **Why is it a separate Bounded Context?**
  * **Security and Isolation:** Credentials, password hashing algorithms (`IPasswordHasher`), and authentication flows (tokens, sessions) are critical and sensitive topics. No other part of the system needs or should have access to passwords or encryption methods.
  * **Generic Subdomain:** Authentication is a common need for almost any commercial software, not constituting ActionIn's business differentiator.
  * **Avoiding the "God Class" User:** In the authentication context, the central concept is the **Account**. There is no need for the account to know about actions, friends, or activity history.
* **Invariants and Rules:**
  * The email and username must be unique and valid upon registration.
  * The password must meet predefined strength criteria and be persisted exclusively as a hash.

### 2.2. Profile Context (Supporting Subdomain)
* **Why is it a separate Bounded Context?**
  * **Separation between Identity and Presentation:** While `Authentication` deals with who the user is to the security system, `Profile` deals with how the user is perceived by the social community (photo, biography, usage statistics).
  * **Supporting Subdomain:** Supports user engagement and the social experience, acting as a showcase for friends to view their metrics and achievements.
  * **Read Optimization (CQRS / Projections):** Meets requirements like FR07 (*See my actions*) and FR08 (*See how much time I have spent on my actions*), maintaining aggregated projections of time and history without burdening the real-time transactional model of the `Action Context`.

### 2.3. Action Context (Core Domain)
* **Why is it a separate Bounded Context?**
  * **Heart of the Business (Core Domain):** ActionIn's central premise is real-time action sharing ("Start an action, and your friends can see what you're doing and how long you've been at it").
  * **Strict Lifecycle Invariants:**
    * An action starts with a mandatory title and a precise timestamp (`StartedAt`).
    * An action can only be finished if it is active.
    * The finish date (`FinishedAt`) cannot precede the start date.
    * The duration calculation is derived from the action's timeframe.
  * **Operational Independence:** Needs to be able to scale fast write operations (real-time start and finish) and trigger immediate events without suffering bottlenecks or blocks from the social graph or authentication.

### 2.4. Friends Context (Core Domain)
* **Why is it a separate Bounded Context?**
  * **Social Graph Management:** The central entity is not the user, but rather the **Relationship** (`Friendship`/`FriendRequest`).
  * **Specific Lifecycle:** Complex state transitions (`Pending` -> `Accepted`, `Rejected`, `Blocked`, `Ended`) with validations such as: a user cannot send a friend request to themselves, and duplicate requests are prohibited.
  * **Transactional Boundary:** Separating friendships from `Profile` and `Action` prevents changes to the friends list from locking the user's profile or active actions.

### 2.5. Kernel (`ActionIn.Core` - Shared Kernel)
* **Why does it exist?**
  * Eliminates repetitive boilerplate code in a modular .NET monolith.
  * Provides fundamental tactical building blocks: `Entity`, `IAggregateRoot`, `DomainEvent`, and primitive identification types (`UserId`).
  * Any change to the Shared Kernel requires agreement and alignment from all modules.

