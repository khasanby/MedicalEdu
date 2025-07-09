using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class InstructorRatingConfiguration : IEntityTypeConfiguration<InstructorRating>
{
    public void Configure(EntityTypeBuilder<InstructorRating> entity)
    {
        entity.ToTable("InstructorRatings");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.InstructorId).IsRequired();
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.BookingId).IsRequired();
        entity.Property(e => e.Rating).IsRequired();
        entity.Property(e => e.Review).HasMaxLength(2000);
        entity.Property(e => e.IsPublic).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.UpdatedAt);
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.Instructor)
            .WithMany(e => e.InstructorRatingsReceived)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Student)
            .WithMany(e => e.InstructorRatingsGiven)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(e => e.Booking)
            .WithMany(e => e.InstructorRatings)
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        entity.HasIndex(e => e.InstructorId);
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.BookingId);
        entity.HasIndex(e => e.Rating);
        entity.HasIndex(e => e.IsPublic);
        entity.HasIndex(e => e.CreatedAt);
    }
}