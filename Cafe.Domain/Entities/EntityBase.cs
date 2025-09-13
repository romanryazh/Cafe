namespace Cafe.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }

    protected EntityBase()
    {
    }

    protected EntityBase(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not EntityBase other) 
            return false;
        
        if (ReferenceEquals(this, other)) 
            return true;
        
        if (other.GetType() != GetType()) 
            return false;
        
        if (other.Id == Guid.Empty || Id == Guid.Empty)
            return false;
        
        return Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(EntityBase? left, EntityBase? right)
    {
        if (left is null && right is null)
            return true;
        
        if (left is null || right is null)
            return false;
        
        return left.Equals(right);
    }

    public static bool operator !=(EntityBase? left, EntityBase? right) => !(left == right);
}