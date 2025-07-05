using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class InstructorRating : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the instructor rating.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the instructor being rated.
    /// </summary>
    public Guid InstructorId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the student providing the rating.
    /// </summary>
    public Guid StudentId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the booking this rating is for.
    /// </summary>
    public Guid BookingId { get; private set; }

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
    /// Gets and sets privately the instructor user being rated.
    /// </summary>
    public User Instructor { get; private set; }

    /// <summary>
    /// Gets and sets privately the student user who provided this rating.
    /// </summary>
    public User Student { get; private set; }

    /// <summary>
    /// Gets and sets privately the booking this rating is for.
    /// </summary>
    public Booking Booking { get; private set; }

    public InstructorRating(
        Guid id,
        Guid instructorId,
        Guid studentId,
        Guid bookingId,
        int rating,
        string? review = null,
        bool isPublic = true,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Rating ID is required.", nameof(id));
        if (instructorId == Guid.Empty) throw new ArgumentException("Instructor ID is required.", nameof(instructorId));
        if (studentId == Guid.Empty) throw new ArgumentException("Student ID is required.", nameof(studentId));
        if (bookingId == Guid.Empty) throw new ArgumentException("Booking ID is required.", nameof(bookingId));
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5.", nameof(rating));

        Id = id;
        InstructorId = instructorId;
        StudentId = studentId;
        BookingId = bookingId;
        Rating = rating;
        Review = review;
        IsPublic = isPublic;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
} 