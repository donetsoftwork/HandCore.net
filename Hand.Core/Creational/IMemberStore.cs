namespace Hand.Creational;

/// <summary>
/// 成员存储器
/// </summary>
public interface IMemberStore
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    void Save<TMember>(string name, TMember value);
}
