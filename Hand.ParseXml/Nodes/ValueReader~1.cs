using Hand.Configuration;
using Hand.Convert;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 值读取器基类
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class ValueReader<TValue>(TValue defaultValue)
    : IParser<XmlReader, TValue>, IDefault<TValue>
{
    #region 配置
    /// <summary>
    /// 默认值
    /// </summary>
    protected readonly TValue _defaultValue = defaultValue;
    /// <summary>
    /// 默认值
    /// </summary>
    public TValue DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public abstract bool TryParse(XmlReader reader, out TValue result);
}
