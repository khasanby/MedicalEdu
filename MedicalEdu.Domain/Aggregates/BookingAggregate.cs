using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.Events;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Aggregates;

/// <summary>
/// Represents the booking aggregate root that encapsulates booking lifecycle and management operations.
/// </summary>
public sealed class BookingAggregate : IAggregateRoot<Guid>
{
    /// <summary>
    /// Holds domain events raised by this aggregate.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// The underlying Booking entity whose state is managed by this aggregate.
    /// </summary>
    private readonly Booking _booking;

    /// <summary>
    /// Initializes a new instance of the BookingAggregate with the given Booking entity.
    /// </summary>
    /// <param name="booking">The booking entity to encapsulate.</param>
    public BookingAggregate(Booking booking)
    {
        _booking = booking ?? throw new ArgumentNullException(nameof(booking));
    }

    /// <summary>
    /// Gets the unique identifier of the booking.
    /// </summary>
    public Guid Id => _booking.Id;

    /// <summary>
    /// Gets the unique identifier of the student who made the booking.
    /// </summary>
    public Guid StudentId => _booking.StudentId;

    /// <summary>
    /// Gets the student user who made this booking.
    /// </summary>
    public User Student => _booking.Student;

    /// <summary>
    /// Gets the unique identifier of the reserved availability slot.
    /// </summary>
    public Guid AvailabilitySlotId => _booking.AvailabilitySlotId;

    /// <summary>
    /// Gets the current status of the booking (pending, confirmed, cancelled, etc.).
    /// </summary>
    public BookingStatus Status => _booking.Status;

    /// <summary>
    /// Gets the total amount for the booking.
    /// </summary>
    public decimal Amount => _booking.Amount;

    /// <summary>
    /// Gets the ISO currency code for the booking amount.
    /// </summary>
    public Currency Currency => _booking.Currency;

    /// <summary>
    /// Gets optional notes provided by the student.
    /// </summary>
    public string? StudentNotes => _booking.StudentNotes;

    /// <summary>
    /// Gets optional notes provided by the instructor.
    /// </summary>
    public string? InstructorNotes => _booking.InstructorNotes;

    /// <summary>
    /// Gets the timestamp when the booking was created.
    /// </summary>
    public DateTimeOffset CreatedAt => _booking.CreatedAt;

    /// <summary>
    /// Gets the timestamp when the booking was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt => _booking.UpdatedAt;

    /// <summary>
    /// Gets the timestamp when the booking was confirmed, if applicable.
    /// </summary>
    public DateTimeOffset? ConfirmedAt => _booking.ConfirmedAt;

    /// <summary>
    /// Gets the timestamp when the booking was cancelled, if applicable.
    /// </summary>
    public DateTimeOffset? CancelledAt => _booking.CancelledAt;

    /// <summary>
    /// Gets the reason for cancellation, if the booking was cancelled.
    /// </summary>
    public string? CancellationReason => _booking.CancellationReason;

    /// <summary>
    /// Gets the ID of the original booking if this booking is a reschedule.
    /// </summary>
    public Guid? RescheduledFromBookingId => _booking.RescheduledFromBookingId;

    /// <summary>
    /// Gets the meeting URL for virtual sessions, if applicable.
    /// </summary>
    public string? MeetingUrl => _booking.MeetingUrl;

    /// <summary>
    /// Gets notes about the meeting, if applicable.
    /// </summary>
    public string? MeetingNotes => _booking.MeetingNotes;

    /// <summary>
    /// Gets the identifier of the user who created this booking record, if tracked.
    /// </summary>
    public string? CreatedBy => _booking.CreatedBy;

    /// <summary>
    /// Gets the timestamp when the booking was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified => _booking.LastModified;

    /// <summary>
    /// Gets the identifier of the user who last modified this booking record, if tracked.
    /// </summary>
    public string? LastModifiedBy => _booking.LastModifiedBy;

