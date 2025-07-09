using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class AvailabilitySlot : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the availability slot.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the course this slot belongs to.
    /// </summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the instructor for this slot.
    /// </summary>
    public Guid InstructorId { get; private set; }

    /// <summary>
    /// Gets and sets privately the start time of the slot in UTC.
    /// </summary>
    public DateTimeOffset StartTimeUtc { get; private set; }

    /// <summary>
    /// Gets and sets privately the end time of the slot in UTC.
    /// </summary>
    public DateTimeOffset EndTimeUtc { get; private set; }

    /// <summary>
    /// Gets and sets privately whether this slot is currently booked.
    /// </summary>
    public bool IsBooked { get; private set; }

    /// <summary>
    /// Gets and sets privately the price for this slot.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets and sets privately the ISO currency code for the slot price (value object).
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    /// Gets and sets privately the maximum number of participants allowed.
    /// </summary>
    public int MaxParticipants { get; private set; }

    /// <summary>
    /// Gets and sets privately the current number of participants.
    /// </summary>
    public int CurrentParticipants { get; private set; }

    /// <summary>
    /// Gets and sets privately optional notes about this slot.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets and sets privately whether this slot is part of a recurring series.
    /// </summary>
    public bool IsRecurring { get; private set; }

    /// <summary>
    /// Gets and sets privately the recurring pattern configuration (JSON).
    /// </summary>
    public string? RecurringPattern { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the slot was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the slot was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the slot was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the course this availability slot belongs to.
    /// </summary>
    public Course Course { get; private set; }

    /// <summary>
    /// Gets and sets privately the instructor for this availability slot.
    /// </summary>
    public User Instructor { get; private set; }

    /// <summary>
    /// Gets and sets privately the bookings for this availability slot.
    /// </summary>
    public IReadOnlyCollection<Booking> Bookings { get; private set; } 
        = new List<Booking>();

    public AvailabilitySlot(
        Guid id,
        Guid courseId,
        Guid instructorId,
        DateTimeOffset startTimeUtc,
        DateTimeOffset endTimeUtc,
        decimal price,
        Currency currency,
        int maxParticipants,
        string? notes = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Slot ID is required.", nameof(id));
        if (courseId == Guid.Empty) throw new ArgumentException("Course ID is required.", nameof(courseId));
        if (instructorId == Guid.Empty) throw new ArgumentException("Instructor ID is required.", nameof(instructorId));
        if (endTimeUtc <= startTimeUtc) throw new ArgumentException("End time must be after start time.", nameof(endTimeUtc));
        if (price < 0) throw new ArgumentException("Price cannot be negative.", nameof(price));
        if (maxParticipants <= 0) throw new ArgumentException("Max participants must be positive.", nameof(maxParticipants));
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        Id = id;
        CourseId = courseId;
        InstructorId = instructorId;
        StartTimeUtc = startTimeUtc;
        EndTimeUtc = endTimeUtc;
        IsBooked = false;
        Price = price;
        MaxParticipants = maxParticipants;
        CurrentParticipants = 0;
        Notes = notes;
        IsRecurring = false;
        RecurringPattern = null;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    private AvailabilitySlot() { }
}