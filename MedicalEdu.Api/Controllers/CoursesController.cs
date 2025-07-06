using Microsoft.AspNetCore.Mvc;
using MedicalEdu.Domain.Aggregates;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Api.Controllers;

/// <summary>
/// Controller for course-related operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseRepository _courseRepository;

    public CoursesController(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
    }

    /// <summary>
    /// Gets a course by its unique identifier.
    /// </summary>
    /// <param name="id">The course's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The course if found; otherwise, 404 Not Found.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Course>> GetCourse(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        
        if (course == null)
        {
            return NotFound();
        }

        return Ok(course);
    }

    /// <summary>
    /// Gets all courses with optional filtering.
    /// </summary>
    /// <param name="isPublished">Optional filter for published courses only.</param>
    /// <param name="isActive">Optional filter for active courses only.</param>
    /// <param name="instructorId">Optional filter by instructor.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of courses.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Course>>> GetCourses(
        [FromQuery] bool? isPublished = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? instructorId = null,
        CancellationToken cancellationToken = default)
    {
        var courses = await _courseRepository.GetAllAsync(isPublished, isActive, instructorId, cancellationToken);
        return Ok(courses);
    }

    /// <summary>
    /// Creates a new course.
    /// </summary>
    /// <param name="request">The course creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created course.</returns>
    [HttpPost]
    public async Task<ActionResult<Course>> CreateCourse([FromBody] CreateCourseRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Create value objects
            var price = Money.Create(request.Price, Currency.Create(request.CurrencyCode));
            var thumbnailUrl = !string.IsNullOrEmpty(request.ThumbnailUrl) ? Url.Create(request.ThumbnailUrl) : null;

            // Create course entity
            var course = new Course(
                id: Guid.NewGuid(),
                title: request.Title,
                description: request.Description,
                shortDescription: request.ShortDescription,
                instructorId: request.InstructorId,
                price: price,
                category: request.Category,
                difficultyLevel: request.DifficultyLevel,
                duration: request.Duration,
                maxStudents: request.MaxStudents,
                thumbnailUrl: thumbnailUrl);

            // Add to repository
            await _courseRepository.AddAsync(course, cancellationToken);
            await _courseRepository.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, course);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing course.
    /// </summary>
    /// <param name="id">The course's unique identifier.</param>
    /// <param name="request">The course update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseRequest request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        
        if (course == null)
        {
            return NotFound();
        }

        try
        {
            // Update course properties
            course.UpdateTitle(request.Title);
            course.UpdateDescription(request.Description);
            course.UpdateShortDescription(request.ShortDescription);
            course.UpdateCategory(request.Category);
            course.UpdateDifficultyLevel(request.DifficultyLevel);
            course.UpdateDuration(request.Duration);
            course.UpdateMaxStudents(request.MaxStudents);

            if (!string.IsNullOrEmpty(request.ThumbnailUrl))
            {
                course.UpdateThumbnailUrl(Url.Create(request.ThumbnailUrl));
            }

            await _courseRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Publishes a course.
    /// </summary>
    /// <param name="id">The course's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/publish")]
    public async Task<ActionResult> PublishCourse(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        
        if (course == null)
        {
            return NotFound();
        }

        try
        {
            course.Publish();
            await _courseRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Unpublishes a course.
    /// </summary>
    /// <param name="id">The course's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/unpublish")]
    public async Task<ActionResult> UnpublishCourse(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        
        if (course == null)
        {
            return NotFound();
        }

        try
        {
            course.Unpublish();
            await _courseRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Activates a course.
    /// </summary>
    /// <param name="id">The course's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/activate")]
    public async Task<ActionResult> ActivateCourse(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        
        if (course == null)
        {
            return NotFound();
        }

        try
        {
            course.Activate();
            await _courseRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Deactivates a course.
    /// </summary>
    /// <param name="id">The course's unique identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>200 OK if successful; otherwise, 400 Bad Request.</returns>
    [HttpPost("{id}/deactivate")]
    public async Task<ActionResult> DeactivateCourse(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(id, cancellationToken);
        
        if (course == null)
        {
            return NotFound();
        }

        try
        {
            course.Deactivate();
            await _courseRepository.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

/// <summary>
/// Request model for creating a new course.
/// </summary>
public class CreateCourseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public Guid InstructorId { get; set; }
    public decimal Price { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public CourseCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public TimeSpan Duration { get; set; }
    public int MaxStudents { get; set; }
    public string? ThumbnailUrl { get; set; }
}

/// <summary>
/// Request model for updating a course.
/// </summary>
public class UpdateCourseRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public CourseCategory Category { get; set; }
    public DifficultyLevel DifficultyLevel { get; set; }
    public TimeSpan Duration { get; set; }
    public int MaxStudents { get; set; }
    public string? ThumbnailUrl { get; set; }
} 