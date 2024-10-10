using Framework.Core.Domain;
using Framework.Core.Messaging;
using InventoryManagement.Domain.DataContract.DataContract;
using InventoryManagement.Domain.StockAggregate;

namespace InventoryManagement.Application.UseCase;

public class CreateStockCommandHandler(IInventoryRepository repository,IGuidProvider provider):IBaseCommandHandler<CreateStockCommand>
{
    public void Handle(CreateStockCommand command)
    {
        var stock = Stock.CreateStock(command, provider);
        repository.SaveChanges(stock);
    }
}