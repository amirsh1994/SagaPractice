namespace InventoryManagement.Domain.StockAggregate;

public interface IInventoryRepository
{
    void SaveChanges(Stock model);

    Stock Get(Guid id);

    void Update(Stock model);
}