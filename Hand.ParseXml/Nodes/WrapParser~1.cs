using Hand.ParseXml.Contracts;
using Hand.Structural;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 包装解析器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="xml"></param>
/// <param name="original"></param>
public abstract class WrapParser<TValue>(HandXml xml, IXmlParser<TValue> original)
    : IWrapper<IXmlParser<TValue>>
{
    #region 配置
    /// <summary>
    /// xml解析器
    /// </summary>
    protected readonly HandXml _xml = xml;
    /// <summary>
    /// 原始解析器
    /// </summary>
    protected readonly IXmlParser<TValue> _original = original;
    /// <inheritdoc />
    public HandXml Xml
        => _xml;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IXmlParser<TValue> Original
        => _original;
    #endregion
}
