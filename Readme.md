--Migration

IF Directory and Migration Files Not Exists!!! in :
./src/HamedSoft.Template.Infrastructure/Persistence/Migration

		Execute Command (in Terminal):
		dotnet ef migrations add InitialCreate --project .\src\HamedSoft.Template.Infrastructure --output-dir .\Persistence\Migrations

else (IF Directory and Migration Files Exists in :)
		Execute Command (in Terminal):
		dotnet ef migrations add InitialCreate --project .\src\HamedSoft.Template.Infrastructure


--Database Update

dotnet ef database update --project .\src\HamedSoft.Template.Infrastructure