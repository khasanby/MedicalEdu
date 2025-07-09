using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> entity)
    {
        entity.ToTable("Payments");
        entity.HasGuidKey<Payment>();
        
        entity.Property(e => e.BookingId).IsRequired();
        entity.Property(e => e.UserId).IsRequired();
        entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.Currency).HasCurrencyConversion().IsRequired();
        entity.Property(e => e.Status).HasConversion<string>().IsRequired();
        entity.Property(e => e.Provider).HasConversion<string>().IsRequired();
        entity.Property(e => e.ProviderTransactionId).IsRequired().HasMaxLength(255);
        entity.Property(e => e.ProviderPaymentIntentId).HasMaxLength(255);
        entity.Property(e => e.ProviderMetadata).HasMaxLength(2000);
        entity.Property(e => e.FailureReason).HasMaxLength(500);
        entity.Property(e => e.ProcessedAt);
        entity.Property(e => e.RefundedAt);
        entity.Property(e => e.RefundAmount).HasPrecision(18, 2);
        entity.Property(e => e.RefundReason).HasMaxLength(500);

        // Apply common audit properties
        entity.HasAuditProperties<Payment>();

        // Relationships
        entity.HasOne(e => e.Booking)
            .WithMany(e => e.Payments)
            .HasForeignKey(e => e.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(e => e.User)
            .WithMany(e => e.Payments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        entity.HasIndex(e => e.BookingId);
        entity.HasIndex(e => e.UserId);
        entity.HasIndex(e => e.Status);
        entity.HasIndex(e => e.Provider);
        entity.HasIndex(e => e.ProviderTransactionId).IsUnique();
        entity.HasIndex(e => e.CreatedAt);
    }
}