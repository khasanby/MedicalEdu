using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Application.Models.Courses;

public sealed class GetCoursesRequest
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