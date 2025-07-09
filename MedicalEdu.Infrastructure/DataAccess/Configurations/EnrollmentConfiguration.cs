using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> entity)
    {
        entity.ToTable("Enrollments");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.EnrolledAt).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.ProgressPercentage).IsRequired();
        entity.Property(e => e.CompletedAt);
        entity.Property(e => e.LastAccessedAt);
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.Student)
            .WithMany(e => e.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Course)
            .WithMany(e => e.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.IsActive);
        entity.HasIndex(e => e.EnrolledAt);
    }
}