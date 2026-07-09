using Hand.Collections;

namespace Hand.Members;

/// <summary>
/// 索引存储器
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="index"></param>
/// <param name="owner"></param>
public class IndexStore<TMember>(int index, object[] owner)
    : ISlotStore<TMember>
{
    private readonly int _index = index;
    private readonly object[] _owner = owner;

    /// <inheritdoc />
    public void Save(object value)
        => _owner[_index] = value!;
    /// <inheritdoc />
    void ISlotStore<TMember>.Save(TMember value)
        => Save(value!);
}
