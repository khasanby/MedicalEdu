using MediatR;
using MedicalEdu.Application.Common.Results;
using MedicalEdu.Domain.Aggregates;
using MedicalEdu.Domain.DataAccess;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace MedicalEdu.Application.Courses.Create;

public sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CreateCourseResponse>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateCourseCommandHandler> _logger;

    public CreateCourseCommandHandler(
        ICourseRepository courseRepository,
        IUserRepository userRepository,
        ILogger<CreateCourseCommandHandler> logger)
    {
        _courseRepository = courseRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<CreateCourseResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating course with title: {Title}", request.Title);

        // Validate instructor exists
        var instructor = await _userRepository.GetByIdAsync(request.InstructorId, cancellationToken);
        if (instructor == null)
        {
            _logger.LogWarning("Instructor with ID {InstructorId} not found", request.InstructorId);
            throw new InvalidOperationException($"Instructor with ID {request.InstructorId} not found");
        }

        // Create course aggregate
        var course = CourseAggregate.Create(
            title: request.Title,
            description: request.Description,
            content: request.Content,
            category: request.Category,
            difficultyLevel: request.DifficultyLevel,
            tags: request.Tags,
            price: Money.Create(request.Price, Currency.Create(request.Currency)),
            durationMinutes: request.DurationMinutes,
            maxStudents: request.MaxStudents,
            instructorId: request.InstructorId,
            isPublished: request.IsPublished,
            publishedAt: request.PublishedAt
        );

        // Add course materials if provided
        if (request.Materials != null)
        {
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

        // Add to repository (SaveChanges will be handled by TransactionBehavior)
        await _courseRepository.AddAsync(course, cancellationToken);

        _logger.LogInformation("Course created successfully with ID: {CourseId}", course.Id);

        return new CreateCourseResponse
        {
            CourseId = course.Id,
            Title = course.Title,
            Description = course.Description,
            Price = course.Price.Amount,
            Currency = course.Price.Currency.Code,
            InstructorId = course.InstructorId,
            IsPublished = course.IsPublished,
            CreatedAt = course.CreatedAt
        };
    }
}