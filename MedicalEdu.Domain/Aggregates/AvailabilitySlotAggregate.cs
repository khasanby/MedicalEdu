using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Events;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Aggregates;

/// <summary>
/// Represents the availability slot aggregate root that encapsulates slot lifecycle and capacity management operations.
/// </summary>
public sealed class AvailabilitySlotAggregate : IAggregateRoot<Guid>
{
    /// <summary>
    /// Holds domain events raised by this aggregate.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// The underlying AvailabilitySlot entity whose state is managed by this aggregate.
    /// </summary>
    private readonly AvailabilitySlot _slot;

    /// <summary>
    /// Initializes a new instance of the AvailabilitySlotAggregate with the given AvailabilitySlot entity.
    /// </summary>
    /// <param name="slot">The availability slot entity to encapsulate.</param>
    public AvailabilitySlotAggregate(AvailabilitySlot slot)
    {
        _slot = slot ?? throw new ArgumentNullException(nameof(slot));
    }

    /// <summary>
    /// Gets the unique identifier of the availability slot.
    /// </summary>
    public Guid Id => _slot.Id;

    /// <summary>
    /// Gets the unique identifier of the course this slot belongs to.
    /// </summary>
    public Guid CourseId => _slot.CourseId;

    /// <summary>
    /// Gets the unique identifier of the instructor for this slot.
    /// </summary>
    public Guid InstructorId => _slot.InstructorId;

    /// <summary>
    /// Gets the start time of the slot in UTC.
    /// </summary>
    public DateTimeOffset StartTimeUtc => _slot.StartTimeUtc;

    /// <summary>
    /// Gets the end time of the slot in UTC.
    /// </summary>
    public DateTimeOffset EndTimeUtc => _slot.EndTimeUtc;

    /// <summary>
    /// Gets whether this slot is currently booked.
    /// </summary>
    public bool IsBooked => _slot.IsBooked;

    /// <summary>
    /// Gets the price for this slot.
    /// </summary>
    public decimal Price => _slot.Price;

    /// <summary>
    /// Gets the ISO currency code for the slot price.
    /// </summary>
    public Currency Currency => _slot.Currency;

    /// <summary>
    /// Gets the maximum number of participants allowed.
    /// </summary>
    public int MaxParticipants => _slot.MaxParticipants;

    /// <summary>
    /// Gets the current number of participants.
    /// </summary>
    public int CurrentParticipants => _slot.CurrentParticipants;

    /// <summary>
    /// Gets optional notes about this slot.
    /// </summary>
    public string? Notes => _slot.Notes;

    /// <summary>
    /// Gets whether this slot is part of a recurring series.
    /// </summary>
    public bool IsRecurring => _slot.IsRecurring;

    /// <summary>
    /// Gets the recurring pattern configuration (JSON).
    /// </summary>
    public string? RecurringPattern => _slot.RecurringPattern;

    /// <summary>
    /// Gets the timestamp when the slot was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _slot.CreatedAt;

    /// <summary>
    /// Gets the timestamp when the slot was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt => _slot.UpdatedAt;

    /// <summary>
    /// Gets the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy => _slot.CreatedBy;

    /// <summary>
    /// Gets the timestamp when the slot was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified => _slot.LastModified;

    /// <summary>
    /// Gets the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy => _slot.LastModifiedBy;

    /// <summary>
    /// Gets the course this availability slot belongs to.
    /// </summary>
    public Course Course => _slot.Course;

    /// <summary>
    /// Gets the instructor for this availability slot.
    /// </summary>
    public User Instructor => _slot.Instructor;

    /// <summary>
    /// Gets the bookings for this availability slot.
    /// </summary>
    public IReadOnlyCollection<Booking> Bookings => _slot.Bookings;

    /// <summary>
    /// Gets the collection of domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events from the aggregate.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Books a participant for this slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="InvalidOperationException">Thrown when the slot is already booked or at maximum capacity.</exception>
    public void BookParticipant(string? modifiedBy = null)
    {
        _slot.MarkBooked(modifiedBy);
        _domainEvents.Add(new AvailabilitySlotBookedEvent(_slot.Id, _slot.CourseId, _slot.InstructorId));
    }

