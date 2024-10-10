using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Persistence.Ef;

public class OrderRepositoryEf(OrderManagementDbContext dbContext):IOrderRepository
{
    public void SaveChanges(Order model)
    {
        dbContext.Orders.Add(model);
        dbContext.SaveChanges();

    }

    public Order GetById(Guid id)
    {
        return dbContext.Orders.First(x => x.Id == id);
    }

    public void Update(Order model)
    {
        dbContext.Orders.Update(model);
        dbContext.SaveChanges();
    }
}