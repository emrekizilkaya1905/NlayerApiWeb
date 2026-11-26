# NLayer API

A .NET 10 learning project for managing products and categories with ASP.NET Core, Entity Framework Core and SQL Server. The application uses three layers to separate HTTP endpoints, business logic and data access.

## Architecture

```text
Client / Swagger / HTTP requests
             ↓
Api — Controllers and HTTP responses
             ↓
Services — Business rules, DTOs, validation and mapping
             ↓
Repositories — Entities, repositories, EF Core and UnitOfWork
             ↓
SQL Server / LocalDB
```

| Project | Location | Responsibility |
| --- | --- | --- |
| App.Api | Api/ | Controllers, CustomBaseController, startup configuration and OpenAPI |
| App.Services | Services/ | Product/category services, request and response models, AutoMapper, FluentValidation and ServiceResult |
| App.Repositories | Repositories/ | Entities, configurations, generic/custom repositories, AppDbContext, UnitOfWork and migrations |

Project references follow `Api → Services → Repositories`. Entity classes live in the repository project in this implementation.

Controllers call services. Services use repositories for database access and UnitOfWork to save changes. ServiceResult carries data, errors and a status code; CustomBaseController converts that result into an HTTP response.

## Repository layout

```text
NlayerApiWeb.slnx
├── Api/
│   ├── Controllers/
│   ├── Properties/launchSettings.json
│   ├── Api.http
│   └── Program.cs
├── Services/
│   ├── Products/
│   ├── Categories/
│   ├── Extensions/
│   ├── Filters/
│   ├── ExceptionHandlers/
│   ├── FluentValidationFilter.cs
│   └── ServiceResult.cs
└── Repositories/
    ├── Products/
    ├── Categories/
    ├── Extensions/
    ├── Interceptors/
    ├── Migrations/
    ├── AppDbContext.cs
    ├── GenericRepository.cs
    └── UnitOfWork.cs
```

Product and category folders group related files together. For example, Repositories/Products contains the Product entity, its configuration and its custom repository interface/implementation.

## Main components

- **GenericRepository<T, TId>:** shared lookup, query, add, update and delete operations.
- **Custom repositories:** queries specific to products or categories, including categories with their products.
- **UnitOfWork:** persists tracked changes through SaveChangesAsync.
- **AutoMapper:** maps request models, entities and response DTOs.
- **FluentValidation:** validates requests that have registered validators.
- **FluentValidationFilter:** converts invalid ModelState into a 400 ServiceResult response. This project deliberately disables the default ApiController ModelState filter in favor of this filter.
- **NotFoundFilter:** checks existence before selected update/delete actions.
- **Audit interceptor:** sets CreatedDate and UpdatedDate for entities implementing IAuditEntity.
- **EF Core configurations:** loaded from the repository assembly in OnModelCreating.

A category has multiple products; each product has a CategoryId and belongs to a category.

## Run locally

### Requirements

- .NET 10 SDK.
- SQL Server or SQL Server Express LocalDB (Windows).
- Visual Studio with .NET support, or a terminal and dotnet-ef.

Open `NlayerApiWeb.slnx` as a solution. In Visual Studio, set **App.Api** as the startup project.

### Configure the connection

The development connection is configured in `Api/appsettings.Development.json` under `ConnectionStrings:SqlServer`.

The current configuration uses:

- Server: `(localdb)\NlayerLocalDB`
- Database: `NetNlayerAndCleanArchDb`
- Windows integrated authentication.

Use a SQL Server instance available on your machine. The custom LocalDB instance name is not created automatically by the application. Select a separate database name if you want to keep this sample isolated from other projects.

### Restore, build and apply migrations

Run from the repository root:

```powershell
dotnet restore NlayerApiWeb.slnx
dotnet build NlayerApiWeb.slnx
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef database update --project Repositories --startup-project Api --context AppDbContext
```

If the CLI tool is missing, install a version matching the project's EF Core packages:

```powershell
dotnet tool install --global dotnet-ef --version 10.0.12
```

Alternatively, use Visual Studio Package Manager Console:

```powershell
Update-Database -Project App.Repositories -StartupProject App.Api -Context AppDbContext -Args '--environment Development'
```

Apply the committed migrations; a fresh checkout does not require another InitialCreate migration.

### Start the API

```powershell
dotnet run --project Api --launch-profile https
```

If needed, trust the local development certificate:

```powershell
dotnet dev-certs https --trust
```

| Resource | URL |
| --- | --- |
| HTTPS Swagger UI | https://localhost:7277/swagger |
| HTTP Swagger UI | http://localhost:5295/swagger |
| HTTPS OpenAPI document | https://localhost:7277/openapi/v1.json |

Use `--launch-profile http` for the HTTP profile. Visual Studio profiles open Swagger automatically; from a terminal, open the URL manually. Swagger and the OpenAPI endpoint are enabled in Development.

## Product endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| GET | /api/products | List products |
| GET | /api/products/{pageNumber}/{pageSize} | Get a page of products |
| GET | /api/products/{id} | Get a product |
| POST | /api/products | Create a product |
| PUT | /api/products/{id} | Update a product |
| PATCH | /api/products/stock | Submit a stock update |
| DELETE | /api/products/{id} | Delete a product |

Pagination uses route values, for example `/api/products/1/10`.

Create/update request example:

```json
{
  "name": "Notebook",
  "price": 49.90,
  "stock": 20,
  "categoryId": 1
}
```

Use an existing category ID returned by the API rather than assuming ID 1 exists.

Stock request shape:

```json
{
  "productId": 1,
  "quantity": 5
}
```

## Category endpoints

| Method | Route | Purpose |
| --- | --- | --- |
| GET | /api/categories | List categories with products |
| GET | /api/categories/{id} | Get a category |
| GET | /api/categories/{id}/products | Get a category with its products |
| GET | /api/categories/products | List categories with products |
| POST | /api/categories | Create a category |
| PUT | /api/categories/{id} | Update a category |
| DELETE | /api/categories/{id} | Delete a category |

```json
{
  "name": "Stationery"
}
```

The two category-list routes currently call the same service method.

## Branches and Visual Studio

- **master:** the three NLayer projects documented here.
- **Clean-Arc:** also contains Clean Architecture projects and virtual solution folders grouping CleanArch and NLayer.

Both branches use `NlayerApiWeb.slnx`, but its contents differ. If Solution Explorer does not refresh after changing branches, close the solution and reopen the `.slnx` file with **Open Project/Solution**. Opening the directory with **Open Folder** provides a different view.

## Trying the API

Use Swagger, Postman or the requests in `Api/Api.http`. Start by creating a category, then create products with the returned category ID. Verify updates through a separate GET request and check invalid requests and missing resources.

This is a learning project. The endpoint list above reflects the current controller code; this README does not claim that every endpoint has passed integration tests.
