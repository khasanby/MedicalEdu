# AutoMapper

## Overview
AutoMapper is used in MedicalEdu for object-to-object mapping between domain entities and DTOs (Data Transfer Objects), reducing boilerplate code and improving maintainability.

## 📦 Required Packages

### Core Package
```xml
<!-- MedicalEdu.Application -->
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
```

## 🔧 Configuration

### Service Registration
```csharp
// Program.cs or Startup.cs
services.AddAutoMapper(typeof(Program).Assembly);
```

### Profile Setup
```csharp
// MedicalEdu.Application/Mapping/MappingProfile.cs
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>();
        CreateMap<User, UserSummaryDto>();
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserRequest, User>();

        // Course mappings
        CreateMap<Course, CourseDto>();
        CreateMap<Course, CourseSummaryDto>();
        CreateMap<CreateCourseRequest, Course>();
        CreateMap<UpdateCourseRequest, Course>();

        // Booking mappings
        CreateMap<Booking, BookingDto>();
        CreateMap<Booking, BookingSummaryDto>();
        CreateMap<CreateBookingRequest, Booking>();

        // Payment mappings
        CreateMap<Payment, PaymentDto>();
        CreateMap<Payment, PaymentSummaryDto>();
    }
}
```

## 📊 Mapping Examples

### Basic Entity to DTO Mapping
```csharp
// Domain Entity
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

// DTO
public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
}

// Mapping Configuration
CreateMap<User, UserDto>()
    .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
```

### Complex Mapping with Related Entities
```csharp
// Course with Instructor
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public string InstructorName { get; set; }
    public string InstructorEmail { get; set; }
    public List<CourseMaterialDto> Materials { get; set; }
}

// Mapping Configuration
CreateMap<Course, CourseDto>()
    .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name))
    .ForMember(dest => dest.InstructorEmail, opt => opt.MapFrom(src => src.Instructor.Email))
    .ForMember(dest => dest.Materials, opt => opt.MapFrom(src => src.Materials.OrderBy(m => m.Order)));
```

### Request to Entity Mapping
```csharp
// Create Request
public class CreateCourseRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
}

// Mapping Configuration
CreateMap<CreateCourseRequest, Course>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.InstructorId, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
    .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
    .ForMember(dest => dest.IsPublished, opt => opt.MapFrom(src => false));
```

## 🔄 Advanced Mapping

### Conditional Mapping
```csharp
CreateMap<User, UserSummaryDto>()
    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => 
        !string.IsNullOrEmpty(src.Name) ? src.Name : src.Email.Split('@')[0]))
    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => 
        src.IsActive && src.EmailConfirmed));
```

### Custom Value Resolvers
```csharp
public class FullNameResolver : IValueResolver<User, UserDto, string>
{
    public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
    {
        return $"{source.Name} ({source.Email})";
    }
}

// Usage in mapping
CreateMap<User, UserDto>()
    .ForMember(dest => dest.FullName, opt => opt.MapFrom<FullNameResolver>());
```

### Nested Object Mapping
```csharp
// Booking with related entities
public class BookingDetailDto
{
    public Guid Id { get; set; }
    public string CourseTitle { get; set; }
    public string InstructorName { get; set; }
    public string StudentName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
}

CreateMap<Booking, BookingDetailDto>()
    .ForMember(dest => dest.CourseTitle, opt => opt.MapFrom(src => src.Course.Title))
    .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.Name))
    .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
    .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.AvailabilitySlot.StartTimeUtc))
    .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.AvailabilitySlot.EndTimeUtc))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
```

## 🏗️ Service Layer Integration

### Repository with AutoMapper
```csharp
public class CourseService : ICourseService
{
    private readonly IRepository<Course> _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(IRepository<Course> courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CourseSummaryDto>> GetPublishedCoursesAsync()
    {
        var courses = await _courseRepository.GetAllAsync();
        var publishedCourses = courses.Where(c => c.IsPublished);
        
        return _mapper.Map<IEnumerable<CourseSummaryDto>>(publishedCourses);
    }

    public async Task<CourseDto> GetCourseByIdAsync(Guid id)
    {
        var course = await _courseRepository.GetByIdAsync(id);
        if (course == null)
            throw new NotFoundException($"Course with ID {id} not found.");
        
        return _mapper.Map<CourseDto>(course);
    }

    public async Task<CourseDto> CreateCourseAsync(CreateCourseRequest request, Guid instructorId)
    {
        var course = _mapper.Map<Course>(request);
        course.InstructorId = instructorId;
        
        var createdCourse = await _courseRepository.AddAsync(course);
        return _mapper.Map<CourseDto>(createdCourse);
    }
}
```