    /// <summary>
    /// Gets the promo codes applied to this booking.
    /// </summary>
    public IReadOnlyCollection<BookingPromoCode> BookingPromoCodes => _booking.BookingPromoCodes;

    /// <summary>
    /// Gets the payments for this booking.
    /// </summary>
    public IReadOnlyCollection<Payment> Payments => _booking.Payments;

    /// <summary>
    /// Gets the instructor ratings for this booking.
    /// </summary>
    public IReadOnlyCollection<InstructorRating> InstructorRatings => _booking.InstructorRatings;

    /// <summary>
    /// Gets the collection of domain events raised by this aggregate.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events from the aggregate.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Confirms the booking and raises the corresponding domain event.
    /// </summary>
    /// <param name="courseId">The ID of the course associated with this booking.</param>
    /// <exception cref="InvalidOperationException">Thrown when the booking cannot be confirmed.</exception>
    public void Confirm(Guid courseId)
    {
        _booking.Confirm();
        _domainEvents.Add(new BookingConfirmedEvent(_booking.Id, _booking.StudentId, courseId, _booking.AvailabilitySlotId));
    }

    /// <summary>
    /// Cancels the booking with the specified reason and raises the corresponding domain event.
    /// </summary>
    /// <param name="reason">The reason for cancellation.</param>
    /// <param name="courseId">The ID of the course associated with this booking.</param>
    /// <exception cref="ArgumentException">Thrown when reason is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the booking cannot be cancelled.</exception>
    public void Cancel(string reason, Guid courseId)
    {
        _booking.Cancel(reason);
        _domainEvents.Add(new BookingCancelledEvent(_booking.Id, _booking.StudentId, courseId, reason));
    }

    /// <summary>
    /// Marks the booking as completed and raises the corresponding domain event.
    /// </summary>
    /// <param name="courseId">The ID of the course associated with this booking.</param>
    /// <exception cref="InvalidOperationException">Thrown when the booking cannot be completed.</exception>
    public void Complete(Guid courseId)
    {
        _booking.Complete();
        _domainEvents.Add(new BookingCompletedEvent(_booking.Id, _booking.StudentId, courseId));
    }

    /// <summary>
    /// Reschedules the booking to a new slot and raises the corresponding domain event.
    /// </summary>
    /// <param name="newSlotId">The ID of the new availability slot.</param>
    /// <param name="courseId">The ID of the course associated with this booking.</param>
    /// <exception cref="ArgumentException">Thrown when new slot ID is invalid.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the booking cannot be rescheduled.</exception>
    public void Reschedule(Guid newSlotId, Guid courseId)
    {
        _booking.Reschedule(newSlotId);
        // Optionally, you could define a BookingRescheduledEvent if needed
        _domainEvents.Add(new BookingConfirmedEvent(_booking.Id, _booking.StudentId, courseId, newSlotId));
    }

    /// <summary>
    /// Updates the student notes and raises the corresponding domain event.
    /// </summary>
    /// <param name="notes">The new student notes.</param>
    public void UpdateStudentNotes(string notes)
    {
        _booking.SetStudentNotes(notes);
        _domainEvents.Add(new BookingNotesUpdatedEvent(_booking.Id));
    }

    /// <summary>
    /// Updates the instructor notes and raises the corresponding domain event.
    /// </summary>
    /// <param name="notes">The new instructor notes.</param>
    public void UpdateInstructorNotes(string notes)
    {
        _booking.SetInstructorNotes(notes);
        _domainEvents.Add(new BookingNotesUpdatedEvent(_booking.Id));
    }

    /// <summary>
    /// Assigns a payment to this booking and raises the corresponding domain event.
    /// </summary>
    /// <param name="paymentId">The ID of the payment to assign.</param>
    public void AssignPayment(Guid paymentId)
    {
        // You may want to add logic to the Booking entity to actually add the payment to the Payments collection
        _domainEvents.Add(new BookingPaymentAssignedEvent(_booking.Id, paymentId));
    }
}