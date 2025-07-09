# Database Migration Tool Documentation

## Overview

The `MedicalEdu.Migrations` project is a standalone console application designed to handle database creation and migration application for the MedicalEdu platform. It provides a robust, production-ready solution for managing database schema changes across different environments.

## Features

### ✅ Standalone Console Application
- Runs independently of the main API
- No dependency on web server or hosting environment
- Perfect for CI/CD pipelines and containerized deployments

### ✅ Environment-Specific Configuration
- Supports Development, Staging, and Production environments
- Uses standard .NET configuration patterns
- Environment-specific connection strings and settings

### ✅ Comprehensive Logging
- Detailed logging of migration progress
- Database operation tracking
- Error reporting and debugging information
- Structured logging with different log levels

### ✅ Error Handling
- Graceful handling of connection issues
- Migration conflict resolution
- Database creation error handling
- Clear error messages and troubleshooting guidance

### ✅ Database Creation
- Automatically creates database if it doesn't exist
- Handles PostgreSQL-specific extensions (uuid-ossp)
- Proper database initialization

## Project Structure

```
MedicalEdu.Migrations/
├── Program.cs                    # Main application entry point
├── MedicalEduDbContextFactory.cs # EF Core context factory
├── appsettings.json             # Default configuration
├── appsettings.Development.json # Development environment
├── MedicalEdu.Migrations.csproj # Project file
└── bin/                         # Compiled output
```

## Usage

### Basic Usage

```bash
# Navigate to migrations directory
cd MedicalEdu.Migrations

# Run migrations (uses appsettings.json by default)
dotnet run
```

### Environment-Specific Usage

```bash
# Development environment
dotnet run --environment Development

# Production environment
dotnet run --environment Production

# Custom configuration
dotnet run --configuration Release
```

### CI/CD Integration

```bash
# Build the migration tool
dotnet build MedicalEdu.Migrations

# Run in production environment
dotnet run --project MedicalEdu.Migrations --environment Production
```

## Configuration

### Configuration Files

The migration tool uses the same configuration structure as the main API:

- **`appsettings.json`** - Default configuration
- **`appsettings.Development.json`** - Development environment
- **`appsettings.Production.json`** - Production environment

### Connection String Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=MedicalEdu;Username=postgres;Password=your_password"
  }
}
```

### Logging Configuration

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

## Migration Process

### 1. Connection Establishment
- Validates connection string configuration
- Attempts to connect to the database server
- Handles connection errors gracefully

### 2. Database Creation
- Checks if database exists
- Creates database if it doesn't exist
- Installs required PostgreSQL extensions (uuid-ossp)

### 3. Migration History Setup
- Creates `__EFMigrationsHistory` table
- Tracks applied migrations and their versions
- Ensures migration integrity

### 4. Migration Application
- Identifies pending migrations
- Applies migrations in chronological order
- Creates tables, indexes, and constraints
- Records migration completion

### 5. Verification
- Confirms all migrations are applied
- Validates database schema
- Reports migration status

## Database Schema

### Core Entities (15 Total)

#### User Management
- **Users** - Multi-role user management with email verification
- **UserSessions** - User session management and authentication

#### Course Management
- **Courses** - Course creation and publishing workflow
- **CourseMaterials** - File upload and content management
- **CourseRatings** - Student course ratings and reviews
- **Enrollments** - Student course enrollment tracking
- **CourseProgresses** - Student progress tracking per course material

#### Booking & Scheduling
- **AvailabilitySlots** - Instructor scheduling system
- **Bookings** - Student booking lifecycle management
- **BookingPromoCodes** - Promotional code application to bookings

#### Payment & Financial
- **Payments** - Multi-provider payment processing
- **PromoCodes** - Discount and promotional code management

#### Communication & Analytics
- **Notifications** - System-wide notification management
- **InstructorRatings** - Student ratings for instructors
- **AuditLogs** - Comprehensive activity tracking

### Database Features

- **UUID Primary Keys** - All entities use UUID primary keys for security
- **Audit Trail** - Comprehensive audit logging with user tracking
- **Soft Deletes** - Support for soft deletion where applicable
- **Optimized Indexes** - Performance-optimized database indexes
- **Foreign Key Constraints** - Proper referential integrity
- **Time Zone Support** - UTC timestamps with timezone awareness

## Troubleshooting

### Common Issues

#### Database Already Exists
**Problem**: Tables exist from previous runs causing conflicts
**Solution**: 
```bash
# Drop database manually
psql -h localhost -U postgres -c "DROP DATABASE \"MedicalEdu\";"

