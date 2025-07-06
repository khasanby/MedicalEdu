using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class PromoCode : IEquatable<PromoCode>
{
    private static readonly Regex PromoCodePattern = new(@"^[A-Z0-9]{4,20}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the promotional code value.
    /// </summary>
    public string Value { get; }

    private PromoCode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new PromoCode instance from a valid promotional code string.
    /// </summary>
    public static PromoCode Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Promotional code cannot be empty", nameof(code));

        var normalized = NormalizeCode(code);

        if (!IsValidPromoCode(normalized))
            throw new ArgumentException("Promotional code must be 4-20 characters long and contain only uppercase letters and numbers", nameof(code));

        return new PromoCode(normalized);
    }

    /// <summary>
    /// Attempts to create a PromoCode instance from a string.
    /// </summary>
    public static bool TryCreate(string code, out PromoCode? result)
    {
        try
        {
            result = Create(code);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// Generates a random promotional code.
    /// </summary>
    public static PromoCode Generate(int length = 8)
    {
        if (length < 4 || length > 20)
            throw new ArgumentException("Code length must be between 4 and 20 characters", nameof(length));

        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var code = new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        return new PromoCode(code);
    }

    private static string NormalizeCode(string code)
    {
        // Remove spaces and convert to uppercase
        return code.Replace(" ", "").ToUpperInvariant();
    }

    private static bool IsValidPromoCode(string code)
    {
        return PromoCodePattern.IsMatch(code);
    }

    /// <summary>
    /// Gets the length of the promotional code.
    /// </summary>
    public int Length => Value.Length;

    /// <summary>
    /// Checks if the promotional code is valid.
    /// </summary>
    public bool IsValid => IsValidPromoCode(Value);

    /// <summary>
    /// Implicitly converts the promotional code to a string.
    /// </summary>
    public static implicit operator string(PromoCode promoCode) => promoCode.Value;

    /// <summary>
    /// Returns the string representation of the promotional code.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(PromoCode? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PromoCode);
    }

    /// <summary>
    /// Returns the hash code for the promotional code.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two promotional codes are equal.
    /// </summary>
    public static bool operator ==(PromoCode? left, PromoCode? right)
    {
        return EqualityComparer<PromoCode>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two promotional codes are not equal.
    /// </summary>
    public static bool operator !=(PromoCode? left, PromoCode? right)
    {
        return !(left == right);
    }
}