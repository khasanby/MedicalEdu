using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class CourseMaterial : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the course material.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the course this material belongs to.
    /// </summary>
    public Guid CourseId { get; private set; }

    /// <summary>
    /// Gets and sets privately the title of the course material.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Gets and sets privately the description of the course material.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets and sets privately the URL where the file is stored.
    /// </summary>
    public string FileUrl { get; private set; }

    /// <summary>
    /// Gets and sets privately the original filename of the uploaded file.
    /// </summary>
    public string FileName { get; private set; }

    /// <summary>
    /// Gets and sets privately the MIME content type of the file.
    /// </summary>
    public string ContentType { get; private set; }

    /// <summary>
    /// Gets and sets privately the size of the file in bytes.
    /// </summary>
    public long FileSizeBytes { get; private set; }

    /// <summary>
    /// Gets and sets privately the sort order index for displaying materials in sequence.
    /// </summary>
    public int SortIndex { get; private set; }

    /// <summary>
    /// Gets and sets privately whether this material is free or requires payment.
    /// </summary>
    public bool IsFree { get; private set; }

    /// <summary>
    /// Gets and sets privately the duration in minutes for video content.
    /// </summary>
    public int? DurationMinutes { get; private set; }

    /// <summary>
    /// Gets and sets privately whether this material is required for course completion.
    /// </summary>
    public bool IsRequired { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course material was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course material was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the course material was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the course this material belongs to.
    /// </summary>
    public Course Course { get; private set; }

    /// <summary>
    /// Gets and sets privately the course progress records for this material.
    /// </summary>
    public IReadOnlyCollection<CourseProgress> CourseProgresses { get; private set; } 
        = new List<CourseProgress>();

    public CourseMaterial(
        Guid id,
        Guid courseId,
        string title,
        string fileUrl,
        string fileName,
        string contentType,
        long fileSizeBytes,
        int sortIndex,
        bool isFree = false,
        int? durationMinutes = null,
        bool isRequired = true,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("ID is required.", nameof(id));
        if (courseId == Guid.Empty) throw new ArgumentException("Course ID is required.", nameof(courseId));
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        if (string.IsNullOrWhiteSpace(fileUrl)) throw new ArgumentException("File URL is required.", nameof(fileUrl));
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("File name is required.", nameof(fileName));
        if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentException("Content type is required.", nameof(contentType));
        if (fileSizeBytes < 0) throw new ArgumentException("File size must be non-negative.", nameof(fileSizeBytes));
        if (sortIndex < 0) throw new ArgumentException("Sort index must be non-negative.", nameof(sortIndex));

        Id = id;
        CourseId = courseId;
        Title = title;
        Description = null;
        FileUrl = fileUrl;
        FileName = fileName;
        ContentType = contentType;
        FileSizeBytes = fileSizeBytes;
        SortIndex = sortIndex;
        IsFree = isFree;
        DurationMinutes = durationMinutes;
        IsRequired = isRequired;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    private CourseMaterial() { }
}