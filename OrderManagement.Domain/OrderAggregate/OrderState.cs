namespace OrderManagement.Domain.OrderAggregate;

public abstract class OrderState
{
    public virtual OrderState Pending() => throw new NotImplementedException();
    public virtual OrderState Approved() => throw new NotImplementedException();
    public virtual OrderState Paid() => throw new NotImplementedException();
    public virtual OrderState Rejected() => throw new NotImplementedException();
    public virtual OrderState Delivered() => throw new NotImplementedException();
}


public class PendingState : OrderState
{
    public PendingState()
    {
        
    }
    public override OrderState Approved()
    {
        return new ApprovedState();
    }

    public override OrderState Rejected()
    {
        return new RejectedState();
    }
}

public class ApprovedState : OrderState
{

}

public class RejectedState : OrderState
{

}