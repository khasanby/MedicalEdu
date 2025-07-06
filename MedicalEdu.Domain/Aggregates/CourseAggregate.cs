using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.Events;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Aggregates;

public sealed class CourseAggregate : IAggregateRoot<Guid>
{
    /// <summary>
    /// Holds domain events raised by this entity.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// The underlying Course entity whose state is managed by this aggregate.
    /// </summary>
    private readonly Course _course;

    /// <summary>
    /// Initializes a new instance of the CourseAggregate with the given Course entity.
    /// </summary>
    public CourseAggregate(Course course)
    {
        _course = course ?? throw new ArgumentNullException(nameof(course));
    }

    #region Properties

    public Guid Id => _course.Id;
    public Guid InstructorId => _course.InstructorId;
    public string Title => _course.Title;
    public string? Description => _course.Description;
    public string? ShortDescription => _course.ShortDescription;
    public bool IsPublished => _course.IsPublished;
    public decimal Price => _course.Price;
    public Currency Currency => _course.Currency;
    public string? ThumbnailUrl => _course.ThumbnailUrl;
    public string? VideoIntroUrl => _course.VideoIntroUrl;
    public int? DurationMinutes => _course.DurationMinutes;
    public int? MaxStudents => _course.MaxStudents;
    public string? Category => _course.Category;
    public string? Tags => _course.Tags;
    public DateTimeOffset CreatedAt => _course.CreatedAt;
    public DateTimeOffset? UpdatedAt => _course.UpdatedAt;
    public DateTimeOffset? PublishedAt => _course.PublishedAt;
    public DateTimeOffset? DeletedAt => _course.DeletedAt;
    public string? CreatedBy => _course.CreatedBy;
    public DateTimeOffset? LastModified => _course.LastModified;
    public string? LastModifiedBy => _course.LastModifiedBy;
    public DifficultyLevel DifficultyLevel => _course.DifficultyLevel;

    // Navigation properties
    public User Instructor => _course.Instructor;
    public IReadOnlyCollection<Enrollment> Enrollments => _course.Enrollments;
    public IReadOnlyCollection<CourseRating> CourseRatings => _course.CourseRatings;
    public IReadOnlyCollection<CourseMaterial> CourseMaterials => _course.CourseMaterials;
    public IReadOnlyCollection<AvailabilitySlot> AvailabilitySlots => _course.AvailabilitySlots;

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new course aggregate.
    /// </summary>
    public static CourseAggregate Create(
        Guid instructorId,
        string title,
        Money price,
        DifficultyLevel difficultyLevel,
        string? createdBy = null)
    {
        var courseId = Guid.NewGuid();
        var course = new Course(
            courseId,
            instructorId,
            title,
            price.Amount,
            price.Currency,
            difficultyLevel,
            createdBy
        );
        var aggregate = new CourseAggregate(course);

        aggregate.AddDomainEvent(new CourseCreatedEvent(courseId, instructorId, title));

        return aggregate;
    }

    #endregion

    #region Course Management

    /// <summary>
    /// Updates the course title.
    /// </summary>
    public void UpdateTitle(string newTitle, string? modifiedBy = null)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title cannot be empty", nameof(newTitle));

        if (newTitle == _course.Title)
            return;

