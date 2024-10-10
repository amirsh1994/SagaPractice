using Framework.Core.Messaging;

namespace OrderManagement.Domain.Contract.DataContract;

public class CreateOrderCommand:IBaseCommand
{
    public Guid CustomerId { get;  set; }
    public DateTime OrderDate { get;  set; }
    public List<OrderItemDto> OrderItemDtos { get; set; } = new();

   
    public bool Validate()
    {
        return true;
    }
}

public class OrderItemDto
{
    public Guid ProductId { get;  set; }
    public decimal UnitPrice { get;  set; }
    public uint Quantity { get;  set; }

}