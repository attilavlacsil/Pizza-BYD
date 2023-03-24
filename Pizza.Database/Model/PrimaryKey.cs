using System.Text.Json.Serialization;

namespace Pizza.Database.Model;

public abstract class PrimaryKey<TKey> where TKey : notnull
{
    public TKey Id { get; set; } = default!;

    protected bool Equals(PrimaryKey<TKey> other)
    {
        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((PrimaryKey<TKey>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TKey>.Default.GetHashCode(Id);
    }

    public static bool operator ==(PrimaryKey<TKey>? left, PrimaryKey<TKey>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(PrimaryKey<TKey>? left, PrimaryKey<TKey>? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"#{Id}";
    }
}