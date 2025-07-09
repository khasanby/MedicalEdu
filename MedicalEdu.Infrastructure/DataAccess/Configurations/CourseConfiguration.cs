using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> entity)
    {
        entity.ToTable("Courses");
        entity.HasGuidKey<Course>();
        
        entity.Property(e => e.InstructorId).IsRequired();
        entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Description).HasMaxLength(2000);
        entity.Property(e => e.ShortDescription).HasMaxLength(500);
        entity.Property(e => e.IsPublished).IsRequired();
        entity.Property(e => e.Price).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.Currency).HasCurrencyConversion().IsRequired();
        entity.Property(e => e.ThumbnailUrl).HasMaxLength(500);
        entity.Property(e => e.VideoIntroUrl).HasMaxLength(500);
        entity.Property(e => e.DurationMinutes);
        entity.Property(e => e.MaxStudents);
        entity.Property(e => e.Category).HasMaxLength(100);
        entity.Property(e => e.Tags).HasMaxLength(500);
        entity.Property(e => e.PublishedAt);
        entity.Property(e => e.DifficultyLevel).HasConversion<string>().IsRequired();

        // Apply common audit and soft delete properties
        entity.HasAuditProperties<Course>();
        entity.HasSoftDelete<Course>();

        // Relationships
        entity.HasOne(e => e.Instructor)
            .WithMany(e => e.InstructorCourses)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.InstructorId);
        entity.HasIndex(e => e.IsPublished);
        entity.HasIndex(e => e.Category);
        entity.HasIndex(e => e.DifficultyLevel);
        entity.HasIndex(e => e.CreatedAt);
    }
}