using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class CourseProgress : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the course progress record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the enrollment this progress belongs to.
    /// </summary>
    public Guid EnrollmentId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the course material this progress tracks.
    /// </summary>
    public Guid CourseMaterialId { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the course material has been completed.
    /// </summary>
    public bool IsCompleted { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course material was completed, if applicable.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the total time spent on this material in seconds.
    /// </summary>
    public int TimeSpentSeconds { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the progress record was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the progress record was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the progress record was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the enrollment this progress belongs to.
    /// </summary>
    public Enrollment Enrollment { get; private set; }

    /// <summary>
    /// Gets and sets privately the course material this progress tracks.
    /// </summary>
    public CourseMaterial CourseMaterial { get; private set; }

    public CourseProgress(
        Guid id,
        Guid enrollmentId,
        Guid courseMaterialId,
        int timeSpentSeconds = 0,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Progress ID is required.", nameof(id));
        if (enrollmentId == Guid.Empty) throw new ArgumentException("Enrollment ID is required.", nameof(enrollmentId));
        if (courseMaterialId == Guid.Empty) throw new ArgumentException("Course material ID is required.", nameof(courseMaterialId));
        if (timeSpentSeconds < 0) throw new ArgumentException("Time spent must be non-negative.", nameof(timeSpentSeconds));

        Id = id;
        EnrollmentId = enrollmentId;
        CourseMaterialId = courseMaterialId;
        IsCompleted = false;
        CompletedAt = null;
        TimeSpentSeconds = timeSpentSeconds;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
} 