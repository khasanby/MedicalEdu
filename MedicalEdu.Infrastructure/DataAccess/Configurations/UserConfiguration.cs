using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");
        entity.HasGuidKey<User>();
        
        entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Email).HasEmailConversion().IsRequired();
        entity.Property(e => e.PasswordHash).HasPasswordHashConversion().IsRequired();
        entity.Property(e => e.Role).HasConversion<string>().IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.EmailConfirmed).IsRequired();
        entity.Property(e => e.EmailConfirmationToken).HasMaxLength(500);
        entity.Property(e => e.EmailConfirmationTokenExpiry);
        entity.Property(e => e.PasswordResetToken).HasMaxLength(500);
        entity.Property(e => e.PasswordResetTokenExpiry);
        entity.Property(e => e.Zone).HasTimeZoneConversion().IsRequired();
        entity.Property(e => e.PhoneNumber).HasNullablePhoneNumberConversion();
        entity.Property(e => e.ProfilePictureUrl).HasNullableUrlConversion();
        entity.Property(e => e.LastLoginAt);
        entity.Property(e => e.FailedLoginAttempts).IsRequired();
        entity.Property(e => e.LockedUntil);

        // Apply common audit and soft delete properties
        entity.HasAuditProperties<User>();
        entity.HasSoftDelete<User>();

        // Indexes
        entity.HasIndex(e => e.Email).IsUnique();
        entity.HasIndex(e => e.Role);
        entity.HasIndex(e => e.IsActive);
        entity.HasIndex(e => e.CreatedAt);
    }
}