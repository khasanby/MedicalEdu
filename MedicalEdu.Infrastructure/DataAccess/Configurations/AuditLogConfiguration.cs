using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> entity)
    {
        entity.ToTable("AuditLogs");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.EntityName).IsRequired().HasMaxLength(100);
        entity.Property(e => e.EntityId).IsRequired().HasMaxLength(100);
        entity.Property(e => e.Action).HasConversion<string>().IsRequired();
        entity.Property(e => e.UserId);
        entity.Property(e => e.IpAddress).HasMaxLength(45);
        entity.Property(e => e.UserAgent).HasMaxLength(500);
        entity.Property(e => e.OldValues).HasMaxLength(4000);
        entity.Property(e => e.NewValues).HasMaxLength(4000);
        entity.Property(e => e.Metadata).HasMaxLength(2000);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Indexes
        entity.HasIndex(e => e.EntityName);
        entity.HasIndex(e => e.EntityId);
        entity.HasIndex(e => e.Action);
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.CreatedAt);
    }
}