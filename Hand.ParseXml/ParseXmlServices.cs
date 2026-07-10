using Hand.Configuration;
using Hand.Convert;
using Hand.Maping;
using Hand.ParseXml.Contracts;
using Hand.ParseXml.Nodes;
using Hand.Storage;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Hand.ParseXml;

/// <summary>
/// xml解析扩展方法
/// </summary>
public static class ParseXmlServices
{
    /// <summary>
    /// 解析xml
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static TResult Get<TResult>(this IDataGet<XmlReader, TResult> parser, string xml)
    {
        using var stringReader = new StringReader(xml);
        using var xmlReader = XmlReader.Create(stringReader);
        return parser.Get(xmlReader);
    }
    /// <summary>
    /// 解析xml
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static TResult Parse<TResult>(this IParser<XmlReader, TResult> parser, string xml)
    {
        _ = TryParse(parser, xml, out var result);
        return result;
    }
    /// <summary>
    /// 获取解析结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static TResult Parse<TResult>(this IParser<XmlReader, TResult> parser, XmlReader reader)
    {
        _ = parser.TryParse(reader, out var result);
        return result;
    }
    /// <summary>
    /// 解析xml
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <param name="xml"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public static bool TryParse<TResult>(this IParser<XmlReader, TResult> parser, string xml, out TResult result)
    {
        using var stringReader = new StringReader(xml);
        using var xmlReader = XmlReader.Create(stringReader);
        return parser.TryParse(xmlReader, out result);
    }
    /// <summary>
    /// 成员
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemberParser<TMember> Member<TMember>(this IParser<XmlReader, TMember> reader, string name)
        => new(name, reader);
    #region First
    /// <summary>
    /// 第一个节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="element"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    public static FirstReader<TResult> First<TResult>(this IParser<XmlReader, TResult> original, string element)
    {
        if (original is IDefault<TResult> @default)
            return new FirstReader<TResult>(element, original, @default.DefaultValue);
        if (original is IEntityParser entity)
            return entity.Xml.First(element, original);
        return HandXml.Default.First(element, original);
    }
    /// <summary>
    /// 第一个节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FirstReader<TResult> First<TResult>(this IParser<XmlReader, TResult> original)
        => First(original, typeof(TResult).Name);
    #endregion
    #region Repeat
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="element"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReader<TResult> Repeat<TResult>(this IEntityParser<TResult> item, string element)
        => new(item.Xml, element, item);
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReader<TResult> Repeat<TResult>(this IEntityParser<TResult> item)
        => new(item.Xml, typeof(TResult).Name, item);
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="element"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReader<TResult> Repeat<TResult>(this IParser<XmlReader, TResult> item, string element)
    {
        if(item is IEntityParser entity)
            return new RepeatReader<TResult>(entity.Xml, element, item);
        return new RepeatReader<TResult>(HandXml.Default, element, item);
    }
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static RepeatReader<TResult> Repeat<TResult>(this IParser<XmlReader, TResult> item)
        => Repeat(item, typeof(TResult).Name);
    #endregion
    #region Convert
    /// <summary>
    /// 转换为其他类型解析器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="entity"></param>
    /// <param name="convert"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<TSource, TDest> Convert<TSource, TDest>(this IEntityParser<TSource> entity, Converter<TSource, TDest> convert)
        => Convert(entity, new DelegateConverter<TSource, TDest>(convert));
    /// <summary>
    /// 转换为其他类型解析器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="entity"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<TSource, TDest> Convert<TSource, TDest>(this IEntityParser<TSource> entity, IConverter<TSource, TDest> converter)
    {
        var xml = entity.Xml;
        var defaultValue = xml.DefaultValues.Get<TDest>();
        return new ConvertParser<TSource, TDest>(xml, entity, converter, defaultValue);
    }
    /// <summary>
    /// 转换为其他类型解析器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="node"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static ConvertParser<TSource, TDest> Convert<TSource, TDest>(this IParser<XmlReader, TSource> node, IConverter<TSource, TDest> converter)
    {
        var xml = node is IEntityParser entity ? entity.Xml : HandXml.Default;
        var defaultValue = xml.DefaultValues.Get<TDest>();
        return new ConvertParser<TSource, TDest>(xml, node, converter, defaultValue);
    }
    /// <summary>
    /// 转换为其他类型解析器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="node"></param>
    /// <param name="convert"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<TSource, TDest> Convert<TSource, TDest>(this IParser<XmlReader, TSource> node, Converter<TSource, TDest> convert)
        => Convert(node, new DelegateConverter<TSource, TDest>(convert));
    /// <summary>
    /// 转换为数组解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="repeat"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<IEnumerable<TItem>, TItem[]> ToArray<TItem>(this RepeatReader<TItem> repeat)
        => Convert(repeat, new DelegateConverter<IEnumerable<TItem>, TItem[]>(static items => [.. items]));
    /// <summary>
    /// 转换为列表解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="repeat"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<IEnumerable<TItem>, List<TItem>> ToList<TItem>(this RepeatReader<TItem> repeat)
        => Convert(repeat, new DelegateConverter<IEnumerable<TItem>, List<TItem>>(static items => [.. items]));
    #endregion
}
