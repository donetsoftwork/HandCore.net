using Hand.Creational;
using System.Xml;

namespace Hand.ParseXml.Contracts;

/// <summary>
/// 成员收集器
/// </summary>
public interface IMemberParser
{
    /// <summary>
    /// 保存成员
    /// </summary>
    /// <param name="entity">实体构造器</param>
    /// <param name="reader"></param>
    void Save(IMemberStore entity, XmlReader reader);
}
