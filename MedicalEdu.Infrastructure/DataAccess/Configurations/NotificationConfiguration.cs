using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> entity)
    {
        entity.ToTable("Notifications");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.UserId).IsRequired();
        entity.Property(e => e.Type).HasConversion<string>().IsRequired();
        entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
        entity.Property(e => e.IsRead).IsRequired();
        entity.Property(e => e.EmailSent).IsRequired();
        entity.Property(e => e.EmailSentAt);
        entity.Property(e => e.SmsSent).IsRequired();
        entity.Property(e => e.SmsSentAt);
        entity.Property(e => e.PushSent).IsRequired();
        entity.Property(e => e.PushSentAt);
        entity.Property(e => e.RelatedEntityId);
        entity.Property(e => e.RelatedEntityType).HasMaxLength(100);
        entity.Property(e => e.Metadata).HasMaxLength(2000);
        entity.Property(e => e.ScheduledFor);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.ReadAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne<User>()
            .WithMany(e => e.Notifications)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.Type);
        entity.HasIndex(e => e.IsRead);
        entity.HasIndex(e => e.CreatedAt);
        entity.HasIndex(e => e.ScheduledFor);
    }
}