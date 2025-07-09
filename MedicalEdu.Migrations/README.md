# MedicalEdu.Migrations

Console application for running database migrations for the MedicalEdu platform.

## Features

- **Runtime Migration Execution**: Apply database migrations at runtime
- **Environment-Specific Configuration**: Support for different environments (Development, Production)
- **Clear Status Reporting**: Success/failure messages for monitoring
- **Design-Time Support**: Still supports `dotnet ef migrations` commands
- **Database Creation**: Automatically creates database if it doesn't exist

## Usage

### Running Migrations

```bash
# Run migrations (uses current directory's appsettings.json)
dotnet run

# Run with specific environment
ASPNETCORE_ENVIRONMENT=Production dotnet run

# Run from different directory
dotnet run --project MedicalEdu.Migrations
```

### Creating New Migrations

```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project ../MedicalEdu.Infrastructure -o "DataAccess/Migrations"

# Examples:
dotnet ef migrations add Initial --project ../MedicalEdu.Infrastructure -o "DataAccess/Migrations"
dotnet ef migrations add AddUserTable --project ../MedicalEdu.Infrastructure -o "DataAccess/Migrations"
dotnet ef migrations add AddCourseTable --project ../MedicalEdu.Infrastructure -o "DataAccess/Migrations"
```

### Managing Migrations

```bash
# List all migrations
dotnet ef migrations list --project ../MedicalEdu.Infrastructure

# Remove last migration (if not applied)
dotnet ef migrations remove --project ../MedicalEdu.Infrastructure

# Update database to specific migration
dotnet ef database update <MigrationName> --project ../MedicalEdu.Infrastructure
```

## Configuration

The application supports multiple configuration sources:

### Connection Strings

The application looks for connection strings in this order:
1. `DefaultConnection`
2. `Default`
3. `MedDB`

### Environment-Specific Settings

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development environment
- `appsettings.Production.json` - Production environment

### Environment Variables

Set `ASPNETCORE_ENVIRONMENT` to control which configuration file is loaded:
- `Development` (default)
- `Production`

## Output Messages

The application provides clear status messages:

```
MedicalEdu.Migrations: STARTED.
Connecting to database...
Found 2 pending migrations: 20240101000000_Initial, 20240102000000_AddUserTable
All migrations applied successfully.
Applied migrations: 2
MedicalEdu.Migrations: SUCCESS.
```

Or on failure:

```
MedicalEdu.Migrations: STARTED.
MedicalEdu.Migrations: FAILED.
Error: Connection failed
Inner Error: Invalid username or password
```

## Examples

### Development Environment

```bash
# Set environment
export ASPNETCORE_ENVIRONMENT=Development

# Run migrations
dotnet run
```

### Production Environment

```bash
# Set environment
export ASPNETCORE_ENVIRONMENT=Production

# Run migrations
dotnet run
```

### Custom Connection String

```bash
# Override connection string via environment variable
export ConnectionStrings__DefaultConnection="Host=prod-server;Database=MedicalEdu;Username=app_user;Password=secure_password"
dotnet run
```

## Troubleshooting

### Common Issues

1. **Connection String Not Found**
   - Ensure one of the supported connection string names is configured
   - Check that appsettings.json is in the correct location

2. **Database Connection Failed**
   - Verify PostgreSQL server is running
   - Check connection string parameters
   - Ensure database user has proper permissions

3. **Migration Already Applied**
   - This is normal - the application will skip already applied migrations

### Debug Mode

For detailed logging, set the environment to Development:

```bash
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

This will show SQL commands and detailed migration information. 