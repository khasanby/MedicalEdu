using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Enrollment : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the enrollment.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the student.
    /// </summary>
    public Guid StudentId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the course.
    /// </summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the student enrolled in the course.
    /// </summary>
    public DateTimeOffset EnrolledAt { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the enrollment is currently active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets and sets privately the progress percentage (0-100) in the course.
    /// </summary>
    public int ProgressPercentage { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course was completed, if applicable.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the student last accessed the course.
    /// </summary>
    public DateTimeOffset? LastAccessedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the enrollment was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the enrollment was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the enrollment was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the student user for this enrollment.
    /// </summary>
    public User Student { get; private set; }

    /// <summary>
    /// Gets and sets privately the course for this enrollment.
    /// </summary>
    public Course Course { get; private set; }

    /// <summary>
    /// Gets and sets privately the course progress records for this enrollment.
    /// </summary>
    public IReadOnlyCollection<CourseProgress> CourseProgresses { get; private set; } = new List<CourseProgress>();

    public Enrollment(
        Guid id,
        Guid studentId,
        Guid courseId,
        DateTimeOffset? enrolledAt = null,
        bool isActive = true,
        int progressPercentage = 0,
        DateTimeOffset? completedAt = null,
        DateTimeOffset? lastAccessedAt = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Enrollment ID is required.", nameof(id));
        if (studentId == Guid.Empty) throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (courseId == Guid.Empty) throw new ArgumentException("Course ID is required.", nameof(courseId));
        if (progressPercentage < 0 || progressPercentage > 100) 
            throw new ArgumentException("Progress percentage must be between 0 and 100.", nameof(progressPercentage));

        Id = id;
        StudentId = studentId;
        CourseId = courseId;
        EnrolledAt = enrolledAt ?? DateTimeOffset.UtcNow;
        IsActive = isActive;
        ProgressPercentage = progressPercentage;
        CompletedAt = completedAt;
        LastAccessedAt = lastAccessedAt;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    private Enrollment() { }
}