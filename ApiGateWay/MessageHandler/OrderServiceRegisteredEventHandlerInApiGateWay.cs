using MassTransit;
using OrderManagement.Api;

namespace ApiGateWay.MessageHandler;

public class OrderServiceRegisteredEventHandlerInApiGateWay:IConsumer<OrderServiceRegisteredEvent>
{
    public Task Consume(ConsumeContext<OrderServiceRegisteredEvent> context)
    {
        try
        {
            ServiceRegistry.AddService(GetRegisterServiceModel(context.Message));
           
        }
        catch (Exception)
        {
           
            throw;
        }
        return Task.CompletedTask;
    }

    private RegisterServiceModel GetRegisterServiceModel(OrderServiceRegisteredEvent @event)
    {
        return new RegisterServiceModel()
        {
            Name = @event.Name,
            Url = @event.Url
        };
    }
}