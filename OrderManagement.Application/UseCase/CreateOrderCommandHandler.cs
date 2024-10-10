using Framework.Core.Domain;
using Framework.Core.Messaging;
using OrderManagement.Domain.Contract.DataContract;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Application.UseCase;

public class CreateOrderCommandHandler(IOrderRepository repository,IGuidProvider provider): IBaseCommandHandler<CreateOrderCommand>
{
    public void Handle(CreateOrderCommand command)
    {
        var order = Order.CreateOrder(command, provider);
        repository.SaveChanges(order);
    }
}