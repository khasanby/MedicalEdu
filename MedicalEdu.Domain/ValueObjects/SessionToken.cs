using System.Security.Cryptography;
using System.Text;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class SessionToken : IEquatable<SessionToken>
{
    private const int TokenLength = 64;
    private static readonly string ValidCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
    /// <summary>
    /// Gets the session token value.
    /// </summary>
    public string Value { get; }

    private SessionToken(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new SessionToken instance with a cryptographically secure random token.
    /// </summary>
    /// <returns>A new SessionToken instance.</returns>
    public static SessionToken Create()
    {
        return new SessionToken(GenerateSecureToken());
    }

    /// <summary>
    /// Creates a new SessionToken instance from an existing token string.
    /// </summary>
    /// <param name="token">The token string to validate and create.</param>
    /// <returns>A new SessionToken instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the token format is invalid.</exception>
    public static SessionToken Create(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Session token cannot be empty", nameof(token));

        if (!IsValidToken(token))
            throw new ArgumentException($"Session token must be {TokenLength} characters long and contain only alphanumeric characters", nameof(token));

        return new SessionToken(token);
    }

    /// <summary>
    /// Attempts to create a SessionToken instance from a string.
    /// </summary>
    /// <param name="token">The token to parse.</param>
    /// <param name="result">The resulting SessionToken instance if successful.</param>
    /// <returns>True if parsing was successful; otherwise, false.</returns>
    public static bool TryCreate(string token, out SessionToken? result)
    {
        try
        {
            result = Create(token);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private static string GenerateSecureToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[TokenLength];
        rng.GetBytes(bytes);
        
        var token = new StringBuilder(TokenLength);
        for (int i = 0; i < TokenLength; i++)
        {
            token.Append(ValidCharacters[bytes[i] % ValidCharacters.Length]);
        }
        
        return token.ToString();
    }

    private static bool IsValidToken(string token)
    {
        if (token.Length != TokenLength)
            return false;

        return token.All(c => ValidCharacters.Contains(c));
    }

    /// <summary>
    /// Gets the token length.
    /// </summary>
    public static int Length => TokenLength;

    /// <summary>
    /// Checks if the token is valid.
    /// </summary>
    /// <returns>True if the token is valid; otherwise, false.</returns>
    public bool IsValid => IsValidToken(Value);

    /// <summary>
    /// Implicitly converts the session token to a string.
    /// </summary>
    public static implicit operator string(SessionToken sessionToken) => sessionToken.Value;

    /// <summary>
    /// Returns the string representation of the session token.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(SessionToken? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as SessionToken);
    }

    /// <summary>
    /// Returns the hash code for the session token.
    /// </summary>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Checks if two session tokens are equal.
    /// </summary>
    public static bool operator ==(SessionToken? left, SessionToken? right)
    {
        return EqualityComparer<SessionToken>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two session tokens are not equal.
    /// </summary>
    public static bool operator !=(SessionToken? left, SessionToken? right)
    {
        return !(left == right);
    }
} 