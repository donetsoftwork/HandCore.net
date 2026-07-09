using Hand.Maping;
using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace Hand.ParseJson.Nodes;

/// <summary>
/// 转化解析器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
/// <param name="json"></param>
/// <param name="original"></param>
/// <param name="converter"></param>
public class ConvertParser<TSource, TDest>(HandJson json, IJsonParser<TSource> original, IConverter<TSource, TDest> converter)
    : WrapParser<TSource>(json, original)
    , IJsonParser<TDest>
{
    #region 配置
    private readonly IConverter<TSource, TDest> _converter = converter;
    #endregion

    /// <inheritdoc />
    public bool TryParser(ref Utf8JsonReader reader, out TDest result)
    {
        var state = _original.TryParser(ref reader, out var source);
        result = _converter.Convert(source);
        return state;
    }
}