    /// <summary>
    /// Releases a booking for this slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="InvalidOperationException">Thrown when the slot is not booked or has no participants.</exception>
    public void ReleaseBooking(string? modifiedBy = null)
    {
        _slot.ReleaseBooking(modifiedBy);
        _domainEvents.Add(new AvailabilitySlotReleasedEvent(_slot.Id, _slot.CourseId, _slot.InstructorId));
    }

    /// <summary>
    /// Adds a participant to the slot (for group bookings) and raises the corresponding domain event.
    /// </summary>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="InvalidOperationException">Thrown when the slot is at maximum capacity.</exception>
    public void AddParticipant(string? modifiedBy = null)
    {
        _slot.AddParticipant(modifiedBy);
        _domainEvents.Add(new AvailabilitySlotParticipantAddedEvent(_slot.Id, _slot.CourseId, _slot.CurrentParticipants));
    }

    /// <summary>
    /// Removes a participant from the slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="InvalidOperationException">Thrown when there are no participants to remove.</exception>
    public void RemoveParticipant(string? modifiedBy = null)
    {
        _slot.RemoveParticipant(modifiedBy);
        _domainEvents.Add(new AvailabilitySlotParticipantRemovedEvent(_slot.Id, _slot.CourseId, _slot.CurrentParticipants));
    }

    /// <summary>
    /// Updates the price for this slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="newPrice">The new price for the slot.</param>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="ArgumentException">Thrown when the new price is negative.</exception>
    public void UpdatePrice(decimal newPrice, string? modifiedBy = null)
    {
        _slot.UpdatePrice(newPrice, modifiedBy);
        _domainEvents.Add(new AvailabilitySlotPriceUpdatedEvent(_slot.Id, _slot.CourseId, newPrice));
    }

    /// <summary>
    /// Sets this slot as part of a recurring series and raises the corresponding domain event.
    /// </summary>
    /// <param name="pattern">The recurring pattern configuration (JSON).</param>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="ArgumentException">Thrown when the pattern is empty.</exception>
    public void SetRecurring(string pattern, string? modifiedBy = null)
    {
        _slot.SetRecurring(pattern, modifiedBy);
        _domainEvents.Add(new AvailabilitySlotRecurringSetEvent(_slot.Id, _slot.CourseId, pattern));
    }

    /// <summary>
    /// Cancels the recurring pattern for this slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    public void CancelRecurring(string? modifiedBy = null)
    {
        _slot.CancelRecurring(modifiedBy);
        _domainEvents.Add(new AvailabilitySlotRecurringCancelledEvent(_slot.Id, _slot.CourseId));
    }

    /// <summary>
    /// Updates the notes for this slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="notes">The new notes for the slot.</param>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    public void UpdateNotes(string? notes, string? modifiedBy = null)
    {
        _slot.UpdateNotes(notes, modifiedBy);
        _domainEvents.Add(new AvailabilitySlotNotesUpdatedEvent(_slot.Id, _slot.CourseId));
    }

    /// <summary>
    /// Updates the time for this slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="newStartTime">The new start time for the slot.</param>
    /// <param name="newEndTime">The new end time for the slot.</param>
    /// <param name="modifiedBy">The identifier of the user making the modification, if tracked.</param>
    /// <exception cref="ArgumentException">Thrown when the end time is not after the start time.</exception>
    public void UpdateTime(DateTimeOffset newStartTime, DateTimeOffset newEndTime, string? modifiedBy = null)
    {
        _slot.UpdateTime(newStartTime, newEndTime, modifiedBy);
        _domainEvents.Add(new AvailabilitySlotTimeUpdatedEvent(_slot.Id, _slot.CourseId, newStartTime, newEndTime));
    }

    /// <summary>
    /// Gets whether the slot has available capacity for additional participants.
    /// </summary>
    public bool HasAvailableCapacity => CurrentParticipants < MaxParticipants;

    /// <summary>
    /// Gets the remaining capacity for this slot.
    /// </summary>
    public int RemainingCapacity => MaxParticipants - CurrentParticipants;

    /// <summary>
    /// Gets whether the slot is at full capacity.
    /// </summary>
    public bool IsAtFullCapacity => CurrentParticipants >= MaxParticipants;
}