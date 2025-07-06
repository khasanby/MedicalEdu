using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class Currency
{
    private static readonly Regex CurrencyCodePattern = new(@"^[A-Z]{3}$", RegexOptions.Compiled);

    /// <summary>
    /// Gets the ISO currency code (e.g., "USD", "EUR", "GBP").
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Private constructor to enforce the use of Parse or TryParse methods.
    /// </summary>
    private Currency(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Creates a new Currency instance from a valid ISO currency code.
    /// </summary>
    public static Currency Parse(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be null or empty.", nameof(code));

        if (!CurrencyCodePattern.IsMatch(code))
            throw new ArgumentException("Currency code must be a 3-letter ISO code (e.g., USD, EUR).", nameof(code));

        return new Currency(code);
    }

    /// <summary>
    /// Attempts to create a Currency instance from a string.
    /// </summary>
    public static bool TryParse(string code, out Currency? currency)
    {
        try
        {
            currency = Parse(code);
            return true;
        }
        catch
        {
            currency = null;
            return false;
        }
    }

    /// <summary>
    /// Implicitly converts a Currency to string.
    /// </summary>
    public static implicit operator string(Currency currency) => currency.Code;

    /// <summary>
    /// Explicitly converts a string to Currency.
    /// </summary>
    public static explicit operator Currency(string code) => Parse(code);

    /// <summary>
    /// Returns the ISO currency code as a string.
    /// </summary>
    public override string ToString() => Code;

    /// <summary>
    /// Checks if this Currency instance is equal to another object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is Currency other)
            return Code.Equals(other.Code, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    /// <summary>
    /// Returns a hash code for the currency based on its ISO code.
    /// </summary>
    public override int GetHashCode() => Code.GetHashCode();

    /// <summary>
    /// Compares two Currency instances for equality.
    /// </summary>
    public static bool operator ==(Currency left, Currency right) => left.Equals(right);

    /// <summary>
    /// Checks if two Currency instances are not equal.
    /// </summary>
    public static bool operator !=(Currency left, Currency right) => !left.Equals(right);
}