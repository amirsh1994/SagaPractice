using MassTransit;
using OrderManagement.Domain.Contract.DomainEvents;
using OrderManagement.Query.Api.Persistence.Ef;
using OrderManagement.Query.Api.Persistence.mongodb;
using OrderManagement.Query.Api.ReadModel.EfModel;
using OrderManagement.Query.Api.ReadModel.MongodbModels;

namespace OrderManagement.Query.Api.EventHandler;

public class OrderCreatedEventHandlerInOrderReadModel(OrderManagementQueryDbContext dbContext, OrderManagementQueryMongodbContext mongodbContext) : IConsumer<OrderCreatedDomainEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedDomainEvent> context)
    {
        if (AlreadyHandled(context.Message.EventId))
        {
            return;
        }

        #region PersistenceInSqlServer
        var orderReadModel = GetOrderReadModel(context.Message);
        var inBox = GetInboxMessage(context.Message.EventId);
        dbContext.Orders.Add(orderReadModel);
        dbContext.InboxMessages.Add(inBox);
        await dbContext.SaveChangesAsync();


        #endregion


        //#region PersistenceInMongoDb
        //var mongodbInBox = GetMongodbInbox(context.Message.EventId);
        //var orderMongodb = GetOrderMongodb(context.Message);
        //mongodbContext.Orders.Add(orderMongodb);
        //mongodbContext.Messages.Add(mongodbInBox);
        ////mongodbContext.Items.AddRange(orderMongodb.Items);
        //await mongodbContext.SaveChangesAsync();
        //#endregion

    }

    //#region GetMongodbReadModel

    //private OrderMongodb GetOrderMongodb(OrderCreatedDomainEvent contextMessage)
    //{
    //    var orderMongodb = new OrderMongodb()
    //    {
    //        _id = contextMessage.OrderId,
    //        CustomerId = contextMessage.CustomerId,
    //        OrderDate = contextMessage.OrderDate,
    //        CustomerName = "ToMongodb",
    //        NationalCode = "Mongoose",
    //        Items = contextMessage.OrderItemDtos.Select(x => new OrderItemMongodb()
    //        {
    //            ProductId = x.ProductId,
    //            Quantity = x.Quantity,
    //            UnitPrice = x.UnitPrice,

    //        }).ToList()

    //    };
    //    return orderMongodb;
    //}

    //private InboxMessageMongodb GetMongodbInbox(Guid messageId)
    //{
    //    return new InboxMessageMongodb()
    //    {
    //        _id = messageId,
    //        HandledTime = DateTime.Now
    //    };
    //}

    //#endregion


    private bool AlreadyHandled(Guid eventId)
    {
        return dbContext.InboxMessages.Any(x => x.MessageId == eventId);
    }

    private InboxMessage GetInboxMessage(Guid messageEventId)
    {
        var inboxMessage = new InboxMessage()
        {
            MessageId = messageEventId,
            HandledTime = DateTime.Now
        };
        return inboxMessage;
    }

    private Order GetOrderReadModel(OrderCreatedDomainEvent @event)
    {
        var order = new Order
        {
            OrderDate = @event.OrderDate,
            CustomerId = @event.CustomerId,
            OrderId = @event.OrderId,
            CustomerName = "Test",
            NationalCode = "e",
            Items = @event.OrderItemDtos.Select(x => new OrderItem()
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,

            }).ToList()
        };
        return order;
    }
}