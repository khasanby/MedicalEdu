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

    internal void SetThumbnail(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException("Thumbnail URL is required.", nameof(url));
        ThumbnailUrl = url;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}