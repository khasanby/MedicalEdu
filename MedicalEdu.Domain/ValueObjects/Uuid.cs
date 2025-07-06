namespace MedicalEdu.Domain.ValueObjects;

public sealed class Uuid : IEquatable<Uuid>
{
    /// <summary>
    /// Gets the underlying Guid value.
    /// </summary>
    public Guid Value { get; }

    /// <summary>
    /// By making the constructor private, we ensure that Uuid instances can only be created through the provided static methods.
    /// </summary>
    private Uuid(Guid value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Uuid instance with a new random GUID.
    /// </summary>
    public static Uuid Create()
    {
        return new Uuid(Guid.NewGuid());
    }

    /// <summary>
    /// Creates a new Uuid instance from an existing GUID.
    /// </summary>
    public static Uuid Create(Guid guid)
    {
        if (guid == Guid.Empty)
            throw new ArgumentException("GUID cannot be empty", nameof(guid));

        return new Uuid(guid);
    }

    /// <summary>
    /// Creates a new Uuid instance from a GUID string.
    /// </summary>
    public static Uuid Create(string guidString)
    {
        if (string.IsNullOrWhiteSpace(guidString))
            throw new ArgumentException("GUID string cannot be empty", nameof(guidString));

        if (!Guid.TryParse(guidString, out var guid))
            throw new ArgumentException("Invalid GUID format", nameof(guidString));

        return Create(guid);
    }

    /// <summary>
    /// Attempts to create a Uuid instance from a GUID string.
    /// </summary>
    public static bool TryCreate(string guidString, out Uuid? result)
    {
        try
        {
            result = Create(guidString);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// Attempts to create a Uuid instance from a GUID.
    /// </summary>
    public static bool TryCreate(Guid guid, out Uuid? result)
    {
        try
        {
            result = Create(guid);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    /// <summary>
    /// Creates a new Uuid instance from a byte array.
    /// </summary>
    public static Uuid Create(byte[] bytes)
    {
        if (bytes == null || bytes.Length != 16)
            throw new ArgumentException("Byte array must be exactly 16 bytes", nameof(bytes));

        var guid = new Guid(bytes);
        return Create(guid);
    }

    /// <summary>
    /// Converts the Uuid to a byte array.
    /// </summary>
    public byte[] ToByteArray()
    {
        return Value.ToByteArray();
    }

    /// <summary>
    /// Converts the Uuid to a string in the specified format.
    /// </summary>
    public string ToString(string format)
    {
        return Value.ToString(format);
    }

    /// <summary>
    /// Implicitly converts the Uuid to a Guid.
    /// </summary>
    public static implicit operator Guid(Uuid uuid) => uuid.Value;

    /// <summary>
    /// Explicitly converts a Guid to Uuid.
    /// </summary>
    public static explicit operator Uuid(Guid guid) => Create(guid);

    /// <summary>
    /// Returns the string representation of the Uuid.
    /// </summary>
    public override string ToString() => Value.ToString();

    public bool Equals(Uuid? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Uuid);
    }

    /// <summary>
    /// Returns the hash code for the Uuid.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two Uuids are equal.
    /// </summary>
    public static bool operator ==(Uuid? left, Uuid? right)
    {
        return EqualityComparer<Uuid>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two Uuids are not equal.
    /// </summary>
    public static bool operator !=(Uuid? left, Uuid? right)
    {
        return !(left == right);
    }
}