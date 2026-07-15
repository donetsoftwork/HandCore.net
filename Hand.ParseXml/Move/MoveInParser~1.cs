using Hand.Convert;
using Hand.ParseXml.Nodes;
using System.Xml;

namespace Hand.ParseXml.Move;

/// <summary>
/// 移入子节点并解析
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class MoveInParser<TResult>(IParser<XmlReader, TResult> original, TResult defaultValue)
    : WrapParser<TResult>(original), IParser<XmlReader, TResult>
{
    #region 配置
    private readonly TResult _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public TResult DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public virtual bool TryParse(XmlReader reader, out TResult result)
    {
        if (Move(reader) && _original.TryParse(reader, out result))
            return true;
        result = _defaultValue;
        return false;
    }
}
