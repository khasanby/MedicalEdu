using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class Currency
{
    private static readonly Regex CurrencyCodePattern = new(@"^[A-Z]{3}$", RegexOptions.Compiled);
    
    /// <summary>
    /// Gets the ISO currency code (e.g., "USD", "EUR", "GBP").
    /// </summary>
    public string Code { get; }

    private Currency(string code)
    {
        Code = code;
    }

    /// <summary>
    /// Creates a new Currency instance from a valid ISO currency code.
    /// </summary>
    /// <param name="code">The 3-letter ISO currency code (e.g., "USD", "EUR").</param>
    /// <returns>A new Currency instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the currency code is invalid.</exception>
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
    /// <param name="code">The currency code to parse.</param>
    /// <param name="currency">The resulting Currency instance if successful.</param>
    /// <returns>True if parsing was successful; otherwise, false.</returns>
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

    public override string ToString() => Code;

    public override bool Equals(object? obj)
    {
        if (obj is Currency other)
            return Code.Equals(other.Code, StringComparison.OrdinalIgnoreCase);
        return false;
    }

    public override int GetHashCode() => Code.GetHashCode();

    public static bool operator ==(Currency left, Currency right) => left.Equals(right);
    public static bool operator !=(Currency left, Currency right) => !left.Equals(right);
} 