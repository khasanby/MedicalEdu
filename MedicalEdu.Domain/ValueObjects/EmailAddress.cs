using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class EmailAddress : IEquatable<EmailAddress>
{
    /// <summary>
    /// Gets the value of the email address.
    /// </summary>
    public string Value { get; }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static EmailAddress Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email address cannot be empty", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        return new EmailAddress(email.ToLowerInvariant());
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Implicitly converts the email address to a string.
    /// </summary>
    public static implicit operator string(EmailAddress emailAddress) => emailAddress.Value;

    /// <summary>
    /// Returns the string representation of the email address.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(EmailAddress? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as EmailAddress);
    }

    /// <summary>
    /// Returns the hash code for the email address.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two email addresses are equal.
    /// </summary>
    public static bool operator ==(EmailAddress? left, EmailAddress? right)
    {
        return EqualityComparer<EmailAddress>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two email addresses are not equal.
    /// </summary>
    public static bool operator !=(EmailAddress? left, EmailAddress? right)
    {
        return !(left == right);
    }
} 