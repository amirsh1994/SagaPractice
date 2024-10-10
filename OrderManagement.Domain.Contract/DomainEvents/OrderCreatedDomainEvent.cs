using Framework.Core.Domain;
using OrderManagement.Domain.Contract.DataContract;

namespace OrderManagement.Domain.Contract.DomainEvents;

public class OrderCreatedDomainEvent:IDomainEvent
{
    public Guid EventId { get; set; }
    public Guid OrderId { get; set; }
    public Guid  CustomerId { get;  set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItemDto> OrderItemDtos { get; set; }

   
}