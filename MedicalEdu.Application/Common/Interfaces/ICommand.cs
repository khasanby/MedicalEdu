using MediatR;

namespace MedicalEdu.Application.Common.Interfaces;

/// <summary>
/// Marker interface for commands (write operations).
/// </summary>
public interface ICommand : IRequest
{
}

/// <summary>
/// Generic marker interface for commands that return a response.
/// </summary>
/// <typeparam name="TResponse">The response type.</typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>, ICommand
{
} 