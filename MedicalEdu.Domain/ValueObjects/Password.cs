namespace MedicalEdu.Domain.ValueObjects;

public sealed class PasswordHash : IEquatable<PasswordHash>
{
    public string Hash { get; }

    private PasswordHash(string hash)
    {
        Hash = hash;
    }

    public static PasswordHash Create(string plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            throw new ArgumentException("Password cannot be empty", nameof(plainTextPassword));

        if (plainTextPassword.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long", nameof(plainTextPassword));

        if (!HasRequiredCharacters(plainTextPassword))
            throw new ArgumentException("Password must contain uppercase, lowercase, number, and special character", nameof(plainTextPassword));

        // In a real implementation, this would be hashed using BCrypt or similar
        // For now, we'll just store the plain text (this should be replaced with proper hashing)
        return new PasswordHash(plainTextPassword);
    }

    public static PasswordHash FromHash(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new ArgumentException("Hash cannot be empty", nameof(hash));

        return new PasswordHash(hash);
    }

    private static bool HasRequiredCharacters(string password)
    {
        return password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit) &&
               password.Any(c => !char.IsLetterOrDigit(c));
    }

    public bool Verify(string plainTextPassword)
    {
        // In a real implementation, this would verify against the hash
        // For now, we'll just compare (this should be replaced with proper verification)
        return Hash == plainTextPassword;
    }

    public static implicit operator string(PasswordHash password) => password.Hash;

    public override string ToString() => Hash;

    public bool Equals(PasswordHash? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Hash == other.Hash;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PasswordHash);
    }

    public override int GetHashCode()
    {
        return Hash.GetHashCode();
    }

    public static bool operator ==(PasswordHash? left, PasswordHash? right)
    {
        return EqualityComparer<PasswordHash>.Default.Equals(left, right);
    }

    public static bool operator !=(PasswordHash? left, PasswordHash? right)
    {
        return !(left == right);
    }
} 