using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class PromoCode : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the promo code.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the promo code string (e.g., "SAVE20").
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Gets and sets privately the description of the promo code.
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// Gets and sets privately the type of discount (percentage or fixed amount).
    /// </summary>
    public DiscountType DiscountType { get; private set; }

    /// <summary>
    /// Gets and sets privately the discount value.
    /// </summary>
    public decimal DiscountValue { get; private set; }

    /// <summary>
    /// Gets and sets privately the ISO currency code for the discount (value object).
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    /// Gets and sets privately the maximum number of times this code can be used.
    /// </summary>
    public int? MaxUses { get; private set; }

    /// <summary>
    /// Gets and sets privately the current number of times this code has been used.
    /// </summary>
    public int CurrentUses { get; private set; }

    /// <summary>
    /// Gets and sets privately the date when the promo code becomes valid.
    /// </summary>
    public DateTimeOffset ValidFrom { get; private set; }

    /// <summary>
    /// Gets and sets privately the date when the promo code expires.
    /// </summary>
    public DateTimeOffset ValidUntil { get; private set; }

    /// <summary>
    /// Gets and sets privately whether the promo code is currently active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Gets and sets privately the JSON array of applicable course IDs, null for all courses.
    /// </summary>
    public string? ApplicableCourseIds { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the promo code was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the promo code was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the promo code was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the booking promo codes that use this promo code.
    /// </summary>
    public IReadOnlyCollection<BookingPromoCode> BookingPromoCodes { get; private set; }
        = new List<BookingPromoCode>();

    public PromoCode(
        Guid id,
        string code,
        DiscountType discountType,
        decimal discountValue,
        Currency currency,
        DateTimeOffset validFrom,
        DateTimeOffset validUntil,
        string? description = null,
        int? maxUses = null,
        string? applicableCourseIds = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Promo code ID is required.", nameof(id));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code is required.", nameof(code));
        if (discountValue < 0) throw new ArgumentException("Discount value cannot be negative.", nameof(discountValue));
        if (validUntil <= validFrom) throw new ArgumentException("Valid until must be after valid from.", nameof(validUntil));
        if (maxUses.HasValue && maxUses <= 0) throw new ArgumentException("Max uses must be positive if specified.", nameof(maxUses));
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        Id = id;
        Code = code;
        Description = description;
        DiscountType = discountType;
        DiscountValue = discountValue;
        MaxUses = maxUses;
        CurrentUses = 0;
        ValidFrom = validFrom;
        ValidUntil = validUntil;
        IsActive = true;
        ApplicableCourseIds = applicableCourseIds;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }
}