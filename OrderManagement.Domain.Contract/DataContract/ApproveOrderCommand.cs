using Framework.Core.Messaging;

namespace OrderManagement.Domain.Contract.DataContract;

public class ApproveOrderCommand:IBaseCommand
{
    public Guid OrderId { get; set; }
    public bool Validate()
    {
        return true;
    }
}

public class RejectOrderCommand : IBaseCommand
{
    public Guid OrderId { get; set; }
    public bool Validate()
    {
        return true;
    }
}