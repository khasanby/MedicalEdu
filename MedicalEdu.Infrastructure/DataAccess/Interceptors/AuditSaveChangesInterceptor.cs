using System.Text.Json;
using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedicalEdu.Infrastructure.DataAccess.Interceptors;

public sealed class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        AddAuditEntries(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        AddAuditEntries(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AddAuditEntries(DbContext? context)
    {
        if (context == null) return;

        var auditEntries = GetAuditEntries(context);

        foreach (var auditEntry in auditEntries)
        {
            context.Set<AuditLog>().Add(new AuditLog(
                Guid.NewGuid(),
                auditEntry.TableName,
                auditEntry.EntityId,
                Enum.Parse<AuditActionType>(auditEntry.Action),
                auditEntry.Timestamp,
                null, // UserId - would be set from current user context
                null, // IpAddress - would be set from request context
                null, // UserAgent - would be set from request context
                JsonSerializer.Serialize(auditEntry.OldValues),
                JsonSerializer.Serialize(auditEntry.NewValues),
                null // Metadata
            ));
        }
    }

    private List<AuditEntry> GetAuditEntries(DbContext context)
    {
        context.ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
                Action = entry.State.ToString(),
                EntityId = entry.Properties.First(p => p.Metadata.IsPrimaryKey()).CurrentValue?.ToString() ?? "",
                Timestamp = DateTimeOffset.UtcNow
            };

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                    continue;

                auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
                if (entry.State == EntityState.Modified)
                {
                    auditEntry.OldValues[property.Metadata.Name] = property.OriginalValue;
                }
            }

            auditEntries.Add(auditEntry);
        }

        return auditEntries;
    }

    private sealed class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// Gets the EntityEntry for the current audit entry.
        /// </summary>
        public EntityEntry Entry { get; }

        /// <summary>
        /// Gets or sets the name of the table/entity being audited.
        /// </summary>
        public string TableName { get; set; } = "";

        /// <summary>
        /// Gets or sets the action performed on the entity (e.g., Insert, Update, Delete).
        /// </summary>
        public string Action { get; set; } = "";

        /// <summary>
        /// Gets or sets the unique identifier of the entity being audited.
        /// </summary>
        public string EntityId { get; set; } = "";

        /// <summary>
        /// Gets or sets the old values before the change.
        /// </summary>
        public Dictionary<string, object?> OldValues { get; } = new();

        /// <summary>
        /// Gets or sets the new values after the change.
        /// </summary>
        public Dictionary<string, object?> NewValues { get; } = new();

        /// <summary>
        /// Gets or sets the timestamp when the action was performed.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}