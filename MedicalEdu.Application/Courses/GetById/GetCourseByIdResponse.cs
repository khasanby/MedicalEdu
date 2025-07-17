using MedicalEdu.Application.Models.Courses;

namespace MedicalEdu.Application.Courses.GetById;

/// <summary>
/// Response model for retrieving a course by its ID.
/// </summary>
/// <param name="Course"></param>
public sealed record GetCourseByIdResponse(CourseResponse Course); 