namespace Hand.ParseJson.Contracts;

/// <summary>
/// 实体解析器
/// </summary>
public interface IEntityParser
{
    /// <summary>
    /// json配置
    /// </summary>
    HandJson Json { get; }
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="name"></param>
    /// <param name="property"></param>
    void AddProperty(string name, IMemberParser property);
}
