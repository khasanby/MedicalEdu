namespace MedicalEdu.Domain.ValueObjects;

public sealed class Money : IEquatable<Money>
{
    /// <summary>
    /// Gets the amount of money in the specified currency.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Gets the currency of the money (e.g., USD, EUR).
    /// </summary>
    public Currency Currency { get; }

    /// <summary>
    /// Private constructor to enforce the use of factory methods for creation.
    /// </summary>
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    /// <summary>
    /// Creates a new Money instance with the specified amount and currency.
    /// </summary>
    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (currency == null)
            throw new ArgumentException("Currency cannot be null", nameof(currency));

        return new Money(amount, currency);
    }

    /// <summary>
    /// Creates a new Money instance with the specified amount and currency code (default is USD).
    /// </summary>
    public static Money Create(decimal amount, string currencyCode = "USD")
    {
        var currency = Currency.Parse(currencyCode);
        return Create(amount, currency);
    }

    /// <summary>
    /// Creates a Money instance representing zero in the specified currency.
    /// </summary>
    public static Money Zero(Currency currency) => Create(0, currency);

    /// <summary>
    /// Creates a Money instance representing zero in the specified currency code (default is USD).
    /// </summary>
    public static Money Zero(string currencyCode = "USD") => Create(0, currencyCode);

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot add money with different currencies: {Currency.Code} and {other.Currency.Code}");

        return Create(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException($"Cannot subtract money with different currencies: {Currency.Code} and {other.Currency.Code}");

        return Create(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0)
            throw new ArgumentException("Factor cannot be negative", nameof(factor));

        return Create(Amount * factor, Currency);
    }

    /// <summary>
    /// Checks if the Money instance represents zero amount.
    /// </summary>
    public bool IsZero => Amount == 0;

    /// <summary>
    /// Checks if the Money instance is positive or negative.
    /// </summary>
    public bool IsPositive => Amount > 0;

    /// <summary>
    /// Checks if the Money instance is negative.
    /// </summary>
    public bool IsNegative => Amount < 0;


    /// <summary>
    /// Implicitly converts a Money instance to a decimal value representing the amount.
    /// </summary>
    public static implicit operator decimal(Money money) => money.Amount;

    /// <summary>
    /// Returns a string representation of the Money instance, formatted as currency.
    /// </summary>
    public override string ToString() => $"{Amount:C}";

    public bool Equals(Money? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Amount == other.Amount && Currency.Equals(other.Currency);
    }

    /// <summary>
    /// Checks if the current Money instance is equal to another object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        return Equals(obj as Money);
    }

    /// <summary>
    /// Generates a hash code for the Money instance.
    /// </summary>
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency.GetHashCode());
    }

    /// <summary>
    /// Checks if two Money instances are equal.
    /// </summary>
    public static bool operator ==(Money? left, Money? right)
    {
        return EqualityComparer<Money>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two Money instances are not equal.
    /// </summary>
    public static bool operator !=(Money? left, Money? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Adds two Money instances together.
    /// </summary>
    public static Money operator +(Money left, Money right) => left.Add(right);

    /// <summary>
    /// Subtracts one Money instance from another.
    /// </summary>
    public static Money operator -(Money left, Money right) => left.Subtract(right);

    /// <summary>
    /// Multiplies a Money instance by a decimal factor.
    /// </summary>
    public static Money operator *(Money money, decimal factor) => money.Multiply(factor);

    /// <summary>
    /// Multiplies a Money instance by a decimal factor (factor on the left).
    /// </summary>
    public static Money operator *(decimal factor, Money money) => money.Multiply(factor);
}