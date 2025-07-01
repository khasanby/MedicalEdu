using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class CourseMaterial
{
    /// <summary>
    /// Gets or sets the unique identifier for the course material.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the course identifier this material belongs to.
    /// </summary>
    [Required]
    public Guid CourseId { get; set; }

    /// <summary>
    /// Gets or sets the title of the material.
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the description of the material.
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the file URL in blob storage.
    /// </summary>
    [Required]
    public string FileUrl { get; set; }

    /// <summary>
    /// Gets or sets the original file name.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string FileName { get; set; }

    /// <summary>
    /// Gets or sets the file content type (MIME type).
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the file size in bytes.
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// Gets or sets the order/sequence of this material within the course.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets whether this material is free or requires payment/enrollment.
    /// </summary>
    public bool IsFree { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the material was uploaded.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the material was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Navigation property
    public virtual Course Course { get; set; }
}