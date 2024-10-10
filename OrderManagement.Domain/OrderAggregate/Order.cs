using Framework.Core.Domain;
using OrderManagement.Domain.Contract.DataContract;
using OrderManagement.Domain.Contract.DomainEvents;

namespace OrderManagement.Domain.OrderAggregate;

public class Order : AggregateRoot<Guid>
{
    public Guid CustomerId { get; private init; }

    public DateTime OrderDate { get; private init; }

    private  List<OrderItem> _Items = new();

    public IReadOnlyCollection<OrderItem> Items => _Items.AsReadOnly();

    public OrderState State { get; private set; }

    public static decimal MaxSize => 200000;


    public static Order CreateOrder(CreateOrderCommand command, IGuidProvider provider)
    {
        var order = new Order()
        {
            CustomerId = command.CustomerId,
            OrderDate = DateTime.Now,
            State = new PendingState(),
            Id = provider.GetGuid()

        };
        SetItems(command, order);
        order.AddChanges(new OrderCreatedDomainEvent(){OrderId = order.Id,CustomerId = order.CustomerId,OrderDate = order.OrderDate,OrderItemDtos = command.OrderItemDtos,EventId = Guid.NewGuid()});
        return order;
    }

    private static void SetItems(CreateOrderCommand command, Order order)
    {
        var totalPrice =command.OrderItemDtos.Sum(x => x.UnitPrice * x.Quantity);
        if (command.OrderItemDtos.Any(item => totalPrice + item.UnitPrice * item.Quantity >MaxSize))
        {
            throw new Exception("total price is too large");
        }

        order._Items = command.OrderItemDtos.Select(x => OrderItem.CreateOrderItem(x, Guid.NewGuid())).ToList();
        if (!order._Items.Any()|| order._Items.Count==0)
        {
            throw new Exception("order item is empty");
        }

        
    }

    public void ApproveOrder()
    {
        State = State.Approved();
        AddChanges(new ApprovedOrderEvent(){OrderId = Id,EventId = Guid.NewGuid()});
    }

    public void RejectedOrder()
    {
        State = State.Rejected();
        AddChanges(new RejectedOrderEvent() { OrderId = Id, EventId = Guid.NewGuid() });
    }
}

public class OrderItem : BaseEntity<Guid>
{

    public Guid ProductId { get; private set; }

    public Money UnitPrice { get; private set; }

    public uint Quantity { get; private set; }


    public static OrderItem CreateOrderItem(OrderItemDto dto,Guid id)
    {
        return new OrderItem()
        {
            ProductId = dto.ProductId,

            Quantity = dto.Quantity,

            UnitPrice = Money.FromRialValue(dto.UnitPrice),
            Id = id

        };
    }

}