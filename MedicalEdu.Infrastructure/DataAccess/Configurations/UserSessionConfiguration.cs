using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> entity)
    {
        entity.ToTable("UserSessions");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.UserId).IsRequired();
        entity.Property(e => e.SessionToken).IsRequired().HasMaxLength(500);
        entity.Property(e => e.IpAddress).HasMaxLength(45);
        entity.Property(e => e.UserAgent).HasMaxLength(500);
        entity.Property(e => e.ExpiresAt).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.LastActivityAt).IsRequired();
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.User)
            .WithMany(e => e.UserSessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.SessionToken).IsUnique();
        entity.HasIndex(e => e.ExpiresAt);
        entity.HasIndex(e => e.LastActivityAt);
    }
}