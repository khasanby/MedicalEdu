# Entity Framework Core

## Overview
Entity Framework Core is the primary ORM (Object-Relational Mapper) used in MedicalEdu for database operations, migrations, and data access.

## 📦 Required Packages

### Core Packages
```xml
<!-- MedicalEdu.Infrastructure -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
```

### Development Tools
```bash
# Install EF Core CLI tools globally
dotnet tool install --global dotnet-ef

# Update EF Core tools
dotnet tool update --global dotnet-ef
```

## 🏗️ Project Structure

### DbContext Setup
```csharp
// MedicalEdu.Infrastructure/DataAccess/MedicalEduDbContext.cs
public class MedicalEduDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseMaterial> CourseMaterials { get; set; }
    public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MedicalEduDbContext).Assembly);
    }
}
```

## 🔧 Configuration

### Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MedicalEdu;Trusted_Connection=true;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### Service Registration
```csharp
// Program.cs or Startup.cs
services.AddDbContext<MedicalEduDbContext>(options =>
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
```

## 📊 Entity Configurations

### User Configuration
```csharp
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(254);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Role).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
    }
}
```

### Course Configuration
```csharp
public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.Price).HasColumnType("decimal(10,2)");
        
        builder.HasOne(c => c.Instructor)
               .WithMany(u => u.CoursesAsInstructor)
               .HasForeignKey(c => c.InstructorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
```

## 🚀 Migrations

### Creating Migrations
```bash
# Create initial migration
dotnet ef migrations add InitialCreate --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api

# Create migration for specific changes
dotnet ef migrations add AddUserProfileFields --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api

# Create migration with custom name
dotnet ef migrations add "Add payment processing tables" --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api
```

### Applying Migrations
```bash
# Apply all pending migrations
dotnet ef database update --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api

# Apply to specific migration
dotnet ef database update MigrationName --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api

# Generate SQL script
dotnet ef migrations script --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api
```

### Managing Migrations
```bash
# List all migrations
dotnet ef migrations list --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api

# Remove last migration (if not applied)
dotnet ef migrations remove --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api

# Update database to latest migration
dotnet ef database update --project MedicalEdu.Infrastructure --startup-project MedicalEdu.Api
```

## 🔍 Querying Data

### Repository Pattern
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public class Repository<T> : IRepository<T> where T : class
{
    private readonly MedicalEduDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(MedicalEduDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
```

### Complex Queries
```csharp
// Get published courses with instructor information
public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
{
    return await _context.Courses
        .Include(c => c.Instructor)
        .Where(c => c.IsPublished)
        .OrderByDescending(c => c.CreatedAt)
        .ToListAsync();
}

// Get user bookings with related data
public async Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId)
{
    return await _context.Bookings
        .Include(b => b.Course)
        .Include(b => b.Instructor)
        .Include(b => b.AvailabilitySlot)
        .Where(b => b.StudentId == userId)
        .OrderByDescending(b => b.CreatedAt)
        .ToListAsync();
}
```

## ⚡ Performance Optimization

### Indexing Strategy
```csharp
// Composite indexes for common queries
modelBuilder.Entity<AvailabilitySlot>()
    .HasIndex(a => new { a.IsBooked, a.StartTimeUtc });

modelBuilder.Entity<Course>()
    .HasIndex(c => new { c.IsPublished, c.CreatedAt });

modelBuilder.Entity<Booking>()
    .HasIndex(b => new { b.StudentId, b.Status });
```

### Query Optimization
```csharp
// Use AsNoTracking for read-only queries
public async Task<IEnumerable<Course>> GetPublishedCoursesReadOnlyAsync()
{
    return await _context.Courses
        .AsNoTracking()
        .Where(c => c.IsPublished)
        .ToListAsync();
}

// Use projection to select only needed fields
public async Task<IEnumerable<CourseSummaryDto>> GetCourseSummariesAsync()
{
    return await _context.Courses
        .Where(c => c.IsPublished)
        .Select(c => new CourseSummaryDto
        {
            Id = c.Id,
            Title = c.Title,
            InstructorName = c.Instructor.Name,
            Price = c.Price
        })
        .ToListAsync();
}
```

## 🔒 Security Considerations

### SQL Injection Prevention
- EF Core automatically parameterizes queries
- Use parameterized queries for raw SQL
- Validate all user inputs

### Data Access Patterns
```csharp
// Use repository pattern for data access
// Implement proper authorization checks
// Log sensitive operations
```

## 🧪 Testing

### In-Memory Database for Testing
```csharp
// Test project package
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.0.0" />

// Test setup
services.AddDbContext<MedicalEduDbContext>(options =>
    options.UseInMemoryDatabase("TestDatabase"));
```

### Integration Tests
```csharp
[Test]
public async Task CreateUser_ShouldSaveToDatabase()
{
    // Arrange
    var user = new User { Name = "Test User", Email = "test@example.com" };
    
    // Act
    await _repository.AddAsync(user);
    
    // Assert
    var savedUser = await _context.Users.FindAsync(user.Id);
    Assert.IsNotNull(savedUser);
    Assert.AreEqual("Test User", savedUser.Name);
}
```

## 📚 Best Practices

### Do's
- ✅ Use async/await for all database operations
- ✅ Implement proper error handling
- ✅ Use transactions for complex operations
- ✅ Optimize queries with Include() and Select()
- ✅ Use migrations for schema changes
- ✅ Implement repository pattern for data access

### Don'ts
- ❌ Don't use synchronous methods in async contexts
- ❌ Don't load unnecessary data with Include()
- ❌ Don't modify entities outside of DbContext
- ❌ Don't forget to dispose of DbContext
- ❌ Don't use raw SQL unless necessary

## 🔄 Troubleshooting

### Common Issues
1. **Migration conflicts**: Remove and recreate migrations
2. **Performance issues**: Use query optimization techniques
3. **Memory leaks**: Ensure proper disposal of DbContext
4. **Connection issues**: Check connection string and firewall settings

### Debugging Queries
```csharp
// Enable detailed logging
services.AddDbContext<MedicalEduDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});
```

---

**Entity Framework Core Documentation** - Complete guide to EF Core usage in MedicalEdu. 