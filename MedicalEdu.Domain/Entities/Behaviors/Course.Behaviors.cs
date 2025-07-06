using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Course
{
    internal void Publish()
    {
        if (IsPublished) throw new InvalidOperationException("Already published.");
        IsPublished = true;
        PublishedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void Unpublish()
    {
        if (!IsPublished) throw new InvalidOperationException("Not published.");
        IsPublished = false;
        PublishedAt = null;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0) throw new ArgumentException("Price cannot be negative.", nameof(newPrice));
        Price = newPrice;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdatePrice(decimal newPrice, string? modifiedBy = null)
    {
        if (newPrice < 0) throw new ArgumentException("Price cannot be negative.", nameof(newPrice));
        Price = newPrice;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void SetThumbnail(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Thumbnail URL is required.", nameof(url));
        ThumbnailUrl = url;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdateTitle(string newTitle, string? modifiedBy = null)
    {
        if (string.IsNullOrWhiteSpace(newTitle)) throw new ArgumentException("Title cannot be empty.", nameof(newTitle));
        Title = newTitle;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateDescription(string? description, string? modifiedBy = null)
    {
        Description = description;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateShortDescription(string? shortDescription, string? modifiedBy = null)
    {
        ShortDescription = shortDescription;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateCurrency(Currency currency, string? modifiedBy = null)
    {
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateVideoIntroUrl(string? videoIntroUrl, string? modifiedBy = null)
    {
        VideoIntroUrl = videoIntroUrl;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateDurationMinutes(int? durationMinutes, string? modifiedBy = null)
    {
        if (durationMinutes.HasValue && durationMinutes.Value < 0)
            throw new ArgumentException("Duration cannot be negative.", nameof(durationMinutes));
        
        DurationMinutes = durationMinutes;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateMaxStudents(int? maxStudents, string? modifiedBy = null)
    {
        if (maxStudents.HasValue && maxStudents.Value <= 0)
            throw new ArgumentException("Max students must be positive.", nameof(maxStudents));
        
        MaxStudents = maxStudents;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateCategory(string? category, string? modifiedBy = null)
    {
        Category = category;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateTags(string? tags, string? modifiedBy = null)
    {
        Tags = tags;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateLastModified(string? modifiedBy = null)
    {
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void AddCourseMaterial(CourseMaterial material)
    {
        if (material == null) throw new ArgumentNullException(nameof(material));
        
        var materials = CourseMaterials.ToList();
        materials.Add(material);
        CourseMaterials = materials.AsReadOnly();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void RemoveCourseMaterial(Guid materialId)
    {
        var materials = CourseMaterials.Where(m => m.Id != materialId).ToList();
        CourseMaterials = materials.AsReadOnly();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void ReorderCourseMaterials(IEnumerable<Guid> materialIds)
    {
        if (materialIds == null) throw new ArgumentNullException(nameof(materialIds));
        
        var materialIdList = materialIds.ToList();
        var materials = CourseMaterials.ToList();
        
        // Reorder materials based on the provided order
        var reorderedMaterials = materialIdList
            .Select(id => materials.FirstOrDefault(m => m.Id == id))
            .Where(m => m != null)
            .ToList();
        
        CourseMaterials = reorderedMaterials.AsReadOnly();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void AddAvailabilitySlot(AvailabilitySlot slot)
    {
        if (slot == null) throw new ArgumentNullException(nameof(slot));
        
        var slots = AvailabilitySlots.ToList();
        slots.Add(slot);
        AvailabilitySlots = slots.AsReadOnly();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void RemoveAvailabilitySlot(Guid slotId)
    {
        var slots = AvailabilitySlots.Where(s => s.Id != slotId).ToList();
        AvailabilitySlots = slots.AsReadOnly();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    internal void UpdateDifficultyLevel(DifficultyLevel newLevel, string? modifiedBy = null)
    {
        DifficultyLevel = newLevel;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}