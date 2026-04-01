namespace Hand.Models;

/// <summary>
/// 实体属性
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <param name="original"></param>
public abstract class EntityProperty<TProperty>(TProperty original)
    : IEntityProperty<TProperty>
    , IEquatable<EntityProperty<TProperty>>
{
    /// <summary>
    /// 属性原始值
    /// </summary>
    protected TProperty _original = original;
    /// <summary>
    /// 属性原始值
    /// </summary>
    public TProperty Original
    {
        get => _original;
        set => _original = value;
    }
    /// <inheritdoc />
    public override string ToString()
    {
        return _original?.ToString();
    }
    /// <inheritdoc />
    public override bool Equals(object obj)
        => obj is EntityProperty<TProperty> entityProperty && Equals(entityProperty);
#nullable enable
    /// <inheritdoc />
    public bool Equals(EntityProperty<TProperty>? other)
#nullable disable
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        var otherValue = other.Original;
        if (_original is null)
        {
            if (otherValue is null)
                return true;
            return false;
        }
        return _original.Equals(otherValue);
    }
    /// <inheritdoc />
    public override int GetHashCode()
    {
        if(_original is null)
            return 0;
        return _original.GetHashCode();
    }
}
