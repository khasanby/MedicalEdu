# Pagination and Filtering Implementation

This document describes the pagination and filtering implementation for the MedicalEdu application, specifically for the Course entity.

## Overview

The implementation follows a pattern inspired by modern backend architectures, providing:

- **Offset-based pagination** with configurable page size
- **Dynamic filtering** with multiple filter types
- **Flexible sorting** with multiple sort criteria
- **Performance optimization** using Entity Framework's `AsNoTracking()`
- **Type safety** with strongly-typed expressions

## Architecture

### 1. Request Models

#### GetCoursesRequest
Located in `MedicalEdu.Application/Models/Courses/GetCoursesRequest.cs`

```csharp
public class GetCoursesRequest
{
    // Pagination
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 25;

    // Basic Filters
    public bool? IsPublished { get; set; }
    public bool? IsActive { get; set; }
    public Guid? InstructorId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }

    // Price Filters
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Currency { get; set; }

    // Date Filters
    public DateTimeOffset? CreatedFrom { get; set; }
    public DateTimeOffset? CreatedTo { get; set; }
    public DateTimeOffset? PublishedFrom { get; set; }
    public DateTimeOffset? PublishedTo { get; set; }
    public DateTimeOffset? UpdatedFrom { get; set; }
    public DateTimeOffset? UpdatedTo { get; set; }

    // Duration Filters
    public int? MinDurationMinutes { get; set; }
    public int? MaxDurationMinutes { get; set; }

    // Student Capacity Filters
    public int? MinMaxStudents { get; set; }
    public int? MaxMaxStudents { get; set; }

    // Sorting
    public SortDirection? TitleSortDirection { get; set; }
    public SortDirection? PriceSortDirection { get; set; }
    public SortDirection? CreatedAtSortDirection { get; set; }
    public SortDirection? PublishedAtSortDirection { get; set; }
    public SortDirection? UpdatedAtSortDirection { get; set; }
    public SortDirection? DurationMinutesSortDirection { get; set; }
}
```

### 2. Response Models

#### GetCoursesResponse<T>
Located in `MedicalEdu.Application/Models/Courses/GetCoursesResponse.cs`

```csharp
public class GetCoursesResponse<T>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages - 1;
    public bool HasPreviousPage => Page > 0;
    public T[] Courses { get; set; } = Array.Empty<T>();
}
```

### 3. Repository Layer

#### ICourseRepository
Added new method to the interface:

```csharp
public ValueTask<(int TotalCount, T[] Results)> GetWithPagingAsync<T>(
    List<Expression<Func<Course, bool>>> conditions,
    Func<IQueryable<Course>, IQueryable<Course>> sortingFunc,
    (int Page, int PageSize) pagingOptions,
    Expression<Func<Course, T>> selector,
    CancellationToken cancellationToken = default);
```

#### CourseRepository Implementation
Located in `MedicalEdu.Infrastructure/DataAccess/Repositories/CourseRepository.cs`

The implementation:
1. Applies filter conditions using `Where()` clauses
2. Applies sorting using the provided sorting function
3. Gets total count before pagination
4. Applies pagination using `Skip()` and `Take()`
5. Projects results using the selector function

### 4. Service Layer

#### CourseService
Located in `MedicalEdu.Application/Services/CourseService.cs`

The service orchestrates:
- **Filter condition building** based on request parameters
- **Sorting function building** with multiple sort criteria
- **Repository interaction** with proper error handling

#### Filter Conditions
The service builds filter conditions dynamically:

```csharp
private static List<Expression<Func<Course, bool>>> GetFilterConditions(GetCoursesRequest request)
{
    var conditions = new List<Expression<Func<Course, bool>>>();

    if (request.IsPublished.HasValue)
        conditions.Add(c => c.IsPublished == request.IsPublished.Value);

    if (!string.IsNullOrWhiteSpace(request.Title))
        conditions.Add(c => c.Title.Contains(request.Title));

    if (request.MinPrice.HasValue)
        conditions.Add(c => c.Price >= request.MinPrice.Value);

    // ... more conditions
}
```

#### Sorting Function
The service builds sorting functions with multiple criteria:

```csharp
private static Func<IQueryable<Course>, IQueryable<Course>> GetSortingFunction(GetCoursesRequest request)
{
    return query =>
    {
        if (request.TitleSortDirection.HasValue)
        {
            query = request.TitleSortDirection.Value == SortDirection.Asc
                ? query.OrderBy(c => c.Title)
                : query.OrderByDescending(c => c.Title);
        }
        // ... more sorting criteria
        return query;
    };
}
```

### 5. API Controller

#### CoursesController
Added new endpoint in `MedicalEdu.Api/Controllers/CoursesController.cs`:

```csharp
[HttpGet("paginated")]
public async Task<ActionResult<GetCoursesResponse<CourseDto>>> GetCoursesPaginated(
    [FromQuery] GetCoursesRequest request,
    CancellationToken cancellationToken = default)
{
    var response = await _courseService.GetCoursesAsync(
        request,
        c => new CourseDto { /* mapping */ },
        cancellationToken);
    
    return Ok(response);
}
```

## Usage Examples

### 1. Basic Pagination
```http
GET /api/courses/paginated?page=0&pageSize=10
```

### 2. Filtering by Published Status
```http
GET /api/courses/paginated?isPublished=true&page=0&pageSize=20
```

### 3. Price Range Filtering
```http
GET /api/courses/paginated?minPrice=50&maxPrice=200&currency=USD
```

### 4. Text Search
```http
GET /api/courses/paginated?title=anatomy&description=medical
```

### 5. Date Range Filtering
```http
GET /api/courses/paginated?createdFrom=2024-01-01&createdTo=2024-12-31
```

### 6. Sorting
```http
GET /api/courses/paginated?titleSortDirection=asc&priceSortDirection=desc
```

### 7. Complex Filtering
```http
GET /api/courses/paginated?isPublished=true&minPrice=100&maxDurationMinutes=120&titleSortDirection=asc&page=0&pageSize=15
```

## Response Format

```json
{
  "totalCount": 150,
  "page": 0,
  "pageSize": 25,
  "totalPages": 6,
  "hasNextPage": true,
  "hasPreviousPage": false,
  "courses": [
    {
      "id": "guid-here",
      "title": "Anatomy Fundamentals",
      "description": "Learn the basics of human anatomy",
      "price": 99.99,
      "currency": "USD",
      "isPublished": true,
      "instructorName": "Dr. John Smith",
      "enrollmentCount": 45,
      "averageRating": 4.5,
      "ratingCount": 12,
      "materialCount": 8,
      "availabilitySlotCount": 3
    }
  ]
}
```

## Performance Considerations

1. **AsNoTracking()**: Used for read-only queries to improve performance
2. **Count before pagination**: Total count is calculated before applying pagination
3. **Expression-based filtering**: Filters are translated to SQL for database-level filtering
4. **Projection**: Only selected fields are retrieved from the database
5. **Indexing**: Ensure proper database indexes on frequently filtered fields

## Extending the Pattern

To implement this pattern for other entities:

1. Create request/response models for the entity
2. Add the `GetWithPagingAsync` method to the repository interface
3. Implement the method in the repository
4. Create a service class for filtering and sorting logic
5. Add API endpoints in the controller
6. Register services in DI container

## Benefits

- **Scalable**: Handles large datasets efficiently
- **Flexible**: Supports multiple filter and sort combinations
- **Type-safe**: Uses strongly-typed expressions
- **Performance-optimized**: Database-level filtering and pagination
- **Maintainable**: Clear separation of concerns
- **Testable**: Each layer can be tested independently 