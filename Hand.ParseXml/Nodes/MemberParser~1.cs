using Hand.Convert;
using Hand.Creational;
using Hand.ParseXml.Contracts;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 成员处理器
/// </summary>
/// <typeparam name="TMember"></typeparam>
/// <param name="name"></param>
/// <param name="original"></param>
public class MemberParser<TMember>(string name, IParser<XmlReader, TMember> original)
    : IMemberParser
{
    #region 配置
    private readonly string _name = name;
    private readonly IParser<XmlReader, TMember> _original = original;

    /// <summary>
    /// 成员名
    /// </summary>
    public string Name
        => _name;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IParser<XmlReader, TMember> Original 
        => _original;
    #endregion

    /// <inheritdoc />
    public void Save(IMemberStore entity, XmlReader reader)
    {
        if (_original.TryParse(reader, out var value))
            entity.Save(_name, value);
    }
}
