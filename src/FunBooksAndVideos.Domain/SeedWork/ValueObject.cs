namespace FunBooksAndVideos.Domain.SeedWork;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public static bool operator ==(ValueObject obj1, ValueObject obj2)
    {
        if (obj1 is null ^ obj2 is null)
        {
            return false;
        }

        return obj1?.Equals(obj2) != false;
    }

    public static bool operator !=(ValueObject obj1, ValueObject obj2)
    {
        return !(obj1 == obj2);
    }

    public bool Equals(ValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        if (obj is ValueObject other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents().Aggregate(1, (current, component) =>
        {
            unchecked
            {
                return (current * 23) + (component?.GetHashCode() ?? 0);
            }
        });
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}
