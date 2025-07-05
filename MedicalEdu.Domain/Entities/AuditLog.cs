using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class AuditLog : IEntity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit log entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the entity that was modified.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string EntityName { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the entity that was modified.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string EntityId { get; set; }

    /// <summary>
    /// Gets or sets the action that was performed.
    /// </summary>
    public AuditActionType Action { get; set; }

    /// <summary>
    /// Gets or sets the user identifier who performed the action.
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Gets or sets the user's IP address when the action was performed.
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// Gets or sets the user agent string.
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    /// <summary>
    /// Gets or sets the old values before the change (JSON format).
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// Gets or sets the new values after the change (JSON format).
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// Gets or sets additional context or metadata (JSON format).
    /// </summary>
    public string? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the audit log entry was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation property
    public virtual User? User { get; set; }
} 