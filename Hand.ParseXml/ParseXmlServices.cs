using Hand.Configuration;
using Hand.Convert;
using Hand.Maping;
using Hand.ParseXml.Contracts;
using Hand.ParseXml.Move;
using Hand.ParseXml.Nodes;
using Hand.Storage;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

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
    public static IEnumerable<TResult> Parse<TResult>(this IParser<XmlReader, IEnumerable<TResult>> parser, string xml)
    {
        _ = TryParse(parser, xml, out var result);
        return result;
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
    public static bool TryParse<TResult>(this IParser<XmlReader, IEnumerable<TResult>> parser, string xml, out IEnumerable<TResult> result)
    {
        using var stringReader = new StringReader(xml);
        using var xmlReader = XmlReader.Create(stringReader);
        if( parser.TryParse(xmlReader, out var enumerable))
        {
            result = [.. enumerable];
            return true;
        }
        result = [];
        return false;
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
    #region First
    /// <summary>
    /// 第一个节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public static FirstReader<TResult> First<TResult>(this IEntityParser<TResult> original)
        => new(original, original.Xml.DefaultValues.Get<TResult>());
    /// <summary>
    /// 第一个节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public static FirstReader<TResult> First<TResult>(this IParser<XmlReader, TResult> original)
    {
        if (original is IDefault<TResult> @default)
            return new FirstReader<TResult>(original, @default.DefaultValue);
        if (original is IEntityParser entity)
            return entity.Xml.First(original);
        return HandXml.Default.First(original);
    }
    #endregion
    #endregion
    /// <summary>
    /// 移入子节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public static MoveInParser<TResult> MoveIn<TResult>(this IParser<XmlReader, TResult> original)
    {
        if (original is IDefault<TResult> @default)
            return new MoveInParser<TResult>(original, @default.DefaultValue);
        if (original is IEntityParser entity)
            return entity.Xml.MoveIn(original);
        return HandXml.Default.MoveIn(original);
    }
    /// <summary>
    /// 移入子节点
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static MoveToParser<TResult> MoveTo<TResult>(this IParser<XmlReader, TResult> original, string name)
    {
        if (original is IDefault<TResult> @default)
            return new MoveToParser<TResult>(name, original, @default.DefaultValue);
        if (original is IEntityParser entity)
            return entity.Xml.MoveTo(name, original);
        return HandXml.Default.MoveTo(name, original);
    }
    #region  Element
    /// <summary>
    /// 父节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="element"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    public static ElementParser<TResult> Element<TResult>(this IParser<XmlReader, TResult> original, string element)
    {
        if (original is IDefault<TResult> @default)
            return new ElementParser<TResult>(element, original, @default.DefaultValue);
        if (original is IEntityParser entity)
            return entity.Xml.Element(element, original);
        return HandXml.Default.Element(element, original);
    }
    /// <summary>
    /// 节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ElementParser<TResult> Element<TResult>(this IParser<XmlReader, TResult> original)
        => Element(original, typeof(TResult).Name);
    #endregion
    #region Each
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static EachReader<TResult> Each<TResult>(this IParser<XmlReader, TResult> item)
        => new(item);
    #endregion
    #region Dictionary
    /// <summary>
    /// 字典读取
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="comparer"></param>
    /// <param name="acceptDefault"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DictionaryParser<TKey, TValue> Dictionary<TKey, TValue>(this IParser<XmlReader, TKey> key, IParser<XmlReader, TValue> value, IEqualityComparer<TKey> comparer, bool acceptDefault = false)
        where TKey : notnull
        =>new(key, value, comparer, acceptDefault);
    /// <summary>
    /// 字典读取
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="acceptDefault"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DictionaryParser<TKey, TValue> Dictionary<TKey, TValue>(this IParser<XmlReader, TKey> key, IParser<XmlReader, TValue> value, bool acceptDefault = false)
        where TKey : notnull
        => new(key, value, EqualityComparer<TKey>.Default, acceptDefault);
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
        => new(entity, converter, entity.Xml.DefaultValues.Get<TDest>());
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
        return new ConvertParser<TSource, TDest>(node, converter, xml.DefaultValues.Get<TDest>());
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
    /// <param name="original"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<IEnumerable<TItem>, TItem[]> ToArray<TItem>(this EachReader<TItem> original)
        => Convert(original, new DelegateConverter<IEnumerable<TItem>, TItem[]>(static items => [.. items]));
    /// <summary>
    /// 转换为列表解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<IEnumerable<TItem>, List<TItem>> ToList<TItem>(this EachReader<TItem> original)
        => Convert(original, new DelegateConverter<IEnumerable<TItem>, List<TItem>>(static items => [.. items]));
    #endregion
}
