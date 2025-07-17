using MedicalEdu.Application.Models.Courses;
using MedicalEdu.Domain.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MedicalEdu.Application.Courses.GetById;

internal sealed class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, GetCourseByIdResponse?>
{
    private readonly IMedicalEduDbContext _dbContext;

    public GetCourseByIdQueryHandler(IMedicalEduDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetCourseByIdResponse?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _dbContext.Courses
            .AsNoTracking()
            .Where(c => c.Id == request.CourseId)
            .Select(course => new CourseResponse
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                ShortDescription = course.ShortDescription,
                Price = course.Price != null ? course.Price.Amount : 0,
                Currency = course.Price != null ? course.Price.Currency.Code : "USD",
                ThumbnailUrl = course.ThumbnailUrl != null ? course.ThumbnailUrl.Value : null,
                VideoIntroUrl = course.VideoIntroUrl != null ? course.VideoIntroUrl.Value : null,
                DurationMinutes = course.DurationMinutes,
                MaxStudents = course.MaxStudents,
                Category = course.Category,
                Tags = course.Tags,
                IsPublished = course.IsPublished,
                IsActive = course.IsActive,
                InstructorId = course.InstructorId,
                CreatedAt = course.CreatedAt,
                UpdatedAt = course.UpdatedAt,
                PublishedAt = course.PublishedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return course != null ? new GetCourseByIdResponse(course) : null;
    }
}