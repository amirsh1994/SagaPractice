using Framework.Core.Messaging;
using InventoryManagement.Domain.DataContract.DataContract;
using InventoryManagement.Domain.DataContract.DomainEvents;
using InventoryManagement.Domain.StockAggregate;
using InventoryManagement.Persistence.Ef;
using InventoryManagement.Persistence.Mongodb.MongodbInBoxModel;
using MassTransit;
using MongoDB.Driver;
using OrderManagement.Domain.Contract.DomainEvents;
using static InventoryManagement.Domain.DataContract.DomainEvents.AdjustmentQuantityProceedSuccess;

namespace InventoryManagement.Api.EventHandler;

public class OrderCreatedEventHandlerInInventory(ICommandBuss commandBuss, InventoryDbContext dbContext,IMongoClient client) : IConsumer<OrderCreatedDomainEvent>
{
    public Task Consume(ConsumeContext<OrderCreatedDomainEvent> context)
    {
        if (IsAlreadyHandled(context.Message.EventId))
        {
            return Task.CompletedTask;
        }
        var orderItems = context.Message.OrderItemDtos;

        try
        {
            foreach (var item in orderItems)
            {
                commandBuss.Send(new AdjustQuantityCommand() { Quantity = item.Quantity, ProductId = item.ProductId });
            }
        }
        catch (Exception ex)
        {
            context.Publish(new AdjustmentQuantityProceedFailed() { OrderId = context.Message.OrderId, EventId = Guid.NewGuid() });
            throw;
        }

        context.Publish(new AdjustmentQuantityProceedSuccess()
        { OrderId = context.Message.OrderId, EventId = Guid.NewGuid() });

        var inbox = GetInBox(context.Message.EventId);
        dbContext.Messages.Add(inbox);
        dbContext.SaveChanges();


        //var inboxMongodb = GetInBoxMongodb(context.Message.EventId); 
        //var database = client.GetDatabase("InventoryManagementPersistence");
        //var mongodbContext=MongodbContextInventory.Create(database);
        //mongodbContext.Messages.Add(inboxMongodb);
        //mongodbContext.SaveChanges();

        

        return Task.CompletedTask;
    }

    private InBoxMessageFromMongodb GetInBoxMongodb(Guid messageEventId)
    {
        return new InBoxMessageFromMongodb()
        {
            _id = messageEventId,
            HandledTime = DateTime.Now
        };
    }

    private bool IsAlreadyHandled(Guid messageId)
    {
        return dbContext.Messages.Any(x => x.MessageId == messageId);
    }

    private InBoxMessage GetInBox(Guid messageEventId)
    {
        var inbox = new InBoxMessage()
        {
            MessageId = messageEventId,
            HandledTime = DateTime.Now
        };
        return inbox;
    }
}