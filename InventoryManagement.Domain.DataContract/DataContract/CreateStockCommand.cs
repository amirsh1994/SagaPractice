using Framework.Core.Messaging;

namespace InventoryManagement.Domain.DataContract.DataContract;

public class CreateStockCommand:IBaseCommand
{

    public Guid ProductId { get;  set; }
    public uint Quantity { get;  set; }
    public bool Validate()
    {
        return true;
    }
}