using MediatR;
using MedicalEdu.Domain.DataAccess.Repositories;
using MedicalEdu.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MedicalEdu.Application.Courses.Update;

public sealed class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, UpdateCourseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateCourseCommandHandler> _logger;

    public UpdateCourseCommandHandler(
        ICourseRepository courseRepository,
        IUserRepository userRepository,
        ILogger<UpdateCourseCommandHandler> logger)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UpdateCourseResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating course with ID: {CourseId}", request.CourseId);

        // Get existing course
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course == null)
        {
            _logger.LogWarning("Course with ID {CourseId} not found", request.CourseId);
            throw new InvalidOperationException($"Course with ID {request.CourseId} not found");
        }

        // Update basic information
        if (!string.IsNullOrEmpty(request.Title))
            course.UpdateTitle(request.Title);

        if (!string.IsNullOrEmpty(request.Description))
            course.UpdateDescription(request.Description);

        if (!string.IsNullOrEmpty(request.Content))
            course.UpdateContent(request.Content);

        if (request.Category.HasValue)
            course.UpdateCategory(request.Category.Value);

        if (request.DifficultyLevel.HasValue)
            course.UpdateDifficultyLevel(request.DifficultyLevel.Value);

        if (!string.IsNullOrEmpty(request.Tags))
            course.UpdateTags(request.Tags);

        // Update pricing
        if (request.Price.HasValue)
        {
            var currency = !string.IsNullOrEmpty(request.Currency)
                ? Currency.Create(request.Currency)
                : course.Price.Currency;
            course.UpdatePrice(Money.Create(request.Price.Value, currency));
        }

        // Update duration and capacity
        if (request.DurationMinutes.HasValue)
            course.UpdateDuration(request.DurationMinutes.Value);

        if (request.MaxStudents.HasValue)
            course.UpdateMaxStudents(request.MaxStudents.Value);

        // Update instructor if provided
        if (request.InstructorId.HasValue)
        {
            var instructor = await _userRepository.GetByIdAsync(request.InstructorId.Value, cancellationToken);
            if (instructor == null)
            {
                _logger.LogWarning("Instructor with ID {InstructorId} not found", request.InstructorId.Value);
                throw new InvalidOperationException($"Instructor with ID {request.InstructorId.Value} not found");
            }
            course.UpdateInstructor(request.InstructorId.Value);
        }

        // Update publication settings
        if (request.IsPublished.HasValue)
        {
            if (request.IsPublished.Value)
                course.Publish(request.PublishedAt);
            else
                course.Unpublish();
        }

        // Update course materials if provided
        if (request.Materials != null)
        {
            // Clear existing materials and add new ones
            course.ClearMaterials();

            foreach (var materialDto in request.Materials.OrderBy(m => m.OrderIndex))
            {
                course.AddMaterial(
                    title: materialDto.Title,
                    description: materialDto.Description,
                    fileUrl: materialDto.FileUrl,
                    fileType: materialDto.FileType,
                    orderIndex: materialDto.OrderIndex
                );
            }
        }

        // Update in repository (SaveChanges will be handled by TransactionBehavior)
        await _courseRepository.UpdateAsync(course, cancellationToken);

        _logger.LogInformation("Course updated successfully with ID: {CourseId}", course.Id);

        return new UpdateCourseResponse
        {
            CourseId = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price.Amount,
            Currency = course.Price.Currency.Code,
            InstructorId = course.InstructorId,
            IsPublished = course.IsPublished,
            UpdatedAt = course.UpdatedAt
        };
    }
}