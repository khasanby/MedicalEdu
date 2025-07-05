using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.Entities;

public sealed class AuditLog : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the audit log entry.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the name of the entity being audited.
    /// </summary>
    public string EntityName { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the entity being audited.
    /// </summary>
    public string EntityId { get; private set; }

    /// <summary>
    /// Gets and sets privately the type of action performed.
    /// </summary>
    public AuditActionType Action { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the user who performed the action.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets and sets privately the IP address of the user who performed the action.
    /// </summary>
    public string? IpAddress { get; private set; }

    /// <summary>
    /// Gets and sets privately the user agent string of the client.
    /// </summary>
    public string? UserAgent { get; private set; }

    /// <summary>
    /// Gets and sets privately the old values before the change (JSON).
    /// </summary>
    public string? OldValues { get; private set; }

    /// <summary>
    /// Gets and sets privately the new values after the change (JSON).
    /// </summary>
    public string? NewValues { get; private set; }

    /// <summary>
    /// Gets and sets privately additional metadata about the action (JSON).
    /// </summary>
    public string? Metadata { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the audit log entry was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the audit log was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    public AuditLog(
        Guid id,
        string entityName,
        string entityId,
        AuditActionType action,
        DateTimeOffset? createdAt = null,
        Guid? userId = null,
        string? ipAddress = null,
        string? userAgent = null,
        string? oldValues = null,
        string? newValues = null,
        string? metadata = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Audit log ID is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(entityName)) throw new ArgumentException("Entity name is required.", nameof(entityName));
        if (string.IsNullOrWhiteSpace(entityId)) throw new ArgumentException("Entity ID is required.", nameof(entityId));

        Id = id;
        EntityName = entityName;
        EntityId = entityId;
        Action = action;
        UserId = userId;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        OldValues = oldValues;
        NewValues = newValues;
        Metadata = metadata;
        CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
}