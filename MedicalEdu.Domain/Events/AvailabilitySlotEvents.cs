using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Events;

public abstract class AvailabilitySlotEvent : IDomainEvent
{
    public Guid Id { get; }
    public DateTime OccurredOn { get; }
    public Guid SlotId { get; }

    protected AvailabilitySlotEvent(Guid slotId)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        SlotId = slotId;
    }
}

public class AvailabilitySlotBookedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public Guid InstructorId { get; }

    public AvailabilitySlotBookedEvent(Guid slotId, Guid courseId, Guid instructorId) : base(slotId)
    {
        CourseId = courseId;
        InstructorId = instructorId;
    }
}

public class AvailabilitySlotReleasedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public Guid InstructorId { get; }

    public AvailabilitySlotReleasedEvent(Guid slotId, Guid courseId, Guid instructorId) : base(slotId)
    {
        CourseId = courseId;
        InstructorId = instructorId;
    }
}

public class AvailabilitySlotParticipantAddedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public int CurrentParticipants { get; }

    public AvailabilitySlotParticipantAddedEvent(Guid slotId, Guid courseId, int currentParticipants) : base(slotId)
    {
        CourseId = courseId;
        CurrentParticipants = currentParticipants;
    }
}

public class AvailabilitySlotParticipantRemovedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public int CurrentParticipants { get; }

    public AvailabilitySlotParticipantRemovedEvent(Guid slotId, Guid courseId, int currentParticipants) : base(slotId)
    {
        CourseId = courseId;
        CurrentParticipants = currentParticipants;
    }
}

public class AvailabilitySlotPriceUpdatedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public decimal NewPrice { get; }

    public AvailabilitySlotPriceUpdatedEvent(Guid slotId, Guid courseId, decimal newPrice) : base(slotId)
    {
        CourseId = courseId;
        NewPrice = newPrice;
    }
}

public class AvailabilitySlotRecurringSetEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public string Pattern { get; }

    public AvailabilitySlotRecurringSetEvent(Guid slotId, Guid courseId, string pattern) : base(slotId)
    {
        CourseId = courseId;
        Pattern = pattern;
    }
}

public class AvailabilitySlotRecurringCancelledEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }

    public AvailabilitySlotRecurringCancelledEvent(Guid slotId, Guid courseId) : base(slotId)
    {
        CourseId = courseId;
    }
}

public class AvailabilitySlotNotesUpdatedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }

    public AvailabilitySlotNotesUpdatedEvent(Guid slotId, Guid courseId) : base(slotId)
    {
        CourseId = courseId;
    }
}

public class AvailabilitySlotTimeUpdatedEvent : AvailabilitySlotEvent
{
    public Guid CourseId { get; }
    public DateTimeOffset NewStartTime { get; }
    public DateTimeOffset NewEndTime { get; }

    public AvailabilitySlotTimeUpdatedEvent(Guid slotId, Guid courseId, DateTimeOffset newStartTime, DateTimeOffset newEndTime) : base(slotId)
    {
        CourseId = courseId;
        NewStartTime = newStartTime;
        NewEndTime = newEndTime;
    }
} 