using Hand.Collections;
using System.Reflection;

namespace Hand.Members;

/// <summary>
/// 属性存储器
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="property"></param>
/// <param name="initializers"></param>
public class PropertyStore<TMember>(PropertyInfo property, ICollection<Action<object>> initializers)
    : ISlotStore<TMember>
{
    private readonly ICollection<Action<object>> _initializers = initializers;
    private readonly PropertyInfo _property = property;

    /// <inheritdoc />
    public void Save(object value)
    {
        void initializer(object instance) 
            => _property.SetValue(instance, value);
        _initializers.Add(initializer);
    }
    /// <inheritdoc />
    void ISlotStore<TMember>.Save(TMember value)
        => Save(value!);
}