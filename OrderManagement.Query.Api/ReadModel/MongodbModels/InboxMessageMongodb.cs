using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Query.Api.ReadModel.MongodbModels;

public class InboxMessageMongodb
{
    [BsonId]
    public Guid _id { get; set; }
    public DateTime HandledTime { get; set; }
}

public class OrderMongodb
{
    [BsonId]
    public Guid _id { get; set; }

    public DateTime OrderDate { get; set; }

    public Guid CustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string NationalCode { get; set; } = string.Empty;

    public List<OrderItemMongodb> Items { get; set; } = [];
}

public class OrderItemMongodb
{

    public Guid OrderItemId { get; set; }

    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public uint Quantity { get; set; }
}