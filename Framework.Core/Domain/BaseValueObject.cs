namespace Framework.Core.Domain;

public abstract class BaseValueObject
{
    public abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if(obj is null)
            return false;

        if (obj as BaseValueObject==null )
            return false;

        return IsEqual(obj as BaseValueObject);
    }

    public virtual bool IsEqual(BaseValueObject? baseValueObject)
    {
        return baseValueObject.GetEqualityComponents().SequenceEqual(this.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Sum(item => item.GetHashCode());
    }

    public static bool operator==(BaseValueObject? a, BaseValueObject? b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(BaseValueObject? a, BaseValueObject? b)
    {
        return !(a == b);
    }
}