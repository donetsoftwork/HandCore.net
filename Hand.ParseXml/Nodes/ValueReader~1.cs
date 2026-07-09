using Hand.Configuration;
using Hand.ParseXml.Contracts;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 值读取器基类
/// </summary>
/// <typeparam name="TValue"></typeparam>
public abstract class ValueReader<TValue>(TValue defaultValue)
    : IXmlParser<TValue>, IDefault<TValue>
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

    ///// <inheritdoc />
    //public abstract TValue Get(XmlReader reader);
    /// <inheritdoc />
    public abstract bool TryParser(XmlReader reader, out TValue result);
}
