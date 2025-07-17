using MedicalEdu.Application.Models.Courses;

namespace MedicalEdu.Application.Courses.GetAll;

public sealed record GetAllCoursesResponse
{
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages - 1;
    public bool HasPreviousPage => Page > 0;
    public CourseResponse[] Courses { get; init; } = Array.Empty<CourseResponse>();
} 