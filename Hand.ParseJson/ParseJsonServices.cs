using Hand.Configuration;
using Hand.Maping;
using Hand.ParseJson.Contracts;
using Hand.ParseJson.Nodes;
using Hand.Utf8;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Hand.ParseJson;

/// <summary>
/// json解析扩展方法
/// </summary>
public static class ParseJsonServices
{
    #region 常量
    /// <summary>
    /// 内存直接申请字节数上限
    /// </summary>
    public const int StackallocByteThreshold = 256;
    /// <summary>
    /// 内存直接申请字符数上限
    /// </summary>
    public const int StackallocCharThreshold = StackallocByteThreshold / 6;
    /// <summary>
    /// 字符最大字节数
    /// </summary>
    public const int MaxExpansionFactorWhileTranscoding = 3;
    /// <summary>
    /// 内存池申请内存上限
    /// </summary>
    public const long ArrayPoolMaxSizeBeforeUsingNormalAlloc =
#if NET
    1024 * 1024 * 1024 / 3; // ArrayPool limit increased in .NET 6
#else
    1024 * 1024 / 3;
#endif
    #endregion
    /// <summary>
    /// 获取解析结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <param name="json"></param>
    /// <returns></returns>
    public static TResult Get<TResult>(this IJsonParser<TResult> parser, string json)
    {
        byte[]? tempArray = null;
        var count = json.Length;
        // For performance, avoid obtaining actual byte count unless memory usage is higher than the threshold.
        Span<byte> utf8 =
            // Use stack memory
            count <= StackallocCharThreshold ? stackalloc byte[StackallocByteThreshold] :
            // Use a pooled array
            count <= ArrayPoolMaxSizeBeforeUsingNormalAlloc ? tempArray = ArrayPool<byte>.Shared.Rent(count * MaxExpansionFactorWhileTranscoding) :
            // Use a normal alloc since the pool would create a normal alloc anyway based on the threshold (per current implementation)
            // and by using a normal alloc we can avoid the Clear().
            new byte[StringConverter.GetByteCount(json)];

        try
        {
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            int actualByteCount = StringConverter.GetBytes(json, utf8);
#else
            int actualByteCount = StringConverter.GetBytes(json, tempArray!);
#endif

            var reader = new Utf8JsonReader(utf8.Slice(0, actualByteCount));
            _ = parser.TryParser(ref reader, out var result);
            return result;
        }
        finally
        {
            if (tempArray != null)
            {
                utf8.Clear();
                ArrayPool<byte>.Shared.Return(tempArray);
            }
        }
    }
    /// <summary>
    /// 获取解析结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="parser"></param>
    /// <param name="reader"></param>
    /// <returns></returns>
    public static TResult Get<TResult>(this IJsonParser<TResult> parser, Utf8JsonReader reader)
    {
        _ = parser.TryParser(ref reader, out var result);
        return result;
    }
    #region First
    /// <summary>
    /// 第一个节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="element"></param>
    /// <param name="original"></param>
    /// <returns></returns>
    public static FirstReader<TResult> First<TResult>(this IJsonParser<TResult> original, string element)
    {
        if (original is IDefault<TResult> @default)
            return new FirstReader<TResult>(element, original, @default.DefaultValue);
        if (original is IEntityParser entity)
            return entity.Json.First(element, original);
        return HandJson.Default.First(element, original);
    }
    /// <summary>
    /// 第一个节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="original"></param>
    /// <returns></returns>
    public static FirstReader<TResult> First<TResult>(this IJsonParser<TResult> original)
        => First(original, typeof(TResult).Name);
    #endregion
    #region Repeat
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    public static RepeatReader<TResult> Repeat<TResult>(this IEntityParser<TResult> item)
        => new(item.Json, item);
    /// <summary>
    /// 重复节点读取
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="item"></param>
    /// <returns></returns>
    public static RepeatReader<TResult> Repeat<TResult>(this IJsonParser<TResult> item)
    {
        if (item is IEntityParser entity)
            return new RepeatReader<TResult>(entity.Json, item);
        return new RepeatReader<TResult>(HandJson.Default, item);
    }
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
        => new(entity.Json, entity, new DelegateConverter<TSource, TDest>(convert));
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
        => new(entity.Json, entity, converter);
    /// <summary>
    /// 转换为其他类型解析器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDest"></typeparam>
    /// <param name="node"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static ConvertParser<TSource, TDest> Convert<TSource, TDest>(this IJsonParser<TSource> node, IConverter<TSource, TDest> converter)
    {
        if (node is IEntityParser entity)
            return new ConvertParser<TSource, TDest>(entity.Json, node, converter);
        return new ConvertParser<TSource, TDest>(HandJson.Default, node, converter);
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
    public static ConvertParser<TSource, TDest> Convert<TSource, TDest>(this IJsonParser<TSource> node, Converter<TSource, TDest> convert)
        => Convert(node, new DelegateConverter<TSource, TDest>(convert));
    /// <summary>
    /// 转换为数组解析器
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="repeat"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConvertParser<List<TItem>, TItem[]> ToArray<TItem>(this RepeatReader<TItem> repeat)
        => Convert(repeat, new DelegateConverter<List<TItem>, TItem[]>(static items => [.. items]));
    #endregion
}
