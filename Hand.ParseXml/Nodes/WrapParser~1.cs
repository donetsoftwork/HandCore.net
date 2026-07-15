using Hand.Convert;
using Hand.Structural;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 包装解析器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="original"></param>
public abstract class WrapParser<TValue>(IParser<XmlReader, TValue> original)
    : IWrapper<IParser<XmlReader, TValue>>
{
    #region 配置
    /// <summary>
    /// 原始解析器
    /// </summary>
    protected readonly IParser<XmlReader, TValue> _original = original;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IParser<XmlReader, TValue> Original
        => _original;
    #endregion
    /// <summary>
    /// 移入子节点
    /// </summary>
    /// <param name="reader"></param>
    /// <returns></returns>
    protected virtual bool Move(XmlReader reader)
    {
        //reader.ReadToFollowing
        //reader.ReadToDescendant()
        var depth = reader.Depth;
        while (reader.Read() && (reader.Depth == depth || reader.NodeType is XmlNodeType.None or XmlNodeType.Whitespace or XmlNodeType.Comment or XmlNodeType.EndElement)) ;
        return reader.Depth > depth;
    }
}
