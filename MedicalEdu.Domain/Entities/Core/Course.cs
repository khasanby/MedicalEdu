using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Course : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the course.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the instructor for this course.
    /// </summary>
    public Guid InstructorId { get; private set; }

    /// <summary>
    /// Gets and sets privately the instructor user for this course.
    /// </summary>
    public User Instructor { get; private set; }

    /// <summary>
    /// Gets and sets privately the enrollments for this course.
    /// </summary>
    public IReadOnlyCollection<Enrollment> Enrollments { get; private set; } 
        = new List<Enrollment>();

    /// <summary>
    /// Gets and sets privately the course ratings for this course.
    /// </summary>
    public IReadOnlyCollection<CourseRating> CourseRatings { get; private set; } 
        = new List<CourseRating>();

    /// <summary>
    /// Gets and sets privately the course materials for this course.
    /// </summary>
    public IReadOnlyCollection<CourseMaterial> CourseMaterials { get; private set; } 
        = new List<CourseMaterial>();

    /// <summary>
    /// Gets and sets privately the availability slots for this course.
    /// </summary>
    public IReadOnlyCollection<AvailabilitySlot> AvailabilitySlots { get; private set; } 
        = new List<AvailabilitySlot>();

    /// <summary>
    /// Gets and sets privately the title of the course.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets and sets privately the description of the course.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets and sets privately the short description of the course.
    /// </summary>
    public string? ShortDescription { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the course is published.
    /// </summary>
    public bool IsPublished { get; private set; }

    /// <summary>
    /// Gets and sets privately the price of the course.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets and sets privately the ISO currency code for the course price (value object).
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    /// Gets and sets privately the URL of the course thumbnail image.
    /// </summary>
    public string? ThumbnailUrl { get; private set; }

    /// <summary>
    /// Gets and sets privately the URL of the course introduction video.
    /// </summary>
    public string? VideoIntroUrl { get; private set; }

    /// <summary>
    /// Gets and sets privately the duration of the course in minutes.
    /// </summary>
    public int? DurationMinutes { get; private set; }

    /// <summary>
    /// Gets and sets privately the maximum number of students allowed in the course.
    /// </summary>
    public int? MaxStudents { get; private set; }

    /// <summary>
    /// Gets and sets privately the category of the course.
    /// </summary>
    public string? Category { get; private set; }

    /// <summary>
    /// Gets and sets privately the tags associated with the course.
    /// </summary>
    public string? Tags { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course was published.
    /// </summary>
    public DateTimeOffset? PublishedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course was soft-deleted, if any.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets whether the course is active (not soft-deleted).
    /// </summary>
    public bool IsActive => DeletedAt == null;

    public Course(
        Guid id,
        Guid instructorId,
        string title,
        decimal price,
        Currency currency,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Course ID is required.", nameof(id));
        if (instructorId == Guid.Empty) throw new ArgumentException("Instructor ID is required.", nameof(instructorId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        Id = id;
        InstructorId = instructorId;
        Title = title;
        Price = price;
        CreatedAt = DateTimeOffset.UtcNow;
        IsPublished = false;
        CreatedBy = createdBy;
    }
}