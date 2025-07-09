using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class CourseRatingConfiguration : IEntityTypeConfiguration<CourseRating>
{
    public void Configure(EntityTypeBuilder<CourseRating> entity)
    {
        entity.ToTable("CourseRatings");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.Rating).IsRequired();
        entity.Property(e => e.Review).HasMaxLength(2000);
        entity.Property(e => e.IsPublic).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.Course)
            .WithMany(e => e.CourseRatings)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Student)
            .WithMany(e => e.CourseRatings)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.Rating);
        entity.HasIndex(e => e.IsPublic);
        entity.HasIndex(e => e.CreatedAt);
    }
}