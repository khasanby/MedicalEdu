using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    private static readonly Regex E164Pattern = new(@"^\+[1-9]\d{1,14}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the phone number in E.164 format.
    /// </summary>
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new PhoneNumber instance from a valid E.164 formatted number.
    /// </summary>
    public static PhoneNumber Create(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be empty", nameof(phoneNumber));

        var normalized = NormalizePhoneNumber(phoneNumber);

        if (!IsValidE164(normalized))
            throw new ArgumentException("Phone number must be in E.164 format (e.g., +1234567890)", nameof(phoneNumber));

        return new PhoneNumber(normalized);
    }

    /// <summary>
    /// Attempts to create a PhoneNumber instance from a string.
    /// </summary>
    public static bool TryCreate(string phoneNumber, out PhoneNumber? result)
    {
        try
        {
            result = Create(phoneNumber);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private static string NormalizePhoneNumber(string phoneNumber)
    {
        // Remove all non-digit characters except the leading +
        var cleaned = new string(phoneNumber.Where(c => char.IsDigit(c) || c == '+').ToArray());

        // If it doesn't start with +, add it
        if (!cleaned.StartsWith("+"))
        {
            cleaned = "+" + cleaned;
        }

        return cleaned;
    }

    private static bool IsValidE164(string phoneNumber)
    {
        return E164Pattern.IsMatch(phoneNumber);
    }

    /// <summary>
    /// Gets the country code from the phone number.
    /// </summary>
    public string GetCountryCode()
    {
        // Remove the + and get the country code (1-3 digits)
        var withoutPlus = Value.Substring(1);
        return withoutPlus.Length >= 1 ? withoutPlus.Substring(0, Math.Min(3, withoutPlus.Length)) : "";
    }

    /// <summary>
    /// Gets the national number part (without country code).
    /// </summary>
    public string GetNationalNumber()
    {
        var countryCode = GetCountryCode();
        return Value.Substring(1 + countryCode.Length);
    }

    /// <summary>
    /// Implicitly converts the phone number to a string.
    /// </summary>
    public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;

    /// <summary>
    /// Returns the string representation of the phone number.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(PhoneNumber? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PhoneNumber);
    }

    /// <summary>
    /// Returns the hash code for the phone number.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two phone numbers are equal.
    /// </summary>
    public static bool operator ==(PhoneNumber? left, PhoneNumber? right)
    {
        return EqualityComparer<PhoneNumber>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two phone numbers are not equal.
    /// </summary>
    public static bool operator !=(PhoneNumber? left, PhoneNumber? right)
    {
        return !(left == right);
    }
}