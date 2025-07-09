using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
{
    public void Configure(EntityTypeBuilder<CourseProgress> entity)
    {
        entity.ToTable("CourseProgresses");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.EnrollmentId).IsRequired();
        entity.Property(e => e.CourseMaterialId).IsRequired();
        entity.Property(e => e.IsCompleted).IsRequired();
        entity.Property(e => e.CompletedAt);
        entity.Property(e => e.TimeSpentSeconds).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.Enrollment)
            .WithMany(e => e.CourseProgresses)
            .HasForeignKey(e => e.EnrollmentId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.CourseMaterial)
            .WithMany(e => e.CourseProgresses)
            .HasForeignKey(e => e.CourseMaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.EnrollmentId);
        entity.HasIndex(e => e.CourseMaterialId);
        entity.HasIndex(e => e.IsCompleted);
    }
}