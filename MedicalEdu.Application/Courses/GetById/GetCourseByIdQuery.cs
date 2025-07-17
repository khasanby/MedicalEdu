using MediatR;
using MedicalEdu.Application.Common.Interfaces;

namespace MedicalEdu.Application.Courses.GetById;

/// <summary>
/// Query to get a course by its ID.
/// </summary>
/// <param name="CourseId"></param>
public sealed record GetCourseByIdQuery(Guid CourseId) : ICacheableRequest<GetCourseByIdResponse?>
{
    // ICacheableRequest implementation
    public TimeSpan CacheDuration => TimeSpan.FromMinutes(30);
} 