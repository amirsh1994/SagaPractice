using Framework.Core.Messaging;
using OrderManagement.Domain.Contract.DataContract;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Application.UseCase;

public class RejectOrderCommandHandler(IOrderRepository repository) : IBaseCommandHandler<RejectOrderCommand>
{
    public void Handle(RejectOrderCommand command)
    {
        var order = repository.GetById(command.OrderId);
        order.RejectedOrder();
        repository.Update(order);
    }
}