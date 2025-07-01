using MedicalEdu.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.Entities;

public sealed class Payment
{
    /// <summary>
    /// Gets or sets the unique identifier for the payment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the booking identifier this payment is for.
    /// </summary>
    [Required]
    public Guid BookingId { get; set; }

    /// <summary>
    /// Gets or sets the user identifier who made the payment.
    /// </summary>
    [Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the payment amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the currency code (e.g., USD, EUR).
    /// </summary>
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; }

    /// <summary>
    /// Gets or sets the payment status.
    /// </summary>
    public PaymentStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the payment provider used.
    /// </summary>
    public PaymentProvider Provider { get; set; }

    /// <summary>
    /// Gets or sets the provider transaction identifier.
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string ProviderTransactionId { get; set; }

    /// <summary>
    /// Gets or sets the provider payment intent identifier.
    /// </summary>
    [MaxLength(255)]
    public string? ProviderPaymentIntentId { get; set; }

    /// <summary>
    /// Gets or sets additional provider-specific metadata as JSON.
    /// </summary>
    public string? ProviderMetadata { get; set; }

    /// <summary>
    /// Gets or sets the failure reason if payment failed.
    /// </summary>
    [MaxLength(500)]
    public string? FailureReason { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the payment was refunded.
    /// </summary>
    public DateTime? RefundedAt { get; set; }

    /// <summary>
    /// Gets or sets the refund amount if applicable.
    /// </summary>
    public decimal? RefundAmount { get; set; }

    // Navigation properties
    public virtual Booking Booking { get; set; }
    public virtual User User { get; set; }
} 