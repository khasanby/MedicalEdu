namespace MedicalEdu.Domain.Entities;

public sealed partial class PromoCode
{
    internal void UpdateDescription(string? newDescription, string? modifiedBy = null)
    {
        Description = newDescription;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateDiscountValue(decimal newValue, string? modifiedBy = null)
    {
        if (newValue < 0)
            throw new ArgumentException("Discount value cannot be negative.", nameof(newValue));
        DiscountValue = newValue;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateMaxUses(int? newMaxUses, string? modifiedBy = null)
    {
        if (newMaxUses.HasValue && newMaxUses <= 0)
            throw new ArgumentException("Max uses must be positive if specified.", nameof(newMaxUses));
        MaxUses = newMaxUses;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void SetActive(bool isActive, string? modifiedBy = null)
    {
        IsActive = isActive;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateApplicableCourseIds(string? newIds, string? modifiedBy = null)
    {
        ApplicableCourseIds = newIds;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}