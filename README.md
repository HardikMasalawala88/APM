# APM - CQRS Pattern Application

A clean architecture ASP.NET Core application implementing the CQRS (Command Query Responsibility Segregation) pattern with Entity Framework Core Code First approach.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Technologies](#technologies)
- [Features](#features)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [CQRS Pattern](#cqrs-pattern)
- [Entity Framework Configuration](#entity-framework-configuration)
- [Validation](#validation)
- [Project Structure Details](#project-structure-details)

## 🎯 Overview

This project demonstrates a well-structured ASP.NET Core application following:
- **Clean Architecture** principles
- **CQRS Pattern** for separating read and write operations
- **Entity Framework Core Code First** for database management
- **FluentValidation** for input validation
- **MediatR** for implementing the mediator pattern

## 🏗️ Architecture

The solution follows Clean Architecture with clear separation of concerns:

```
APM.Web (Presentation Layer)
    ↓
APM.Application (Application Layer - CQRS)
    ↓
APM.Domain (Domain Layer)
    ↑
APM.Infrastructure (Infrastructure Layer - EF Core, Data Access)
```

### Layer Responsibilities

- **APM.Web**: ASP.NET Core MVC application, controllers, views
- **APM.Application**: Commands, Queries, DTOs, Validators, Behaviors
- **APM.Domain**: Domain entities, interfaces
- **APM.Infrastructure**: DbContext, Entity configurations, Migrations

## 📁 Project Structure

```
APM/
├── APM.Web/                    # Presentation Layer (ASP.NET Core MVC)
│   ├── Controllers/            # MVC Controllers
│   ├── Views/                  # Razor Views
│   ├── Models/                 # View Models
│   ├── wwwroot/                # Static files
│   └── Program.cs              # Application entry point
│
├── APM.Application/            # Application Layer (CQRS)
│   ├── Commands/               # Write operations
│   │   └── Products/
│   │       ├── CreateProduct/
│   │       ├── UpdateProduct/
│   │       └── DeleteProduct/
│   ├── Queries/                # Read operations
│   │   └── Products/
│   │       ├── GetAllProducts/
│   │       └── GetProductById/
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Validators/             # FluentValidation validators
│   └── Behaviors/              # MediatR pipeline behaviors
│
├── APM.Domain/                 # Domain Layer
│   ├── Entities/               # Domain entities
│   └── Interfaces/             # Domain interfaces
│
└── APM.Infrastructure/         # Infrastructure Layer
    ├── Configurations/         # EF Core entity configurations
    ├── Migrations/             # Database migrations
    ├── ApplicationDbContext.cs  # DbContext implementation
    └── DependencyInjection.cs  # DI configuration
```

## 🛠️ Technologies

- **.NET 8.0**
- **ASP.NET Core MVC**
- **Entity Framework Core 8.0.21**
- **SQL Server**
- **MediatR 11.0.0** - For CQRS implementation
- **FluentValidation 12.1.0** - For input validation
- **Bootstrap 5** - For UI styling

## ✨ Features

- ✅ **CQRS Pattern**: Clear separation between commands (writes) and queries (reads)
- ✅ **Clean Architecture**: Layered architecture with dependency inversion
- ✅ **Entity Framework Code First**: Database schema managed through migrations
- ✅ **FluentValidation**: Automatic validation of commands
- ✅ **MediatR Pipeline Behaviors**: Cross-cutting concerns (validation)
- ✅ **DTOs**: Data Transfer Objects to separate domain from presentation
- ✅ **Automatic Timestamps**: CreatedAt and UpdatedAt fields managed automatically
- ✅ **Entity Configuration**: Fluent API configurations for entities

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB, Express, or full instance)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd APM
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Update connection string**
   
   Edit `APM.Web/appsettings.json` and update the connection string:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=ProductDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
     }
   }
   ```

4. **Run migrations**
   ```bash
   cd APM.Infrastructure
   dotnet ef migrations add Initial --startup-project ../APM.Web
   dotnet ef database update --startup-project ../APM.Web
   ```

5. **Run the application**
   ```bash
   cd APM.Web
   dotnet run
   ```

   Or use Visual Studio: Press F5 to run the application.

## 🗄️ Database Setup

### Using Entity Framework Migrations

The project uses EF Core Code First migrations. To create and apply migrations:

```bash
# Navigate to Infrastructure project
cd APM.Infrastructure

# Create a new migration
dotnet ef migrations add MigrationName --startup-project ../APM.Web

# Apply migrations to database
dotnet ef database update --startup-project ../APM.Web
```

### Connection String Configuration

Update the connection string in `APM.Web/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ProductDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  }
}
```

## 🔄 CQRS Pattern

### Commands (Write Operations)

Commands represent write operations and are located in `APM.Application/Commands/`:

- **CreateProductCommand**: Creates a new product
- **UpdateProductCommand**: Updates an existing product
- **DeleteProductCommand**: Deletes a product

Each command has:
- Command class (implements `IRequest` or `IRequest<TResponse>`)
- Command handler (implements `IRequestHandler<TCommand>`)
- Validator (implements `AbstractValidator<TCommand>`)

**Example - Create Product Command:**
```csharp
// Command
public record CreateProductCommand(string Name, decimal Price) : IRequest<int>;

