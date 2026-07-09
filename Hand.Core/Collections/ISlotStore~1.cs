namespace Hand.Collections;

/// <summary>
/// 插槽存储器
/// </summary>
/// <typeparam name="TValue"></typeparam>
public interface ISlotStore<TValue>
    : ISlotStore
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="value"></param>
    void Save(TValue value);
}
