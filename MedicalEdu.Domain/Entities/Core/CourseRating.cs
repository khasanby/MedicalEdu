using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class CourseRating : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the course rating.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the course being rated.
    /// </summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the student who provided the rating.
    /// </summary>
    public Guid StudentId { get; private set; }

    /// <summary>
    /// Gets and sets privately the rating value (1-5 stars).
    /// </summary>
    public int Rating { get; private set; }

    /// <summary>
    /// Gets and sets privately the written review text, if provided.
    /// </summary>
    public string? Review { get; private set; }

    /// <summary>
    /// Gets and sets privately whether this rating is publicly visible.
    /// </summary>
    public bool IsPublic { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the rating was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the rating was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the rating was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the course being rated.
    /// </summary>
    public Course Course { get; private set; }

    /// <summary>
    /// Gets and sets privately the student user who provided this rating.
    /// </summary>
    public User Student { get; private set; }

    public CourseRating(
        Guid id,
        Guid courseId,
        Guid studentId,
        int rating,
        string? review = null,
        bool isPublic = true,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Rating ID is required.", nameof(id));
        if (courseId == Guid.Empty) throw new ArgumentException("Course ID is required.", nameof(courseId));
        if (studentId == Guid.Empty) throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

        Id = id;
        CourseId = courseId;
        StudentId = studentId;
        Rating = rating;
        Review = review;
        IsPublic = isPublic;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    private CourseRating() { }
} 