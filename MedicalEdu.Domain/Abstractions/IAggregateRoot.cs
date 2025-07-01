namespace MedicalEdu.Domain.Abstractions;

public interface IAggregateRoot<TId> : IAggregateRoot, IEntity<TId>
{
}

public interface IAggregateRoot : IEntity
{
    /// <summary>
    /// Gets the domain events for the aggregate root.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears the domain events for the aggregate root.
    /// </summary>
    public void ClearDomainEvents();
} 