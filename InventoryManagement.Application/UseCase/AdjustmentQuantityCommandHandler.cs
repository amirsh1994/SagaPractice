using Framework.Core.Messaging;
using InventoryManagement.Domain.DataContract.DataContract;
using InventoryManagement.Domain.StockAggregate;

namespace InventoryManagement.Application.UseCase;

public class AdjustmentQuantityCommandHandler(IInventoryRepository repository):IBaseCommandHandler<AdjustQuantityCommand>
{
    public void Handle(AdjustQuantityCommand command)
    {
        var stock = repository.Get(command.ProductId);
        stock.AdjustmentQuantity(command.Quantity);
        repository.Update(stock);
    }
}