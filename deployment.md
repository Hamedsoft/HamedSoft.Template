# Deployment

## Requirements

- .NET 8 SDK
- SQL Server

## Build

```
dotnet restore
dotnet build
```

## Database

Run EF Core migrations before starting the application.

See:
docs/ef-core.md

## Publish

```
dotnet publish -c Release
```