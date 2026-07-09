namespace Hand.Collections;

/// <summary>
/// 插槽存储器
/// </summary>
public interface ISlotStore
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="value"></param>
    void Save(object value);
}
