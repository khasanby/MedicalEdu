using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Events;

public abstract class CourseEvent : IDomainEvent
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
    /// Gets the unique identifier for the course.
    /// </summary>
    public Guid CourseId { get; }

    protected CourseEvent(Guid courseId)
    {
        Id = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
        CourseId = courseId;
    }
}

public sealed class CourseCreatedEvent : CourseEvent
{
    /// <summary>
    /// Gets the unique identifier for the instructor.
    /// </summary>
    public Guid InstructorId { get; }

    /// <summary>
    /// Gets the title of the course.
    public string Title { get; }

    public CourseCreatedEvent(Guid courseId, Guid instructorId, string title) : base(courseId)
    {
        InstructorId = instructorId;
        Title = title;
    }
}

public sealed class CourseUpdatedEvent : CourseEvent
{
    /// <summary>
    /// Gets the title of the course.
    /// </summary>
    public string Title { get; }

    public CourseUpdatedEvent(Guid courseId, string title) : base(courseId)
    {
        Title = title;
    }
}

public sealed class CourseThumbnailUpdatedEvent : CourseEvent
{
    public CourseThumbnailUpdatedEvent(Guid courseId) : base(courseId)
    {
    }
}

public sealed class CoursePublishedEvent : CourseEvent
{
    /// <summary>
    /// Gets the title of the course.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the unique identifier for the instructor.
    /// </summary>
    public Guid InstructorId { get; }

    public CoursePublishedEvent(Guid courseId, string title, Guid instructorId) : base(courseId)
    {
        Title = title;
        InstructorId = instructorId;
    }
}

public sealed class CourseUnpublishedEvent : CourseEvent
{
    /// <summary>
    /// Gets the title of the course.
    /// </summary>
    public string Title { get; }

    public CourseUnpublishedEvent(Guid courseId, string title) : base(courseId)
    {
        Title = title;
    }
}

public sealed class CourseMaterialAddedEvent : CourseEvent
{
    /// <summary>
    /// Gets the unique identifier for the material.
    /// </summary>
    public Guid MaterialId { get; }

    /// <summary>
    /// Gets the title of the material.
    /// </summary>
    public string MaterialTitle { get; }

    public CourseMaterialAddedEvent(Guid courseId, Guid materialId, string materialTitle) : base(courseId)
    {
        MaterialId = materialId;
        MaterialTitle = materialTitle;
    }
}

public sealed class CourseMaterialRemovedEvent : CourseEvent
{
    /// <summary>
    /// Gets the unique identifier for the material.
    /// </summary>
    public Guid MaterialId { get; }

    public CourseMaterialRemovedEvent(Guid courseId, Guid materialId) : base(courseId)
    {
        MaterialId = materialId;
    }
}

public sealed class CourseMaterialsReorderedEvent : CourseEvent
{
    public CourseMaterialsReorderedEvent(Guid courseId) : base(courseId)
    {
    }
}

public sealed class CourseAvailabilitySlotAddedEvent : CourseEvent
{
    /// <summary>
    /// Gets the unique identifier for the slot.
    /// </summary>
    public Guid SlotId { get; }

    /// <summary>
    /// Gets the start time of the slot.
    /// </summary>
    public DateTime StartTime { get; }

    public CourseAvailabilitySlotAddedEvent(Guid courseId, Guid slotId, DateTime startTime) : base(courseId)
    {
        SlotId = slotId;
        StartTime = startTime;
    }
}

public sealed class CourseAvailabilitySlotRemovedEvent : CourseEvent
{
    /// <summary>
    /// Gets the unique identifier for the slot.
    /// </summary>
    public Guid SlotId { get; }

    public CourseAvailabilitySlotRemovedEvent(Guid courseId, Guid slotId) : base(courseId)
    {
        SlotId = slotId;
    }
} 