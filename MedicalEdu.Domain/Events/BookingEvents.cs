using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Events;

public abstract class BookingEvent : IDomainEvent
{
    /// <summary>
    /// Gets the unique identifier for the domain event.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the date and time when the domain event occurred.
    /// </summary>
    public DateTime OccurredOn { get; }

    /// <summary>
    /// Gets the unique identifier for the booking.
    /// </summary>
    public Guid BookingId { get; }

    protected BookingEvent(Guid bookingId)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        BookingId = bookingId;
    }
}

public sealed class BookingCreatedEvent : BookingEvent
{
    /// <summary>
    /// Gets the unique identifier for the student.
    /// </summary>
    public Guid StudentId { get; }

    /// <summary>
    /// Gets the unique identifier for the course.
    /// </summary>
    public Guid CourseId { get; }

    /// <summary>
    /// Gets the amount of the booking.
    /// </summary>
    public Money Amount { get; }

    public BookingCreatedEvent(Guid bookingId, Guid studentId, Guid courseId, Money amount) : base(bookingId)
    {
        StudentId = studentId;
        CourseId = courseId;
        Amount = amount;
    }
}

public sealed class BookingConfirmedEvent : BookingEvent
{
    /// <summary>
    /// Gets the unique identifier for the student.
    /// </summary>
    public Guid StudentId { get; }

    /// <summary>
    /// Gets the unique identifier for the course.
    /// </summary>
    public Guid CourseId { get; }

    /// <summary>
    /// Gets the unique identifier for the availability slot.
    /// </summary>
    public Guid AvailabilitySlotId { get; }

    public BookingConfirmedEvent(Guid bookingId, Guid studentId, Guid courseId, Guid availabilitySlotId) : base(bookingId)
    {
        StudentId = studentId;
        CourseId = courseId;
        AvailabilitySlotId = availabilitySlotId;
    }
}

public sealed class BookingCancelledEvent : BookingEvent
{
    /// <summary>
    /// Gets the unique identifier for the student.
    /// </summary>
    public Guid StudentId { get; }

    /// <summary>
    /// Gets the unique identifier for the course.
    /// </summary>
    public Guid CourseId { get; }

    /// <summary>
    /// Gets the reason for the cancellation.
    /// </summary>
    public string Reason { get; }

    public BookingCancelledEvent(Guid bookingId, Guid studentId, Guid courseId, string reason) : base(bookingId)
    {
        StudentId = studentId;
        CourseId = courseId;
        Reason = reason;
    }
}

public sealed class BookingCompletedEvent : BookingEvent
{
    /// <summary>
    /// Gets the unique identifier for the student.
    /// </summary>
    public Guid StudentId { get; }

    /// <summary>
    /// Gets the unique identifier for the course.
    /// </summary>
    public Guid CourseId { get; }

    public BookingCompletedEvent(Guid bookingId, Guid studentId, Guid courseId) : base(bookingId)
    {
        StudentId = studentId;
        CourseId = courseId;
    }
}

public sealed class BookingNotesUpdatedEvent : BookingEvent
{
    public BookingNotesUpdatedEvent(Guid bookingId) : base(bookingId)
    {
    }
}

public sealed class BookingPaymentAssignedEvent : BookingEvent
{
    /// <summary>
    /// Gets the unique identifier for the payment.
    /// </summary>
    public Guid PaymentId { get; }

    public BookingPaymentAssignedEvent(Guid bookingId, Guid paymentId) : base(bookingId)
    {
        PaymentId = paymentId;
    }
}