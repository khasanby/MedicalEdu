namespace MedicalEdu.Application.Courses.Create;

/// <summary>
/// Response for the CreateCourse command.
/// </summary>
/// <param name="CourseId"></param>
public sealed record CreateCourseResponse(Guid CourseId); 