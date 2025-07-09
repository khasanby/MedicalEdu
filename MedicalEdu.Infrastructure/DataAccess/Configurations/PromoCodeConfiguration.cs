using MedicalEdu.Domain.Entities;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Infrastructure.DataAccess.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalEdu.Infrastructure.DataAccess.Configurations;

public sealed class PromoCodeConfiguration : IEntityTypeConfiguration<PromoCode>
{
    public void Configure(EntityTypeBuilder<PromoCode> entity)
    {
        entity.ToTable("PromoCodes");
        entity.HasGuidKey<PromoCode>();
        
        entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
        entity.Property(e => e.Description).HasMaxLength(500);
        entity.Property(e => e.DiscountType).HasConversion<string>().IsRequired();
        entity.Property(e => e.DiscountValue).HasPrecision(18, 2).IsRequired();
        entity.Property(e => e.Currency).HasCurrencyConversion().IsRequired();
        entity.Property(e => e.MaxUses);
        entity.Property(e => e.CurrentUses).IsRequired();
        entity.Property(e => e.ValidFrom).IsRequired();
        entity.Property(e => e.ValidUntil).IsRequired();
        entity.Property(e => e.IsActive).IsRequired();
        entity.Property(e => e.ApplicableCourseIds).HasMaxLength(2000);

        // Apply common audit properties
        entity.HasAuditProperties<PromoCode>();

        // Indexes
        entity.HasIndex(e => e.Code).IsUnique();
        entity.HasIndex(e => e.IsActive);
        entity.HasIndex(e => e.ValidFrom);
        entity.HasIndex(e => e.ValidUntil);
    }
}