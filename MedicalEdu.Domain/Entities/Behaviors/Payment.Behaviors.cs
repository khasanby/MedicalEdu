using MedicalEdu.Domain.Enums;

namespace MedicalEdu.Domain.Entities;

public sealed partial class Payment
{
    internal void MarkSucceeded(string? modifiedBy = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be marked as succeeded.");

        Status = PaymentStatus.Succeeded;
        ProcessedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void MarkFailed(string reason, string? modifiedBy = null)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be marked as failed.");
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Failure reason is required.", nameof(reason));

        Status = PaymentStatus.Failed;
        FailureReason = reason;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void Cancel(string? modifiedBy = null)
    {
        if (Status != PaymentStatus.Pending && Status != PaymentStatus.Succeeded)
            throw new InvalidOperationException("Only pending or succeeded payments can be cancelled.");
        Status = PaymentStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;
    }

    internal void Refund(decimal amount, string reason, string? modifiedBy = null)
    {
        if (Status != PaymentStatus.Succeeded)
            throw new InvalidOperationException("Only succeeded payments can be refunded.");
        if (amount <= 0 || amount > Amount)
            throw new ArgumentException("Refund amount must be positive and less than or equal to the payment amount.", nameof(amount));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Refund reason is required.", nameof(reason));

        RefundAmount = amount;
        RefundReason = reason;
        RefundedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        LastModified = DateTimeOffset.UtcNow;
        LastModifiedBy = modifiedBy;

        if (amount == Amount)
            Status = PaymentStatus.Refunded;
        else
            Status = PaymentStatus.PartiallyRefunded;
    }
}
