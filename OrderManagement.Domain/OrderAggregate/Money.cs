using Framework.Core.Domain;

namespace OrderManagement.Domain.OrderAggregate;

public class Money:BaseValueObject
{
    public decimal Value { get; private set; }

    public Money(decimal value)
    {
        if (value < 0)
            throw new ArgumentException("Money cannot be negative");
        Value = value;
    }

    public static Money FromRialValue(decimal value)
    {
        return new Money(value*10);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}