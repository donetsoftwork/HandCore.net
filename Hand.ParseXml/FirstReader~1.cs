using Hand.Convert;
using Hand.ParseXml.Nodes;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// 第一个节点读取器
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class FirstReader<TResult>(IParser<XmlReader, TResult> original, TResult defaultValue)
    : ValueReader<TResult>(defaultValue)
{
    #region 配置
    private readonly IParser<XmlReader, TResult> _original = original;
    /// <summary>
    /// 原始解析器
    /// </summary>
    public IParser<XmlReader, TResult> Original
        => _original;
    #endregion

    /// <inheritdoc />
    public override bool TryParse(XmlReader reader, out TResult result)
    {
        var depth = reader.Depth;
        do
        {
            if (_original.TryParse(reader, out result))
                return true;
        } while (reader.Read() && reader.Depth >= depth);
        result = _defaultValue;
        return false;
    }
}
