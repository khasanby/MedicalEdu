using System.ComponentModel.DataAnnotations;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class Email : IEquatable<Email>
{
    /// <summary>
    /// Gets the value of the email.
    /// </summary>
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        return new Email(email.ToLowerInvariant());
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
    /// Implicitly converts the email to a string.
    /// </summary>
    public static implicit operator string(Email email) => email.Value;

    /// <summary>
    /// Returns the string representation of the email.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(Email? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Email);
    }

    /// <summary>
    /// Returns the hash code for the email.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two emails are equal.
    /// </summary>
    public static bool operator ==(Email? left, Email? right)
    {
        return EqualityComparer<Email>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two emails are not equal.
    /// </summary>
    public static bool operator !=(Email? left, Email? right)
    {
        return !(left == right);
    }
}