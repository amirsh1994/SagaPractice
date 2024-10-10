using Framework.Core.Domain;

namespace InventoryManagement.Domain.DataContract.DomainEvents;

public class AdjustmentQuantityProceedSuccess:IDomainEvent
{
    public Guid EventId { get; set; }
    public Guid OrderId { get; set; }

    public class AdjustmentQuantityProceedFailed : IDomainEvent
    {
        public Guid EventId { get; set; }
        public Guid OrderId { get; set; }
    }
}