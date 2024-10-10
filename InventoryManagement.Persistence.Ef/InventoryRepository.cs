using InventoryManagement.Domain.StockAggregate;

namespace InventoryManagement.Persistence.Ef;

public class InventoryRepository(InventoryDbContext dbContext):IInventoryRepository
{
    public void SaveChanges(Stock model)
    {
        dbContext.Stocks.Add(model);
        dbContext.SaveChanges();
    }

    public Stock Get(Guid id)
    {
        return dbContext.Stocks.First(x => x.ProductId == id);
    }

    public void Update(Stock model)
    {
        dbContext.Stocks.Update(model);
        dbContext.SaveChanges();
    }
}