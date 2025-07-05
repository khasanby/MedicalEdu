using MedicalEdu.Domain.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class BookingPromoCode : IEntity<Guid>
{
    /// <summary>
    /// Gets or sets the unique identifier for the booking promo code entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the booking identifier.
    /// </summary>
    [Required]
    public Guid BookingId { get; set; }

    /// <summary>
    /// Gets or sets the promo code identifier.
    /// </summary>
    [Required]
    public Guid PromoCodeId { get; set; }

    /// <summary>
    /// Gets or sets the discount amount applied.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the promo code was applied.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    // Audit fields from IEntity
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; }
    public virtual PromoCode PromoCode { get; set; }
} 