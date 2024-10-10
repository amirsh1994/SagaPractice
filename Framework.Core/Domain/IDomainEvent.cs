using System.ComponentModel.DataAnnotations;

namespace Framework.Core.Domain;

public interface IDomainEvent
{
    public Guid EventId { get; protected set; }
}

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetChanges();
}

public class AggregateRoot<TKey>:BaseEntity<TKey>,IAggregateRoot
{
    private List<IDomainEvent> _changes = [];

    public IReadOnlyCollection<IDomainEvent> GetChanges() => _changes.AsReadOnly();

    public void AddChanges(IDomainEvent @event)
    {
        _changes.Add(@event);
    }

    [Timestamp]
    public byte[] RowVersion { get; set; }

}

public class BaseEntity<TKey>
{
    public TKey  Id { get;protected set; }

    public override bool Equals(object? obj)
    {
        var other = obj as BaseEntity<TKey>;

        return Id!.Equals(other!.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}

public interface IGuidProvider
{
    Guid GetGuid();

}
public class DefaultGuidProvider : IGuidProvider
{
    public Guid GetGuid()
    {
        return Guid.NewGuid();
    }
}

public interface IEventBus
{
    void Publish<TEvent>(TEvent? @event);
}


