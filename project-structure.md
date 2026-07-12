# Project Structure

```
src
├── HamedSoft.Template.SharedKernel
├── HamedSoft.Template.Domain
├── HamedSoft.Template.Application
├── HamedSoft.Template.Infrastructure
└── HamedSoft.Template.Web
```

## Folder Convention

SharedKernel
- Common
- Entities
- Events
- ValueObjects

Application
- Abstractions
- Features

Infrastructure
- Persistence
- Identity
- Common
- Repositories

Web
- Controllers
- Views
- ViewModels
- wwwroot

Each feature should keep related classes together whenever possible.