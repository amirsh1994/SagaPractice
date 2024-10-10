using Framework.Core.Messaging;
using InventoryManagement.Domain.DataContract.DomainEvents;
using MassTransit;
using OrderManagement.Domain.Contract.DataContract;
using static InventoryManagement.Domain.DataContract.DomainEvents.AdjustmentQuantityProceedSuccess;

namespace OrderManagement.Api.EventHandler;

public class OrderQuantityProceedFailedEventHandlerFromOrderManagement(ICommandBuss commandBuss):IConsumer<AdjustmentQuantityProceedFailed>
{
    public Task Consume(ConsumeContext<AdjustmentQuantityProceedFailed> context)
    {
        commandBuss.Send(new RejectOrderCommand(){OrderId = context.Message.OrderId});
        return Task.CompletedTask;
    }
}


public class OrderQuantityProceedSuccessEventHandlerFromOrderManagement(ICommandBuss commandBuss) : IConsumer<AdjustmentQuantityProceedSuccess>
{
    public Task Consume(ConsumeContext<AdjustmentQuantityProceedSuccess> context)
    {
        commandBuss.Send(new ApproveOrderCommand() { OrderId = context.Message.OrderId });
        return Task.CompletedTask;
    }
}