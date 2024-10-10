using Framework.Core.Domain;
using MassTransit;

namespace Framework.Messaging.Masstransit;

public class MassTransitBusImplementation(IBusControl busControl):IEventBus
{
    public void Publish<TEvent>(TEvent? @event)
    {
        busControl.Publish(@event).GetAwaiter().GetResult();
    }
}