using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class PromoCode : IEntity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the promo code.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the promo code string.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the description of the promo code.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the type of discount (percentage or fixed amount).
    /// </summary>
    public DiscountType DiscountType { get; set; }

    /// <summary>
    /// Gets or sets the discount value.
    /// </summary>
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Gets or sets the currency for the discount.
    /// </summary>
    [MaxLength(3)]
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Gets or sets the maximum number of times this promo code can be used.
    /// </summary>
    public int? MaxUses { get; set; }

    /// <summary>
    /// Gets or sets the current number of times this promo code has been used.
    /// </summary>
    public int CurrentUses { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the promo code becomes valid.
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the promo code expires.
    /// </summary>
    public DateTime ValidUntil { get; set; }

    /// <summary>
    /// Gets or sets whether the promo code is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the applicable course IDs as JSON array (null for all courses).
    /// </summary>
    public string? ApplicableCourseIds { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the promo code was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the promo code was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation properties
    public virtual ICollection<BookingPromoCode> BookingPromoCodes { get; set; } = new List<BookingPromoCode>();
} 