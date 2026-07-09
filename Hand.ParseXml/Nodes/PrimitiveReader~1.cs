using Hand.Maping;
using Hand.ParseXml.Contracts;
using Hand.Storage;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 基础类型读取器
/// </summary>
/// <typeparam name="TPrimitive"></typeparam>
/// <param name="original"></param>
/// <param name="converter"></param>
/// <param name="defaultValue"></param>
public class PrimitiveReader<TPrimitive>(IXmlParser<string> original, IConverter<string, TPrimitive> converter, TPrimitive defaultValue)
    : ValueReader<TPrimitive>(defaultValue)
{
    #region 配置
    private readonly IXmlParser<string> _original = original;
    private readonly IConverter<string, TPrimitive> _converter = converter;

    /// <summary>
    /// 原始读取器
    /// </summary>
    public IXmlParser<string> Original 
        => _original;
    /// <summary>
    /// 转化器
    /// </summary>
    public IConverter<string, TPrimitive> Converter 
        => _converter;
    #endregion

    ///// <inheritdoc />
    //public override TPrimitive Get(XmlReader reader)
    //{
    //    var originalValue = _original.Get(reader);
    //    if (string.IsNullOrEmpty(originalValue))
    //        return _defaultValue;
    //    return _converter.Convert(originalValue);
    //}
    /// <inheritdoc />
    public override bool TryParser(XmlReader reader, out TPrimitive result)
    {
        if (_original.TryParser(reader, out var originalResult))
        {
            result = _converter.Convert(originalResult);
            return true;
        }
        result = _defaultValue;
        return false;
    }
}

