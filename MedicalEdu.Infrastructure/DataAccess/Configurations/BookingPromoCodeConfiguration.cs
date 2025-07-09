using MedicalEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class BookingPromoCodeConfiguration : IEntityTypeConfiguration<BookingPromoCode>
{
    public void Configure(EntityTypeBuilder<BookingPromoCode> entity)
    {
        entity.ToTable("BookingPromoCodes");
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Id).ValueGeneratedNever();
        entity.Property(e => e.BookingId).IsRequired();
        entity.Property(e => e.PromoCodeId).IsRequired();
        entity.Property(e => e.DiscountAmount).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.CreatedAt).IsRequired();
        entity.Property(e => e.CreatedBy).HasMaxLength(255);
        entity.Property(e => e.LastModified);
        entity.Property(e => e.LastModifiedBy).HasMaxLength(255);

        // Relationships
        entity.HasOne(e => e.Booking)
            .WithMany(e => e.BookingPromoCodes)
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.PromoCode)
            .WithMany(e => e.BookingPromoCodes)
            .HasForeignKey(e => e.PromoCodeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.BookingId);
        entity.HasIndex(e => e.PromoCodeId);
        entity.HasIndex(e => e.CreatedAt);
    }
}