// Handler
public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    // Implementation
}

// Validator
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    // Validation rules
}
```

### Queries (Read Operations)

Queries represent read operations and are located in `APM.Application/Queries/`:

- **GetAllProductsQuery**: Retrieves all products
- **GetProductByIdQuery**: Retrieves a product by ID

Each query has:
- Query class (implements `IRequest<TResponse>`)
- Query handler (implements `IRequestHandler<TQuery, TResponse>`)
- Returns DTOs instead of domain entities

**Example - Get All Products Query:**
```csharp
// Query
public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
{
}

// Handler
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    // Implementation
}
```

### Using Commands and Queries in Controllers

```csharp
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Query example
    public async Task<IActionResult> Index()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return View(products);
    }

    // Command example
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return RedirectToAction("Index");
    }
}
```

## 🗃️ Entity Framework Configuration

### Entity Configuration

Entity configurations are defined in `APM.Infrastructure/Configurations/` using Fluent API:

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
        // ... more configurations
    }
}
```

### Automatic Timestamps

The `ApplicationDbContext` automatically manages `CreatedAt` and `UpdatedAt` timestamps:

```csharp
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    // Automatically set timestamps on save
    // ...
}
```

### Product Entity

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
```

## ✅ Validation

Validation is handled using FluentValidation with automatic execution through MediatR pipeline behaviors:

### Validator Example

```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name must not exceed 200 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero.");
    }
}
```

### Validation Behavior

The `ValidationBehavior` automatically validates all requests before they reach handlers:

```csharp
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    // Validates request and throws ValidationException if invalid
}
```

## 📂 Project Structure Details

### APM.Web
- **Controllers/**: MVC controllers that use MediatR to send commands/queries
- **Views/**: Razor views for UI
- **Models/**: View models
- **Program.cs**: Service registration and middleware configuration

### APM.Application
- **Commands/**: Write operations organized by feature
- **Queries/**: Read operations organized by feature
- **DTOs/**: Data Transfer Objects for queries
- **Validators/**: FluentValidation validators
- **Behaviors/**: MediatR pipeline behaviors

### APM.Domain
- **Entities/**: Domain entities (Product, etc.)
- **Interfaces/**: Domain interfaces (IApplicationDbContext)

### APM.Infrastructure
- **Configurations/**: EF Core entity configurations
- **Migrations/**: Database migrations
- **ApplicationDbContext.cs**: DbContext implementation
- **DependencyInjection.cs**: Infrastructure service registration

## 🔧 Configuration

### Dependency Injection Setup

Services are registered in `APM.Web/Program.cs`:

```csharp
// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);

// Validation Behavior
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);
```

### Infrastructure Services

Infrastructure services are registered in `APM.Infrastructure/DependencyInjection.cs`:

```csharp
public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
{
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

    services.AddScoped<IApplicationDbContext>(provider =>
        provider.GetRequiredService<ApplicationDbContext>());

    return services;
}
```

## 📝 Notes

- All timestamps are stored in UTC
- DTOs are used to prevent exposing domain entities directly
- Validation errors are automatically handled by the validation behavior
- The project follows SOLID principles and Clean Architecture guidelines

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## 📄 License

This project is provided as-is for educational and demonstration purposes.

---

**Built with ❤️ using Clean Architecture and CQRS Pattern**

