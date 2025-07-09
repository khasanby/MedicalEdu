using MedicalEdu.Domain.Entities;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class AvailabilitySlotConfiguration : IEntityTypeConfiguration<AvailabilitySlot>
{
    public void Configure(EntityTypeBuilder<AvailabilitySlot> entity)
    {
        entity.ToTable("AvailabilitySlots");
        entity.HasGuidKey<AvailabilitySlot>();
        
        entity.Property(e => e.CourseId).IsRequired();
        entity.Property(e => e.InstructorId).IsRequired();
        entity.Property(e => e.StartTimeUtc).IsRequired();
        entity.Property(e => e.EndTimeUtc).IsRequired();
        entity.Property(e => e.IsBooked).IsRequired();
        entity.Property(e => e.Price).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.Currency).HasCurrencyConversion().IsRequired();
        entity.Property(e => e.MaxParticipants).IsRequired();
        entity.Property(e => e.CurrentParticipants).IsRequired();
        entity.Property(e => e.Notes).HasMaxLength(1000);
        entity.Property(e => e.IsRecurring).IsRequired();
        entity.Property(e => e.RecurringPattern).HasMaxLength(1000);

        // Apply common audit properties
        entity.HasAuditProperties<AvailabilitySlot>();

        // Relationships
        entity.HasOne(e => e.Course)
            .WithMany(e => e.AvailabilitySlots)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.Instructor)
            .WithMany(e => e.InstructorAvailabilitySlots)
            .HasForeignKey(e => e.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.CourseId);
        entity.HasIndex(e => e.InstructorId);
        entity.HasIndex(e => e.StartTimeUtc);
        entity.HasIndex(e => e.EndTimeUtc);
        entity.HasIndex(e => e.IsBooked);
    }
}