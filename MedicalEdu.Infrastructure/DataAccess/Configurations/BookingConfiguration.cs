using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> entity)
    {
        entity.ToTable("Bookings");
        entity.HasGuidKey<Booking>();
        
        entity.Property(e => e.StudentId).IsRequired();
        entity.Property(e => e.AvailabilitySlotId).IsRequired();
        entity.Property(e => e.Status).HasConversion<string>().IsRequired();
        entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.Currency).HasCurrencyConversion().IsRequired();
        entity.Property(e => e.StudentNotes).HasMaxLength(1000);
        entity.Property(e => e.InstructorNotes).HasMaxLength(1000);
        entity.Property(e => e.ConfirmedAt);
        entity.Property(e => e.CancelledAt);
        entity.Property(e => e.CancellationReason).HasMaxLength(500);
        entity.Property(e => e.RescheduledFromBookingId);
        entity.Property(e => e.MeetingUrl).HasMaxLength(500);
        entity.Property(e => e.MeetingNotes).HasMaxLength(1000);

        // Apply common audit properties
        entity.HasAuditProperties<Booking>();

        // Relationships
        entity.HasOne(e => e.Student)
            .WithMany(e => e.StudentBookings)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne<AvailabilitySlot>()
            .WithMany(e => e.Bookings)
            .HasForeignKey(e => e.AvailabilitySlotId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.StudentId);
        entity.HasIndex(e => e.AvailabilitySlotId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.CreatedAt);
    }
}