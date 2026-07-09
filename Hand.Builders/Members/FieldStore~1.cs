using Hand.Collections;
using System.Reflection;

namespace Hand.Members;

/// <summary>
/// 字段存储器
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="field"></param>
/// <param name="initializers"></param>
public class FieldStore<TMember>(FieldInfo field, ICollection<Action<object>> initializers)
    : ISlotStore<TMember>
{
    private readonly ICollection<Action<object>> _initializers = initializers;
    private readonly FieldInfo _field = field;

    /// <inheritdoc />
    public void Save(object value)
    {
        void initializer(object instance) 
            => _field.SetValue(instance, value);

        _initializers.Add(initializer);
    }
    /// <inheritdoc />
    void ISlotStore<TMember>.Save(TMember value)
        => Save(value!);
}
