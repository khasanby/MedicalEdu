using MedicalEdu.Application.Common.Attributes;
using MedicalEdu.Application.Common.Interfaces;
using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Application.Courses.Update;

[CacheInvalidation(new[] { CachePrefixes.GetAllCourses, CachePrefixes.GetCourseById, CachePrefixes.GetCoursesByInstructor, CachePrefixes.GetCoursesByCategory },
    "Course update affects course listings, individual course data, instructor's course list, and category listings")]
public sealed record UpdateCourseCommand : ICommand<UpdateCourseResponse>
{
    public Guid CourseId { get; init; }

    // Basic Information
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Content { get; init; }
    public CourseCategory? Category { get; init; }
    public DifficultyLevel? DifficultyLevel { get; init; }
    public string? Tags { get; init; }

    // Pricing
    public decimal? Price { get; init; }
    public string? Currency { get; init; }

    // Duration and Capacity
    public int? DurationMinutes { get; init; }
    public int? MaxStudents { get; init; }

    // Instructor
    public Guid? InstructorId { get; init; }

    // Publication Settings
    public bool? IsPublished { get; init; }
    public DateTimeOffset? PublishedAt { get; init; }

    // Course Materials
    public List<CourseMaterialDto>? Materials { get; init; }
}

public sealed record CourseMaterialDto
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string FileUrl { get; init; } = string.Empty;
    public string FileType { get; init; } = string.Empty;
    public int OrderIndex { get; init; }
}