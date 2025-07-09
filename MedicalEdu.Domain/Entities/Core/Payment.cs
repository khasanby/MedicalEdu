using MedicalEdu.Domain.Abstractions;
using MedicalEdu.Domain.Enums;
using MedicalEdu.Domain.ValueObjects;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Payment : IEntity<Guid>
{
    /// <summary>
    /// Gets and sets privately the unique identifier for the payment.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the booking this payment is for.
    /// </summary>
    public Guid BookingId { get; private set; }

    /// <summary>
    /// Gets and sets privately the unique identifier of the user making the payment.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Gets and sets privately the payment amount.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Gets and sets privately the ISO currency code for the payment (value object).
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    /// Gets and sets privately the payment status.
    /// </summary>
    public PaymentStatus Status { get; private set; }

    /// <summary>
    /// Gets and sets privately the payment provider.
    /// </summary>
    public PaymentProvider Provider { get; private set; }

    /// <summary>
    /// Gets and sets privately the provider's transaction ID.
    /// </summary>
    public string ProviderTransactionId { get; private set; }

    /// <summary>
    /// Gets and sets privately the provider's payment intent ID.
    /// </summary>
    public string? ProviderPaymentIntentId { get; private set; }

    /// <summary>
    /// Gets and sets privately the provider metadata (JSON).
    /// </summary>
    public string? ProviderMetadata { get; private set; }

    /// <summary>
    /// Gets and sets privately the failure reason, if any.
    /// </summary>
    public string? FailureReason { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the payment was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the payment was processed.
    /// </summary>
    public DateTimeOffset? ProcessedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the payment was refunded.
    /// </summary>
    public DateTimeOffset? RefundedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the refund amount, if any.
    /// </summary>
    public decimal? RefundAmount { get; private set; }

    /// <summary>
    /// Gets and sets privately the refund reason, if any.
    /// </summary>
    public string? RefundReason { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the payment was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who created this record, if tracked.
    /// </summary>
    public string? CreatedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the timestamp when the payment was last modified, if any.
    /// </summary>
    public DateTimeOffset? LastModified { get; private set; }

    /// <summary>
    /// Gets and sets privately the identifier of the user who last modified this record, if tracked.
    /// </summary>
    public string? LastModifiedBy { get; private set; }

    /// <summary>
    /// Gets and sets privately the booking this payment is for.
    /// </summary>
    public Booking Booking { get; private set; }

    /// <summary>
    /// Gets and sets privately the user who made this payment.
    /// </summary>
    public User User { get; private set; }

    public Payment(
        Guid id,
        Guid bookingId,
        Guid userId,
        decimal amount,
        Currency currency,
        PaymentProvider provider,
        string providerTransactionId,
        string? providerPaymentIntentId = null,
        string? providerMetadata = null,
        string? createdBy = null)
    {
        if (id == Guid.Empty) throw new ArgumentException("Payment ID is required.", nameof(id));
        if (bookingId == Guid.Empty) throw new ArgumentException("Booking ID is required.", nameof(bookingId));
        if (userId == Guid.Empty) throw new ArgumentException("User ID is required.", nameof(userId));
        if (amount < 0) throw new ArgumentException("Amount cannot be negative.", nameof(amount));
        if (string.IsNullOrWhiteSpace(providerTransactionId)) throw new ArgumentException("Provider transaction ID is required.", nameof(providerTransactionId));
        Currency = currency ?? throw new ArgumentNullException(nameof(currency));

        Id = id;
        BookingId = bookingId;
        UserId = userId;
        Amount = amount;
        Currency = currency;
        Status = PaymentStatus.Pending;
        Provider = provider;
        ProviderTransactionId = providerTransactionId;
        ProviderPaymentIntentId = providerPaymentIntentId;
        ProviderMetadata = providerMetadata;
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = createdBy;
    }

    private Payment() { }
} 