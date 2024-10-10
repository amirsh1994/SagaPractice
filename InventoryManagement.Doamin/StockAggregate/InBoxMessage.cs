using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Domain.StockAggregate;

public class InBoxMessage
{
    [Key]
    public Guid MessageId { get; set; }
    public DateTime HandledTime { get; set; }
    
}