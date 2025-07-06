using System;

namespace MedicalEdu.Application.Models.Courses;

/// <summary>
/// Response model for course information in paginated results.
/// </summary>
public class CourseResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public bool IsPublished { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? VideoIntroUrl { get; set; }
    public int? DurationMinutes { get; set; }
    public int? MaxStudents { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? PublishedAt { get; set; }
    public Guid InstructorId { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public int EnrollmentCount { get; set; }
    public double? AverageRating { get; set; }
    public int RatingCount { get; set; }
    public int MaterialCount { get; set; }
    public int AvailabilitySlotCount { get; set; }
    public string DifficultyLevel { get; set; } = string.Empty;
} 