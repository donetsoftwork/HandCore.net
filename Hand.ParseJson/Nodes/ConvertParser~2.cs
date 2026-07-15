using Hand.Maping;
using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 转化解析器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="original"></param>
/// <param name="converter"></param>
/// <param name="defaultValue"></param>
public class ConvertParser<TSource, TDest>(IJsonParser<TSource> original, IConverter<TSource, TDest> converter, TDest defaultValue)
    : WrapParser<TSource>(original)
    , IJsonParser<TDest>
{
    #region 配置
    private readonly IConverter<TSource, TDest> _converter = converter;
    private readonly TDest _defaultValue = defaultValue;

    /// <summary>
    /// 转化器
    /// </summary>
    public IConverter<TSource, TDest> Converter 
        => _converter;
    /// <summary>
    /// 默认值
    /// </summary>
    public TDest DefaultValue
        => _defaultValue;
    #endregion

    /// <inheritdoc />
    public bool TryParse(ref Utf8JsonReader reader, out TDest result)
    {
        if(_original.TryParse(ref reader, out var source))
        {
            result = _converter.Convert(source);
            return true;
        }
        result = _defaultValue;
        return false;
    }
}
