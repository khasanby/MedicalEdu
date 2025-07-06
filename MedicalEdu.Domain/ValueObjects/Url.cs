using System.Text.RegularExpressions;

namespace MedicalEdu.Domain.ValueObjects;

public sealed class Url : IEquatable<Url>
{
    private static readonly Regex UrlPattern = new(
        @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    /// <summary>
    /// Gets the URL value.
    /// </summary>
    public string Value { get; }

    private Url(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Url instance from a valid URL string.
    /// </summary>
    public static Url Create(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be empty", nameof(url));

        var normalized = NormalizeUrl(url);

        if (!IsValidUrl(normalized))
            throw new ArgumentException("Invalid URL format. Must be a valid HTTP/HTTPS URL", nameof(url));

        return new Url(normalized);
    }

    /// <summary>
    /// Attempts to create a Url instance from a string.
    /// </summary>
    public static bool TryCreate(string url, out Url? result)
    {
        try
        {
            result = Create(url);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    private static string NormalizeUrl(string url)
    {
        var trimmed = url.Trim();

        // If no scheme is provided, assume HTTPS
        if (!trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            trimmed = "https://" + trimmed;
        }

        return trimmed;
    }

    private static bool IsValidUrl(string url)
    {
        return UrlPattern.IsMatch(url) && Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    /// <summary>
    /// Gets the scheme of the URL (http or https).
    /// </summary>
    public string GetScheme()
    {
        var uri = new Uri(Value);
        return uri.Scheme;
    }

    /// <summary>
    /// Gets the host of the URL.
    /// </summary>
    public string GetHost()
    {
        var uri = new Uri(Value);
        return uri.Host;
    }

    /// <summary>
    /// Gets the path of the URL.
    /// </summary>
    public string GetPath()
    {
        var uri = new Uri(Value);
        return uri.AbsolutePath;
    }

    /// <summary>
    /// Gets the query string of the URL.
    /// </summary>
    public string GetQuery()
    {
        var uri = new Uri(Value);
        return uri.Query;
    }

    /// <summary>
    /// Checks if the URL uses HTTPS.
    /// </summary>
    public bool IsSecure => GetScheme().Equals("https", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Implicitly converts the URL to a string.
    /// </summary>
    public static implicit operator string(Url url) => url.Value;

    /// <summary>
    /// Returns the string representation of the URL.
    /// </summary>
    public override string ToString() => Value;

    public bool Equals(Url? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Url);
    }

    /// <summary>
    /// Returns the hash code for the URL.
    /// </summary>
    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    /// <summary>
    /// Checks if two URLs are equal.
    /// </summary>
    public static bool operator ==(Url? left, Url? right)
    {
        return EqualityComparer<Url>.Default.Equals(left, right);
    }

    /// <summary>
    /// Checks if two URLs are not equal.
    /// </summary>
    public static bool operator !=(Url? left, Url? right)
    {
        return !(left == right);
    }
}