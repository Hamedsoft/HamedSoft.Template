# Entity Framework Core
## Add First Migration

	Execute:
		dotnet ef migrations add <MigrationName> --project .\src\HamedSoft.Template.Infrastructure --output-dir .\Persistence\Migrations

	> Example:
		dotnet ef migrations add InitialCreate --project .\src\HamedSoft.Template.Infrastructure --output-dir .\Persistence\Migrations

## Add Next Migration

	Execute:
		dotnet ef migrations add <MigrationName> --project .\src\HamedSoft.Template.Infrastructure


## Update Database

	Execute:
		dotnet ef database update --project .\src\HamedSoft.Template.Infrastructure


## Remove Last Migration

	Execute:
		dotnet ef migrations remove --project .\src\HamedSoft.Template.Infrastructure

## List Migrations

	Execute:
		dotnet ef migrations list --project .\src\HamedSoft.Template.Infrastructure

## Generate SQL Script

	Execute:
		dotnet ef migrations script --project .\src\HamedSoft.Template.Infrastructure

