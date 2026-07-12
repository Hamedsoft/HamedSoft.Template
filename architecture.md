# Architecture

This solution follows the principles of Clean Architecture.

## Projects

- HamedSoft.Template.SharedKernel
  - Base entities
  - Value objects
  - Domain events
  - Shared abstractions

- HamedSoft.Template.Domain
  - Business entities
  - Business rules
  - Domain logic

- HamedSoft.Template.Application
  - Use Cases
  - CQRS
  - Validation
  - Contracts
  - Application services

- HamedSoft.Template.Infrastructure
  - Entity Framework Core
  - Identity
  - Persistence
  - External services

- HamedSoft.Template.Web
  - ASP.NET Core MVC
  - Controllers
  - Views
  - Authentication
  - Dependency Injection

## Dependency Rule

Dependencies always point inward.

Web
↓
Infrastructure
↓
Application
↓
Domain
↓
SharedKernel

No project may reference a project outside this dependency flow.