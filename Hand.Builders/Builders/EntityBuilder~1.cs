using Hand.Collections;
using Hand.Creational;
using Hand.Members;
using System.Reflection;

namespace Hand.Builders;

/// <summary>
/// 默认实体建造者
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <param name="constructor"></param>
/// <param name="parameterNames"></param>
/// <param name="parameters"></param>
/// <param name="properties"></param>
/// <param name="fields"></param>
public class EntityBuilder<TEntity>(ConstructorInfo constructor, IDictionary<string, int> parameterNames, object[] parameters, IDictionary<string, PropertyInfo> properties, IDictionary<string, FieldInfo> fields)
    : IMemberBuilder<TEntity>
{
    #region 配置
    private readonly ConstructorInfo _constructor = constructor;
    private readonly IDictionary<string, int> _parameterNames = parameterNames;
    private readonly object[] _parameters = parameters;
    private readonly IDictionary<string, PropertyInfo> _properties = properties;
    private readonly IDictionary<string, FieldInfo> _fields = fields;
    private readonly List<Action<object>> _initializers = [];
    #endregion

    /// <inheritdoc />
    public virtual TEntity Build()
    {
        var instance = _constructor.Invoke(_parameters);
        foreach (var initializer in _initializers)
        {
            initializer(instance);
        }
        return (TEntity)instance;
    }
    ///// <inheritdoc />
    //public virtual ISlotStore<TMember>? GetSlot<TMember>(string memberName)
    //{
    //    if (_parameterNames.TryGetValue(memberName, out var index))
    //        return new IndexStore<TMember>(index, _parameters);
    //    if(_properties.TryGetValue(memberName, out var property))
    //        return new PropertyStore<TMember>(property, _initializers);
    //    if (_fields.TryGetValue(memberName, out var field))
    //        return new FieldStore<TMember>(field, _initializers);
    //    return null;
    //}
    /// <inheritdoc />
    public void Save<TMember>(string name, TMember value)
    {
        if (_parameterNames.TryGetValue(name, out var index))
            _parameters[index] = value!;
        if (_properties.TryGetValue(name, out var property))
            _initializers.Add(instance => property.SetValue(instance, value));
        if (_fields.TryGetValue(name, out var field))
            _initializers.Add(instance => field.SetValue(instance, value));
    }
}
