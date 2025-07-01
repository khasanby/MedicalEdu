using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Aggregates;

public sealed class BookingAggregate : IAggregateRoot<Guid>
{

    /// <summary>
    /// Gets the unique identifier for the student.
    /// </summary>
    public Guid StudentId { get; private set; }

    /// <summary>
    /// Gets the unique identifier for the course.
    /// </summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets the unique identifier for the availability slot.
    /// </summary>
    public Guid AvailabilitySlotId { get; private set; }

    /// <summary>
    /// Gets the status of the booking.
    /// </summary>
    public BookingStatus Status { get; private set; }

    /// <summary>
    /// Gets the amount of the booking.
    /// </summary>
    public Money Amount { get; private set; }

    /// <summary>
    /// Gets the date and time when the booking was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the notes for the booking.
    /// </summary>
    public string? Notes { get; private set; }

    /// <summary>
    /// Gets the reason for the cancellation.
    /// </summary>
    public string? CancellationReason { get; private set; }

    // Navigation properties
    /// <summary>
    /// Gets the student for the booking.
    /// </summary>
    public virtual UserAggregate Student { get; private set; }

    /// <summary>
    /// Gets the course for the booking.
    /// </summary>
    public virtual CourseAggregate Course { get; private set; }

    /// <summary>
    /// Gets the availability slot for the booking.
    /// </summary>
    public virtual Payment? Payment { get; private set; }

    // Domain events
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the domain events for the booking.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private BookingAggregate() { } // For EF Core

    public static BookingAggregate Create(
        Guid studentId, 
        Guid courseId, 
        Guid availabilitySlotId, 
        Money amount, 
        string? notes = null)
    {
        if (amount.IsZero)
            throw new ArgumentException("Booking amount cannot be zero", nameof(amount));

        var booking = new BookingAggregate
        {
            Id = Guid.NewGuid(),
            StudentId = studentId,
            CourseId = courseId,
            AvailabilitySlotId = availabilitySlotId,
            Status = BookingStatus.Pending,
            Amount = amount,
            Notes = notes
        };

        booking.AddDomainEvent(new BookingCreatedEvent(booking.Id, booking.StudentId, booking.CourseId, booking.Amount));
        return booking;
    }

    public void Confirm()
    {
        if (Status != BookingStatus.Pending)
            throw new InvalidOperationException($"Cannot confirm booking with status {Status}");

        Status = BookingStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookingConfirmedEvent(Id, StudentId, CourseId, AvailabilitySlotId));
    }

    public void Cancel(string reason)
    {
        if (Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled");

        if (Status == BookingStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed booking");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason is required", nameof(reason));

        Status = BookingStatus.Cancelled;
        CancellationReason = reason;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookingCancelledEvent(Id, StudentId, CourseId, reason));
    }

    public void Complete()
    {
        if (Status != BookingStatus.Confirmed)
            throw new InvalidOperationException($"Cannot complete booking with status {Status}");

        Status = BookingStatus.Completed;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookingCompletedEvent(Id, StudentId, CourseId));
    }

    public void UpdateNotes(string notes)
    {
        Notes = notes;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookingNotesUpdatedEvent(Id));
    }

    public void AssignPayment(Payment payment)
    {
        if (payment == null)
            throw new ArgumentNullException(nameof(payment));

        if (Payment != null)
            throw new InvalidOperationException("Booking already has a payment assigned");

        Payment = payment;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BookingPaymentAssignedEvent(Id, payment.Id));
    }

    public bool CanBeCancelled()
    {
        return Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
    }

    public bool CanBeConfirmed()
    {
        return Status == BookingStatus.Pending;
    }

    public bool CanBeCompleted()
    {
        return Status == BookingStatus.Confirmed;
    }

    public bool IsActive()
    {
        return Status == BookingStatus.Pending || Status == BookingStatus.Confirmed;
    }

    public bool IsInPast()
    {
        return AvailabilitySlot?.StartTimeUtc < DateTime.UtcNow;
    }

    public bool IsUpcoming()
    {
        return AvailabilitySlot?.StartTimeUtc > DateTime.UtcNow;
    }

    public TimeSpan GetTimeUntilSession()
    {
        if (AvailabilitySlot?.StartTimeUtc == null)
            return TimeSpan.Zero;

        return AvailabilitySlot.StartTimeUtc - DateTime.UtcNow;
    }

    public bool IsWithinCancellationWindow()
    {
        if (AvailabilitySlot?.StartTimeUtc == null)
            return false;

        // Allow cancellation up to 24 hours before the session
        var cancellationDeadline = AvailabilitySlot.StartTimeUtc.AddHours(-24);
        return DateTime.UtcNow < cancellationDeadline;
    }

    public Money GetRefundAmount()
    {
        if (Status != BookingStatus.Cancelled)
            return Money.Zero();

        // Full refund if cancelled within cancellation window
        if (IsWithinCancellationWindow())
            return Amount;

        // 50% refund if cancelled outside cancellation window
        return Amount.Multiply(0.5m);
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
} 