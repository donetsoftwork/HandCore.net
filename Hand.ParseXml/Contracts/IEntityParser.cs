namespace Hand.ParseXml.Contracts;

/// <summary>
/// 实体解析器
/// </summary>
public interface IEntityParser
{
    /// <summary>
    /// Xml配置
    /// </summary>
    HandXml Xml { get; }
    /// <summary>
    /// 添加属性
    /// </summary>
    /// <param name="attribute"></param>
    void AddAttribute(IMemberParser attribute);
    /// <summary>
    /// 添加子节点
    /// </summary>
    /// <param name="name"></param>
    /// <param name="child"></param>
    void AddItem(string name, IMemberParser child);
}
