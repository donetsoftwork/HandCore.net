namespace Hand.Models;

/// <summary>
/// 实体属性
/// </summary>
/// <typeparam name="TProperty"></typeparam>
/// <param name="value"></param>
public abstract class EntityProperty<TProperty>(TProperty value)
    : IEntityProperty<TProperty>
    , IEquatable<EntityProperty<TProperty>>
{
    /// <summary>
    /// 属性实际值
    /// </summary>
    protected TProperty _value = value;
    /// <summary>
    /// 属性实际值
    /// </summary>
    public TProperty Value
    {
        get => _value;
        set => _value = value;
    }
    /// <inheritdoc />
    public override string ToString()
    {
        return _value?.ToString();
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
        var otherValue = other.Value;
        if (_value is null)
        {
            if (otherValue is null)
                return true;
            return false;
        }
        return _value.Equals(otherValue);
    }
    /// <inheritdoc />
    public override int GetHashCode()
    {
        if(_value is null)
            return 0;
        return _value.GetHashCode();
    }
}
