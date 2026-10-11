# MoviesAPI

REST API to manage categories and movies, with JWT authentication and change auditing.

## Technologies

- .NET 10 / ASP.NET Core
- Entity Framework Core 10 with SQL Server
- ASP.NET Core Identity + JWT Bearer
- AutoMapper 12 (see the license note below)
- Serilog (console and file logs)
- Swagger (Development only)
- Health checks at `/health`

## Solution structure

| Project | Responsibility |
|---|---|
| `ApiMovies` | Web layer: controllers, middleware, configuration and startup |
| `ApiMovies.Application` | Use cases: services, DTOs and service interfaces |
| `ApiMovies.Core` | Domain entities, `AuditableEntity` and repository interfaces |
| `ApiMovies.Infraestructure` | DbContext, repositories, Unit of Work and migrations |
| `ApiMovies.CrossCutting` | Shared types (`ApiResponse`, `PagedResult`) |
| `ApiMovies.Tests` | Unit tests (xUnit + Moq) |

## Requirements

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (local or in a container)
- For migrations: `dotnet tool install --global dotnet-ef --version 10.*`

## Configuration

Secrets are not stored in `appsettings.json`. Set them with environment variables or *User Secrets*:

```bash
cd ApiMovies/ApiMovies
dotnet user-secrets set "ApiSettings:Secreta" "<long-random-key>"
dotnet user-secrets set "ConnectionStrings:ConexionSql" "<connection-string>"
```

Environment-variable equivalents: `ApiSettings__Secreta` and `ConnectionStrings__ConexionSql`.

## Run locally

```bash
cd ApiMovies/ApiMovies
dotnet run
```

Swagger is available at `/swagger` when the environment is `Development`.

## Database migrations

```bash
cd ApiMovies
dotnet ef database update --project ApiMovies.Infraestructure --startup-project ApiMovies/ApiMovies.csproj
```

The `AuditFieldsEnglish` migration renames `FechaCreacion` to `CreatedAt` in the `Categoria` and `Pelicula` tables, and adds `CreatedBy`, `UpdatedAt` and `UpdatedBy`. It deletes no data.

## Auditing

`Category` and `Movie` inherit from `AuditableEntity`:

| Field | Description |
|---|---|
| `CreatedAt` | UTC creation date and time |
| `CreatedBy` | User (JWT `name` claim) who created the record |
| `UpdatedAt` | UTC date and time of the last change |
| `UpdatedBy` | User who made the last change |

Values are filled automatically in `ApplicationDbContext.SaveChanges`. Repositories must not set them by hand. On update, `CreatedAt` and `CreatedBy` are not modified.

## Tests

```bash
cd ApiMovies
dotnet test ApiMovies.sln
```

## Docker

```bash
cd ApiMovies
export MOVIES_DB_PASSWORD=<sa-password>   # PowerShell: $env:MOVIES_DB_PASSWORD="..."
docker compose up --build
```

The API is available at `http://localhost:8000`. The container listens on port 8080.

## Notes

- **AutoMapper 12.0.1** has a reported high-severity advisory (GHSA-rvv3-g6hj-g44x). The fixed version is 13, which changes the license model. Review the terms before upgrading.
- If any test password was published in earlier versions of this README or the repository, treat it as exposed and rotate it.
