# Technologies & Dependencies

This section documents all technologies, packages, and tools used in the MedicalEdu platform, including installation instructions and configuration details.

## 📋 Technology Stack Overview

### Backend Framework
- **ASP.NET Core 8.0** - Web API framework
- **C# 12.0** - Programming language

### Database & ORM
- **SQL Server** - Primary database
- **Entity Framework Core** - ORM framework
- **EF Core Tools** - Migration and design-time tools

### Mapping & Utilities
- **AutoMapper** - Object-to-object mapping
- **FluentValidation** - Validation framework

### Authentication & Security
- **JWT Bearer Tokens** - Authentication
- **BCrypt** - Password hashing

### File Storage
- **Azure Blob Storage** - File upload storage

### Payment Processing
- **Stripe.NET** - Stripe payment integration
- **PayPal SDK** - PayPal payment integration

### Email & Notifications
- **SendGrid** - Email service (or SMTP)

### Development Tools
- **Swagger/OpenAPI** - API documentation
- **Serilog** - Structured logging

## 📁 Documentation Structure

- **[EntityFramework.md](EntityFramework.md)** - EF Core setup and usage
- **[AutoMapper.md](AutoMapper.md)** - AutoMapper configuration and mapping
- **[Authentication.md](Authentication.md)** - JWT and security setup
- **[FileStorage.md](FileStorage.md)** - Azure Blob Storage integration
- **[Payments.md](Payments.md)** - Payment provider integrations
- **[DevelopmentTools.md](DevelopmentTools.md)** - Development and debugging tools

## 🚀 Quick Installation

### Prerequisites
```bash
# Install .NET 8.0 SDK
# Install SQL Server (Local or Azure)
# Install Azure Storage Emulator (optional for local development)
```

### Package Installation
```bash
# Navigate to project directory
cd MedicalEdu

# Restore packages
dotnet restore

# Install EF Core tools globally (if not already installed)
dotnet tool install --global dotnet-ef
```

### Database Setup
```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update
```

## 🔧 Configuration

### Connection Strings
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MedicalEdu;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

### Package References
Each project's `.csproj` file contains the necessary package references. See individual technology documentation for specific versions and configurations.

## 📦 Package Management

### Adding New Packages
```bash
# Add package to specific project
dotnet add MedicalEdu.Api package PackageName

# Add package to all projects (if needed)
dotnet add MedicalEdu.Application package PackageName
dotnet add MedicalEdu.Infrastructure package PackageName
```

### Updating Packages
```bash
# Update all packages
dotnet list package --outdated
dotnet add package PackageName --version NewVersion
```

## 🔄 Version Management

### Current Versions
- Entity Framework Core: 8.x
- AutoMapper: 12.x
- ASP.NET Core: 8.0
- SQL Server: 2022 or Azure SQL

### Version Compatibility
- All packages are compatible with .NET 8.0
- EF Core 8.x requires .NET 8.0
- AutoMapper 12.x supports .NET 8.0

## 🛠️ Development Environment

### Required Tools
- Visual Studio 2022 or VS Code
- SQL Server Management Studio (SSMS) or Azure Data Studio
- Postman or similar API testing tool
- Git for version control

### Optional Tools
- Azure Storage Explorer
- Redis Desktop Manager (if using Redis)
- Docker Desktop (for containerized development)

## 📚 Learning Resources

### Official Documentation
- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [AutoMapper Documentation](https://docs.automapper.org/)
- [Stripe.NET Documentation](https://stripe.com/docs/api/dotnet)

### Community Resources
- Stack Overflow tags: asp.net-core, entity-framework-core, automapper
- GitHub repositories for examples and best practices

---

**Technologies Documentation** - Comprehensive guide to all tools and packages used in MedicalEdu. 