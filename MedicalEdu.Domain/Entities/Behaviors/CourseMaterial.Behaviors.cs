namespace MedicalEdu.Domain.Entities;

public sealed partial class CourseMaterial
{
    internal void UpdateTitle(string newTitle, string? modifiedBy = null)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ArgumentException("Title is required.", nameof(newTitle));

        Title = newTitle;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateDescription(string? newDesc, string? modifiedBy = null)
    {
        Description = newDesc;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void UpdateFile(string newUrl, string newName, string newType, long newSize, string? modifiedBy = null)
    {
        if (string.IsNullOrWhiteSpace(newUrl) ||
            string.IsNullOrWhiteSpace(newName) ||
            string.IsNullOrWhiteSpace(newType) ||
            newSize < 0)
            throw new ArgumentException("Invalid file parameters.");

        FileUrl = newUrl;
        FileName = newName;
        ContentType = newType;
        FileSizeBytes = newSize;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void Reorder(int newIndex, string? modifiedBy = null)
    {
        if (newIndex < 0)
            throw new ArgumentException("Sort index must be non-negative.", nameof(newIndex));

        SortIndex = newIndex;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void MarkAsFree(string? modifiedBy = null)
    {
        IsFree = true;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void MarkAsPaid(string? modifiedBy = null)
    {
        IsFree = false;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void SetRequired(bool required, string? modifiedBy = null)
    {
        IsRequired = required;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}