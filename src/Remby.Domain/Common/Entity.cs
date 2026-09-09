namespace ECommerce.Domain.Common;

public abstract class Entity<TId>
{
    public TId? Id { get; protected init; }

    protected Entity(TId id)
    {
        Id = id;
    }

    protected Entity() { }
    
    public override bool Equals(object? obj)
    {
        if(obj is null) 
            return false;
        
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            
            return true;
        
        return Id!.Equals(other.Id);
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return Id!.GetHashCode();
    }

    public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
    {
        return !(a == b);
    }
}