# Or use EF Core CLI
dotnet ef database drop --project MedicalEdu.Migrations
```

#### Connection Issues
**Problem**: Cannot connect to database
**Solution**:
1. Verify connection string in `appsettings.json`
2. Check PostgreSQL server is running
3. Confirm database credentials
4. Test connection manually

#### Migration Conflicts
**Problem**: Pending migrations exist
**Solution**:
1. Check migration history: `dotnet ef migrations list`
2. Remove conflicting migrations if needed
3. Ensure clean migration state

#### Permission Issues
**Problem**: Insufficient database permissions
**Solution**:
1. Grant necessary permissions to database user
2. Ensure user can create databases and tables
3. Check PostgreSQL role configuration

### Error Messages

#### Connection Error
```
fail: RelationalEventId.ConnectionError[20004]
An error occurred using the connection to database 'MedicalEdu'
```
**Action**: Check connection string and database server status

#### Migration Already Applied
```
fail: Microsoft.EntityFrameworkCore.Database.Command[20100]
Migration '20250709110227_Initial' already applied
```
**Action**: Database is up to date, no action needed

#### Table Already Exists
```
fail: Microsoft.EntityFrameworkCore.Database.Command[20100]
relation "Users" already exists
```
**Action**: Drop database and rerun migrations

## Best Practices

### Development Workflow

1. **Create Migration**: Use EF Core CLI to create migrations
   ```bash
   dotnet ef migrations add Initial --project MedicalEdu.Migrations
   ```

2. **Test Migration**: Run migration tool to test changes
   ```bash
   cd MedicalEdu.Migrations
   dotnet run
   ```

3. **Verify Schema**: Check database schema matches expectations
   ```bash
   psql -h localhost -U postgres -d MedicalEdu -c "\dt"
   ```

### Production Deployment

1. **Backup Database**: Always backup before migrations
2. **Test in Staging**: Test migrations in staging environment first
3. **Monitor Logs**: Watch migration logs for any issues
4. **Verify Data**: Confirm data integrity after migration

### Migration Naming

- Use descriptive migration names
- Include date prefix for chronological ordering
- Example: `20250709110227_Initial`

## Integration with CI/CD

### Azure DevOps Pipeline

```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run Database Migrations'
  inputs:
    command: 'run'
    projects: 'MedicalEdu.Migrations/MedicalEdu.Migrations.csproj'
    arguments: '--environment Production'
```

### GitHub Actions

```yaml
- name: Run Database Migrations
  run: |
    cd MedicalEdu.Migrations
    dotnet run --environment Production
```

### Docker Deployment

```dockerfile
# Build migration tool
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MedicalEdu.Migrations/MedicalEdu.Migrations.csproj", "MedicalEdu.Migrations/"]
RUN dotnet restore "MedicalEdu.Migrations/MedicalEdu.Migrations.csproj"
COPY . .
RUN dotnet build "MedicalEdu.Migrations/MedicalEdu.Migrations.csproj" -c Release -o /app/build

# Run migrations
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "MedicalEdu.Migrations.dll"]
```

## Security Considerations

### Connection String Security
- Use environment variables for sensitive connection strings
- Avoid hardcoding credentials in configuration files
- Use managed identities in cloud environments

### Database Permissions
- Grant minimal required permissions
- Use dedicated database user for migrations
- Implement proper access controls

### Audit Logging
- All database changes are logged
- User tracking for all operations
- Comprehensive audit trail maintained

## Performance Optimization

### Migration Performance
- Batch operations where possible
- Use appropriate indexes
- Optimize large table migrations

### Database Optimization
- Regular index maintenance
- Monitor query performance
- Implement proper partitioning strategies

## Monitoring and Alerting

### Migration Monitoring
- Track migration execution time
- Monitor database size changes
- Alert on migration failures

### Health Checks
- Database connectivity checks
- Schema validation
- Migration status verification

---

**Note**: This migration tool is designed to be production-ready and follows best practices for database migration management. Always test migrations in a staging environment before applying to production. 