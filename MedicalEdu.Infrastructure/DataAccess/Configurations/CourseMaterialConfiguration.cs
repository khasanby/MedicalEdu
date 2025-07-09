using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class CourseMaterialConfiguration : IEntityTypeConfiguration<CourseMaterial>
{
    public void Configure(EntityTypeBuilder<CourseMaterial> entity)
    {
        entity.ToTable("CourseMaterials");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
        entity.Property(e => e.Description).HasMaxLength(2000);
        entity.Property(e => e.FileUrl).IsRequired().HasMaxLength(500);
        entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ContentType).IsRequired().HasMaxLength(100);
        entity.Property(e => e.FileSizeBytes).IsRequired();
        entity.Property(e => e.SortIndex).IsRequired();
        entity.Property(e => e.IsFree).IsRequired();
        entity.Property(e => e.DurationMinutes);
        entity.Property(e => e.IsRequired).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.Course)
            .WithMany(e => e.CourseMaterials)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.SortIndex);
        entity.HasIndex(e => e.IsFree);
    }
}