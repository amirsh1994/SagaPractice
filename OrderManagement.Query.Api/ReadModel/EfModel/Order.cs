using System.ComponentModel.DataAnnotations;


namespace OrderManagement.Query.Api.ReadModel.EfModel;

public class Order
{
    [Key]
    public Guid OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string NationalCode { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = [];
}

public class OrderItem
{
    [Key]
    public Guid OrderItemId { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Order Order { get; set; }
    public decimal UnitPrice { get; set; }
    public uint Quantity { get; set; }
}

public class InboxMessage
{
    [Key]
    public Guid MessageId { get; set; }
    public DateTime HandledTime { get; set; }
}