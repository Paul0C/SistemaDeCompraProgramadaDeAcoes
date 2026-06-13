using CompraProgramada.Domain.SharedContext;

namespace CompraProgramada.Domain.SharedContext;

public abstract class Entity
{
    public Guid Id { get; protected set; } =  Guid.NewGuid();
    
    private readonly List<IDomainEvent> _domainEvents = new();
    
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
    
    protected Entity()
    {
        
    }
    
    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    public bool Equals(Guid other) => Id == other;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Entity)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(Entity? left, Entity? right) => Equals(left, right);

    public static bool operator !=(Entity? left, Entity? right) => !Equals(left, right);
}