        _course.UpdateTitle(newTitle, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, newTitle));
    }

    /// <summary>
    /// Updates the course description.
    /// </summary>
    public void UpdateDescription(string? description, string? modifiedBy = null)
    {
        if (description == _course.Description)
            return;

        _course.UpdateDescription(description, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course short description.
    /// </summary>
    public void UpdateShortDescription(string? shortDescription, string? modifiedBy = null)
    {
        if (shortDescription == _course.ShortDescription)
            return;

        _course.UpdateShortDescription(shortDescription, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course price.
    /// </summary>
    public void UpdatePrice(Money newPrice, string? modifiedBy = null)
    {
        if (newPrice.Amount == _course.Price && newPrice.Currency.Equals(_course.Currency))
            return;

        _course.UpdatePrice(newPrice.Amount, modifiedBy);
        _course.UpdateCurrency(newPrice.Currency, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course thumbnail.
    /// </summary>
    public void UpdateThumbnail(Url thumbnailUrl, string? modifiedBy = null)
    {
        if (thumbnailUrl.Value == _course.ThumbnailUrl)
            return;

        _course.SetThumbnail(thumbnailUrl.Value);
        _course.UpdateLastModified(modifiedBy);
        AddDomainEvent(new CourseThumbnailUpdatedEvent(_course.Id));
    }

    /// <summary>
    /// Updates the course video intro URL.
    /// </summary>
    public void UpdateVideoIntro(Url videoIntroUrl, string? modifiedBy = null)
    {
        if (videoIntroUrl.Value == _course.VideoIntroUrl)
            return;

        _course.UpdateVideoIntroUrl(videoIntroUrl.Value, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course duration.
    /// </summary>
    public void UpdateDuration(int? durationMinutes, string? modifiedBy = null)
    {
        if (durationMinutes == _course.DurationMinutes)
            return;

        _course.UpdateDurationMinutes(durationMinutes, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the maximum number of students.
    /// </summary>
    public void UpdateMaxStudents(int? maxStudents, string? modifiedBy = null)
    {
        if (maxStudents == _course.MaxStudents)
            return;

        _course.UpdateMaxStudents(maxStudents, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course category.
    /// </summary>
    public void UpdateCategory(string? category, string? modifiedBy = null)
    {
        if (category == _course.Category)
            return;

        _course.UpdateCategory(category, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course tags.
    /// </summary>
    public void UpdateTags(string? tags, string? modifiedBy = null)
    {
        if (tags == _course.Tags)
            return;

        _course.UpdateTags(tags, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    /// <summary>
    /// Updates the course difficulty level.
    /// </summary>
    public void UpdateDifficultyLevel(DifficultyLevel newLevel, string? modifiedBy = null)
    {
        if (newLevel == _course.DifficultyLevel)
            return;
        _course.UpdateDifficultyLevel(newLevel, modifiedBy);
        AddDomainEvent(new CourseUpdatedEvent(_course.Id, _course.Title));
    }

    #endregion

    #region Publishing

    /// <summary>
    /// Publishes the course.
    /// </summary>
    public void Publish(string? publishedBy = null)
    {
        if (_course.IsPublished)
            throw new InvalidOperationException("Course is already published");

        _course.Publish();
        _course.UpdateLastModified(publishedBy);
        AddDomainEvent(new CoursePublishedEvent(_course.Id, _course.Title, _course.InstructorId));
    }

    /// <summary>
    /// Unpublishes the course.
    /// </summary>
    public void Unpublish(string? unpublishedBy = null)
    {
        if (!_course.IsPublished)
            throw new InvalidOperationException("Course is not published");

        _course.Unpublish();
        _course.UpdateLastModified(unpublishedBy);
        AddDomainEvent(new CourseUnpublishedEvent(_course.Id, _course.Title));
    }

    #endregion

    #region Course Materials

    /// <summary>
    /// Adds a course material to the course.
    /// </summary>
    public void AddCourseMaterial(CourseMaterial material)
    {
        if (material == null)
            throw new ArgumentNullException(nameof(material));

        if (_course.CourseMaterials.Any(m => m.Id == material.Id))
            throw new InvalidOperationException("Course material already exists");

        _course.AddCourseMaterial(material);
        AddDomainEvent(new CourseMaterialAddedEvent(_course.Id, material.Id, material.Title));
    }

    /// <summary>
    /// Removes a course material from the course.
    /// </summary>
    public void RemoveCourseMaterial(Guid materialId)
    {
        var material = _course.CourseMaterials.FirstOrDefault(m => m.Id == materialId);
        if (material == null)
            throw new InvalidOperationException("Course material not found");

        _course.RemoveCourseMaterial(materialId);
        AddDomainEvent(new CourseMaterialRemovedEvent(_course.Id, materialId));
    }

    /// <summary>
    /// Reorders course materials.
    /// </summary>
    public void ReorderCourseMaterials(IEnumerable<Guid> materialIds)
    {
        if (materialIds == null)
            throw new ArgumentNullException(nameof(materialIds));

        _course.ReorderCourseMaterials(materialIds);
        AddDomainEvent(new CourseMaterialsReorderedEvent(_course.Id));
    }

    #endregion

    #region Availability Slots

    /// <summary>
    /// Adds an availability slot to the course.
    /// </summary>
    public void AddAvailabilitySlot(AvailabilitySlot slot)
    {
        if (slot == null)
            throw new ArgumentNullException(nameof(slot));

        if (_course.AvailabilitySlots.Any(s => s.Id == slot.Id))
            throw new InvalidOperationException("Availability slot already exists");

        _course.AddAvailabilitySlot(slot);
        AddDomainEvent(new CourseAvailabilitySlotAddedEvent(_course.Id, slot.Id, slot.StartTimeUtc));
    }

    /// <summary>
    /// Removes an availability slot from the course.
    /// </summary>
    public void RemoveAvailabilitySlot(Guid slotId)
    {
        var slot = _course.AvailabilitySlots.FirstOrDefault(s => s.Id == slotId);
        if (slot == null)
            throw new InvalidOperationException("Availability slot not found");

        _course.RemoveAvailabilitySlot(slotId);
        AddDomainEvent(new CourseAvailabilitySlotRemovedEvent(_course.Id, slotId));
    }

    #endregion

    #region Business Logic

    /// <summary>
    /// Gets the average rating of the course.
    /// </summary>
    public decimal GetAverageRating()
    {
        if (!_course.CourseRatings.Any())
            return 0m;

        return (decimal)_course.CourseRatings.Average(r => r.Rating);
    }

    /// <summary>
    /// Gets the total number of enrollments.
    /// </summary>
    public int GetEnrollmentCount()
    {
        return _course.Enrollments.Count;
    }

    /// <summary>
    /// Checks if the course is full (reached maximum students).
    /// </summary>
    public bool IsFull()
    {
        if (_course.MaxStudents == null)
            return false;

        return _course.Enrollments.Count >= _course.MaxStudents.Value;
    }

    /// <summary>
    /// Checks if a student can enroll in the course.
    /// </summary>
    public bool CanEnroll(Guid studentId)
    {
        if (!_course.IsPublished)
            return false;

        if (_course.Enrollments.Any(e => e.StudentId == studentId))
            return false;

        if (IsFull())
            return false;

        return true;
    }

    /// <summary>
    /// Gets the course price as a Money value object.
    /// </summary>
    public Money GetPrice()
    {
        return Money.Create(_course.Price, _course.Currency);
    }

    #endregion

    #region Domain Events

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the aggregate.
    /// </summary>
    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears the domain events for this aggregate root.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    #endregion
}