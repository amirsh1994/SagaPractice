namespace OrderManagement.Domain.OrderAggregate;

public interface IOrderRepository
{
    void SaveChanges(Order model);

    Order GetById(Guid id);

    void Update(Order model);
}