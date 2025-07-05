using MedicalEdu.Domain.Abstractions;

namespace MedicalEdu.Domain.Entities;

public sealed partial class BookingPromoCode : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the booking promo code record.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the booking.
    /// </summary>
    public Guid BookingId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the promo code.
    /// </summary>
    public Guid PromoCodeId { get; private set; }

    /// <summary>
    /// Gets and sets privately the discount amount applied.
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the promo code was applied.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the booking this promo code was applied to.
    /// </summary>
    public Booking Booking { get; private set; }

    /// <summary>
    /// Gets and sets privately the promo code that was applied.
    /// </summary>
    public PromoCode PromoCode { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the booking promo code was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    public BookingPromoCode(
        Guid id,
        Guid bookingId,
        Guid promoCodeId,
        decimal discountAmount,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Booking promo code ID is required.", nameof(id));
        if (bookingId == Guid.Empty) throw new ArgumentException("Booking ID is required.", nameof(bookingId));
        if (promoCodeId == Guid.Empty) throw new ArgumentException("Promo code ID is required.", nameof(promoCodeId));
        if (discountAmount < 0) throw new ArgumentException("Discount amount cannot be negative.", nameof(discountAmount));

        Id = id;
        BookingId = bookingId;
        PromoCodeId = promoCodeId;
        DiscountAmount = discountAmount;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
}