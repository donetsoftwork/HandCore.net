using Hand.Convert;
using Hand.ParseXml.Nodes;
using System.Xml;

namespace Hand.ParseXml.Move;

/// <summary>
/// 移到子节点并解析
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <param name="name"></param>
/// <param name="original"></param>
/// <param name="defaultValue"></param>
public class MoveToParser<TResult>(string name, IParser<XmlReader, TResult> original, TResult defaultValue)
    : WrapParser<TResult>(original), IParser<XmlReader, TResult>
{

    #region 配置
    private readonly string _name = name;
    private readonly TResult _defaultValue = defaultValue;

    /// <summary>
    /// 标签名
    /// </summary>
    public string Name
        => _name;
    /// <summary>
    /// 默认值
    /// </summary>
    public TResult DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public virtual bool TryParse(XmlReader reader, out TResult result)
    {
        if (reader.ReadToDescendant(_name) && _original.TryParse(reader, out result))
            return true;
        result = _defaultValue;
        return false;
    }
}
