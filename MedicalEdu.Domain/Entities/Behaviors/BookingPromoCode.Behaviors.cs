namespace MedicalEdu.Domain.Entities;

public sealed partial class BookingPromoCode
{
    internal void UpdateDiscountAmount(decimal newAmount, string? modifiedBy = null)
    {
        if (newAmount < 0)
            throw new ArgumentException("Discount amount cannot be negative.", nameof(newAmount));

        DiscountAmount = newAmount;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }
}