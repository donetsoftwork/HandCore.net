using Hand.Convert;
using Hand.Maping;
using System.Xml;

namespace Hand.ParseXml.Nodes;

/// <summary>
/// 转化解析器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="xml"></param>
/// <param name="original"></param>
/// <param name="converter"></param>
/// <param name="defaultValue"></param>
public class ConvertParser<TSource, TDest>(HandXml xml, IParser<XmlReader, TSource> original, IConverter<TSource, TDest> converter, TDest defaultValue)
    : WrapParser<TSource>(xml, original)
    , IParser<XmlReader, TDest>
{
    #region 配置
    private readonly IConverter<TSource, TDest> _converter = converter;
    private readonly TDest _defaultValue = defaultValue;
    #endregion

    /// <inheritdoc />
    public bool TryParse(XmlReader reader, out TDest result)
    {
        if(_original.TryParse(reader, out var originalResult))
        {
            result = _converter.Convert(originalResult);
            return true;
        }
        result = _defaultValue;
        return false;
    }
}
