using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Aggregates;

public sealed class CourseAggregate : IAggregateRoot<Guid>
{
    public Guid InstructorId { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public bool IsPublished { get; private set; }
    public Money? Price { get; private set; }
    public string? ThumbnailUrl { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation properties
    public virtual UserAggregate Instructor { get; private set; }
    public virtual ICollection<CourseMaterial> Materials { get; private set; } = new List<CourseMaterial>();
    public virtual ICollection<AvailabilitySlot> AvailabilitySlots { get; private set; } = new List<AvailabilitySlot>();
    public virtual ICollection<Booking> Bookings { get; private set; } = new List<Booking>();
    public virtual ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

    // Domain events
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private CourseAggregate() { } // For EF Core

    public static CourseAggregate Create(Guid instructorId, string title, string? description, Money? price)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));

        if (description?.Length > 1000)
            throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

        var course = new CourseAggregate
        {
            Id = Guid.NewGuid(),
            InstructorId = instructorId,
            Title = title,
            Description = description,
            Price = price,
            IsPublished = false
        };

        course.AddDomainEvent(new CourseCreatedEvent(course.Id, course.InstructorId, course.Title));
        return course;
    }

    public void UpdateDetails(string title, string? description, Money? price)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));

        if (description?.Length > 1000)
            throw new ArgumentException("Description cannot exceed 1000 characters", nameof(description));

        Title = title;
        Description = description;
        Price = price;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new CourseUpdatedEvent(Id, Title));
    }

    public void UpdateThumbnail(string thumbnailUrl)
    {
        ThumbnailUrl = thumbnailUrl;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new CourseThumbnailUpdatedEvent(Id));
    }

    public void Publish()
    {
        if (IsPublished)
            throw new InvalidOperationException("Course is already published");

        if (Materials.Count == 0)
            throw new InvalidOperationException("Cannot publish course without materials");

        IsPublished = true;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new CoursePublishedEvent(Id, Title, InstructorId));
    }

    public void Unpublish()
    {
        if (!IsPublished)
            throw new InvalidOperationException("Course is not published");

        IsPublished = false;
        UpdatedAt = DateTime.UtcNow;
        LastModified = DateTime.UtcNow;

        AddDomainEvent(new CourseUnpublishedEvent(Id, Title));
    }

    public void AddMaterial(CourseMaterial material)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material));

        if (Materials.Any(m => m.Id == material.Id))
            throw new InvalidOperationException("Material already exists in course");

        Materials.Add(material);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseMaterialAddedEvent(Id, material.Id, material.Title));
    }

    public void RemoveMaterial(Guid materialId)
    {
        var material = Materials.FirstOrDefault(m => m.Id == materialId);
        if (material == null)
            throw new InvalidOperationException("Material not found in course");

        Materials.Remove(material);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseMaterialRemovedEvent(Id, materialId));
    }

    public void ReorderMaterials(List<Guid> materialIds)
    {
        if (materialIds == null || materialIds.Count != Materials.Count)
            throw new ArgumentException("Must provide all material IDs in the correct order");

        var materialDict = Materials.ToDictionary(m => m.Id);
        var orderedMaterials = new List<CourseMaterial>();

        foreach (var materialId in materialIds)
        {
            if (!materialDict.TryGetValue(materialId, out var material))
                throw new ArgumentException($"Material with ID {materialId} not found in course");

            orderedMaterials.Add(material);
        }

        // Update order
        for (int i = 0; i < orderedMaterials.Count; i++)
        {
            orderedMaterials[i].UpdateOrder(i + 1);
        }

        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseMaterialsReorderedEvent(Id));
    }

    public void AddAvailabilitySlot(AvailabilitySlot slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (AvailabilitySlots.Any(s => s.Id == slot.Id))
            throw new InvalidOperationException("Availability slot already exists");

        AvailabilitySlots.Add(slot);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseAvailabilitySlotAddedEvent(Id, slot.Id, slot.StartTimeUtc));
    }

    public void RemoveAvailabilitySlot(Guid slotId)
    {
        var slot = AvailabilitySlots.FirstOrDefault(s => s.Id == slotId);
        if (slot == null)
            throw new InvalidOperationException("Availability slot not found");

        if (slot.IsBooked)
            throw new InvalidOperationException("Cannot remove booked availability slot");

        AvailabilitySlots.Remove(slot);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseAvailabilitySlotRemovedEvent(Id, slotId));
    }

    public bool CanBeAccessedBy(Guid userId, UserRole userRole)
    {
        // Admin can access all courses
        if (userRole == UserRole.Admin)
            return true;

        // Instructor can access their own courses
        if (userRole == UserRole.Instructor && InstructorId == userId)
            return true;

        // Student can access published courses they're enrolled in
        if (userRole == UserRole.Student && IsPublished)
            return Enrollments.Any(e => e.StudentId == userId && e.IsActive);

        return false;
    }

    public bool CanBeBookedBy(Guid userId, UserRole userRole)
    {
        return IsPublished && 
               userRole == UserRole.Student && 
               AvailabilitySlots.Any(s => !s.IsBooked && s.StartTimeUtc > DateTime.UtcNow);
    }

    public int GetEnrollmentCount()
    {
        return Enrollments.Count(e => e.IsActive);
    }

    public decimal GetTotalRevenue()
    {
        return Bookings
            .Where(b => b.Status == BookingStatus.Confirmed)
            .Sum(b => b.Amount);
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