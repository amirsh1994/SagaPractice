using Framework.Core.Domain;

namespace OrderManagement.Domain.Contract.DomainEvents;

public class ApprovedOrderEvent : IDomainEvent
{
    public Guid EventId { get; set; }
    public Guid OrderId { get; set; }

}

public class RejectedOrderEvent : IDomainEvent
{
    public Guid EventId { get; set; }
    public Guid OrderId { get; set; }

}