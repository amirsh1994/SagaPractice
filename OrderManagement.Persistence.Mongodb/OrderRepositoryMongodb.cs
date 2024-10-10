using MongoDB.Driver;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Persistence.Mongodb;

public class OrderRepositoryMongodb(MongoClient client):IOrderRepository
{
    
    public void SaveChanges(Order model)
    {
        var db = MongodbOrderDbContext.Create(client.GetDatabase("SagaProject"));
        db.Orders.Add(model);
        db.SaveChanges();
    }

    public Order GetById(Guid id)
    {
        var db = MongodbOrderDbContext.Create(client.GetDatabase("SagaProject"));
        return db.Orders.First(x => x.Id == id);
    }

    public void Update(Order model)
    {
        var db = MongodbOrderDbContext.Create(client.GetDatabase("SagaProject"));
        db.Update(model);
    }
}