using Hand.Convert;
using Hand.Structural;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 包装解析器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="xml"></param>
/// <param name="original"></param>
public abstract class WrapParser<TValue>(HandXml xml, IParser<XmlReader, TValue> original)
    : IWrapper<IParser<XmlReader, TValue>>
{
    #region 配置
    /// <summary>
    /// xml解析器
    /// </summary>
    protected readonly HandXml _xml = xml;
    /// <summary>
    /// 原始解析器
    /// </summary>
    protected readonly IParser<XmlReader, TValue> _original = original;
    /// <inheritdoc />
    public HandXml Xml
        => _xml;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IParser<XmlReader, TValue> Original
        => _original;
    #endregion
}
