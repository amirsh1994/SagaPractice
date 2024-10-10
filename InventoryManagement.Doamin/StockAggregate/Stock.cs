using Framework.Core.Domain;
using InventoryManagement.Domain.DataContract.DataContract;

namespace InventoryManagement.Domain.StockAggregate;

public class Stock:AggregateRoot<Guid>
{
    public Guid ProductId { get; private set; }
    public uint Quantity { get; private set; }

    public static Stock CreateStock(CreateStockCommand command,IGuidProvider provider)
    {
        var stock = new Stock()
        {
            Quantity = command.Quantity,
            ProductId = command.ProductId,
            Id = provider.GetGuid()

        };
        return stock;
    }

    public void AdjustmentQuantity(uint quantity)
    {
        if (quantity>this.Quantity)
        {
            throw new Exception("Quantity is not enough");
        }
        this.Quantity -= quantity;
    }
}