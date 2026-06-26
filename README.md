# LinkDev.IKEA

A small layered ASP.NET Core sample solution that demonstrates a practical application using modern .NET technologies and common architectural patterns.

## Purpose
Provide a simple reference implementation showing how to organize a web application with separate Data (DAL), Business (BLL) and Presentation (PL) layers, including authentication, data access and basic CRUD operations.

## Technologies
- .NET 9 (C# 13)
- ASP.NET Core (Razor Pages / MVC controllers and views)
- Entity Framework Core (EF Core) for data access
- Apply Pagenation and filtering for listing entities
- ASP.NET Core Identity for authentication and authorization
- Repository + Unit of Work patterns with lazy loading and eager loading support and generic repository base class
- Dependency Injection (built-in DI container)
- AutoMapper for object mapping (mapping profiles present)
- SQL Server (LocalDB or other provider configured via connection string)
- DateOnly usage for date fields

## Project structure
- `LinkDev.IKEA.DAL` — EF Core entities, DbContext, repositories, migrations and data seeding
- `LinkDev.IKEA.BLL` — business services, DTOs and mapping profiles
- `LinkDev.IKEA.PL`  — web application (controllers, views, startup configuration)

## Key features
- Layered architecture separating persistence, business logic and presentation
- Repository + Unit of Work abstractions to simplify data operations and testing
- Built-in seeding for sample departments, employees and an initial admin Identity user
- Simple CRUD operations for departments and employees

## Quick start
1. Restore and build
   - `dotnet restore`
   - `dotnet build`

2. Apply EF migrations (from repository root)
   - Add a migration (if schema changes are made):
     `dotnet ef migrations add Initial --project LinkDev.IKEA.DAL --startup-project LinkDev.IKEA.PL`
   - Update the database:
     `dotnet ef database update --project LinkDev.IKEA.DAL --startup-project LinkDev.IKEA.PL`

3. Run the web app
   - `dotnet run --project LinkDev.IKEA.PL`
   - Open the URL shown in the console (e.g. `https://localhost:5001`).

## Database seeding and default admin user
A `DbInitializer` seeds sample data when the database is empty. The default seeded Identity admin user can be found in `LinkDev.IKEA.DAL/Persistence/Data/DbInitializer/DbInitializer.cs`.
- Default admin credentials :
  - Email: `admin@gmail.com`
  - Password: `P@ssw0rd`

To customize or disable seeding, edit or remove the seeding logic in the `DbInitializer` class or adjust how it is invoked in `LinkDev.IKEA.PL/Program.cs`.

## Important files and entry points
- `LinkDev.IKEA.PL/Program.cs` — application startup and service registration
- `LinkDev.IKEA.DAL/Persistence/Data/DbInitializer/DbInitializer.cs` — seed logic and identity setup
- `LinkDev.IKEA.DAL/Persistence/Data/ApplicationDbContext.cs` — EF Core DbContext and configurations
- `LinkDev.IKEA.DAL/Persistence/UnitOfWork/UnitOfWork.cs` — unit of work implementation
- `LinkDev.IKEA.BLL/Services` — business service implementations
- `LinkDev.IKEA.PL/Controllers` and `LinkDev.IKEA.PL/Views` — presentation layer


## Contributing
Contributions are welcome. Please open issues or submit PRs with improvements.


