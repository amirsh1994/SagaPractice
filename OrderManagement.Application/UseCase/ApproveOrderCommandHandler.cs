using Framework.Core.Messaging;
using OrderManagement.Domain.Contract.DataContract;
using OrderManagement.Domain.OrderAggregate;

namespace OrderManagement.Application.UseCase;

public class ApproveOrderCommandHandler(IOrderRepository repository):IBaseCommandHandler<ApproveOrderCommand>
{
    public void Handle(ApproveOrderCommand command)
    {
        var order=repository.GetById(command.OrderId);
        order.ApproveOrder();
        repository.Update(order);
    }
}