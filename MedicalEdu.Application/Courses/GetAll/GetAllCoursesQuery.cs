using MediatR;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Application.Common.Interfaces;

namespace MedicalEdu.Application.Courses.GetAll;

public sealed record GetAllCoursesQuery : ICacheableRequest<GetAllCoursesResponse>
{
    // Pagination
    public int Page { get; init; } = 0;
    public int PageSize { get; init; } = 25;

    // Basic Filters
    public bool? IsPublished { get; init; }
    public bool? IsActive { get; init; }
    public Guid? InstructorId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? Tags { get; init; }

    // Price Filters
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? Currency { get; init; }

    // Date Filters
    public DateTimeOffset? CreatedFrom { get; init; }
    public DateTimeOffset? CreatedTo { get; init; }
    public DateTimeOffset? PublishedFrom { get; init; }
    public DateTimeOffset? PublishedTo { get; init; }
    public DateTimeOffset? UpdatedFrom { get; init; }
    public DateTimeOffset? UpdatedTo { get; init; }

    // Duration Filters
    public int? MinDurationMinutes { get; init; }
    public int? MaxDurationMinutes { get; init; }

    // Student Capacity Filters
    public int? MinMaxStudents { get; init; }
    public int? MaxMaxStudents { get; init; }

    // Sorting
    public SortDirection? TitleSortDirection { get; init; }
    public SortDirection? PriceSortDirection { get; init; }
    public SortDirection? CreatedAtSortDirection { get; init; }
    public SortDirection? PublishedAtSortDirection { get; init; }
    public SortDirection? UpdatedAtSortDirection { get; init; }
    public SortDirection? DurationMinutesSortDirection { get; init; }

    // ICacheableRequest implementation
    public TimeSpan CacheDuration => TimeSpan.FromMinutes(10);
}