## 📋 DTOs and Request Models

### User DTOs
```csharp
public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
    public bool EmailConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UserSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}

public class CreateUserRequest
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}

public class UpdateUserRequest
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string TimeZone { get; set; }
}
```

### Course DTOs
```csharp
public class CourseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublished { get; set; }
    public decimal? Price { get; set; }
    public string ThumbnailUrl { get; set; }
    public string InstructorName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CourseMaterialDto> Materials { get; set; }
}

public class CourseSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal? Price { get; set; }
    public string InstructorName { get; set; }
    public int MaterialCount { get; set; }
}
```

## ⚡ Performance Optimization

### Projection Mapping
```csharp
// Use projection for better performance
public async Task<IEnumerable<CourseSummaryDto>> GetCourseSummariesAsync()
{
    return await _context.Courses
        .Where(c => c.IsPublished)
        .Select(c => new CourseSummaryDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            Price = c.Price,
            InstructorName = c.Instructor.Name,
            MaterialCount = c.Materials.Count
        })
        .ToListAsync();
}
```

### Lazy Loading Considerations
```csharp
// Explicitly include related entities when needed
public async Task<CourseDto> GetCourseWithMaterialsAsync(Guid id)
{
    var course = await _context.Courses
        .Include(c => c.Instructor)
        .Include(c => c.Materials.OrderBy(m => m.Order))
        .FirstOrDefaultAsync(c => c.Id == id);
    
    return _mapper.Map<CourseDto>(course);
}
```

## 🧪 Testing

### Unit Testing AutoMapper
```csharp
[Test]
public void AutoMapper_Configuration_ShouldBeValid()
{
    // Arrange
    var config = new MapperConfiguration(cfg => 
        cfg.AddProfile<MappingProfile>());

    // Act & Assert
    config.AssertConfigurationIsValid();
}

[Test]
public void Map_UserToUserDto_ShouldMapCorrectly()
{
    // Arrange
    var mapper = new Mapper(new MapperConfiguration(cfg => 
        cfg.AddProfile<MappingProfile>()));
    
    var user = new User
    {
        Id = Guid.NewGuid(),
        Name = "John Doe",
        Email = "john@example.com",
        Role = UserRole.Student
    };

    // Act
    var userDto = mapper.Map<UserDto>(user);

    // Assert
    Assert.AreEqual(user.Id, userDto.Id);
    Assert.AreEqual(user.Name, userDto.Name);
    Assert.AreEqual(user.Email, userDto.Email);
    Assert.AreEqual(user.Role.ToString(), userDto.Role);
}
```

## 📚 Best Practices

### Do's
- ✅ Create separate DTOs for different use cases
- ✅ Use explicit mapping configurations
- ✅ Validate AutoMapper configuration in tests
- ✅ Use projection for read-only queries
- ✅ Keep mapping profiles organized and documented

### Don'ts
- ❌ Don't map sensitive data to DTOs
- ❌ Don't create overly complex mappings
- ❌ Don't use AutoMapper for simple property assignments
- ❌ Don't forget to handle null values
- ❌ Don't map circular references

### Naming Conventions
```csharp
// DTOs: EntityName + "Dto"
public class UserDto { }
public class CourseDto { }

// Summary DTOs: EntityName + "SummaryDto"
public class UserSummaryDto { }
public class CourseSummaryDto { }

// Request DTOs: Action + EntityName + "Request"
public class CreateUserRequest { }
public class UpdateUserRequest { }

// Response DTOs: Action + EntityName + "Response"
public class CreateUserResponse { }
public class UpdateUserResponse { }
```

## 🔄 Troubleshooting

### Common Issues
1. **Configuration validation errors**: Check mapping profiles
2. **Null reference exceptions**: Handle null values in mappings
3. **Performance issues**: Use projection for large datasets
4. **Circular references**: Configure mapping to handle cycles

### Debugging Mappings
```csharp
// Enable detailed mapping validation
var config = new MapperConfiguration(cfg => 
{
    cfg.AddProfile<MappingProfile>();
    cfg.CreateMissingTypeMaps = false;
    cfg.ValidateInlineMaps = false;
});

config.AssertConfigurationIsValid();
```

---

**AutoMapper Documentation** - Complete guide to object mapping in MedicalEdu. 