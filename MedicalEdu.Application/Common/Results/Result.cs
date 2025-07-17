using System.Diagnostics.CodeAnalysis;

namespace MedicalEdu.Application.Common.Results;

public sealed class Result<T>
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets the value if the operation was successful, otherwise null.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Gets the error message if the operation failed, otherwise empty list.
    /// </summary>
    public IReadOnlyList<string> Errors { get; }


    private Result(bool isSuccess, T? value = default, IReadOnlyList<string> errors = null)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public bool TryGetValue([NotNullWhen(true)] out T? value)
    {
        value = IsSuccess ? Value : default;
        return IsSuccess;
    }


    public static Result<T> Success(T value) => new(true, value, Array.Empty<string>());

    public static Result<T> Failure(params string[] errors) => new(false, errors: errors);

    public static Result<T> NotFound(string errorMessage = "Entity Not Found") => Failure(errorMessage);

    public static Result<T> Unauthorized(string errorMessage = "Unauthorized access") => Failure(errorMessage);

    public static Result<T> Conflict(string errorMessage = "Entity already exists") => Failure(errorMessage);
}