using System.Buffers;

namespace Hand.Maping;

/// <summary>
/// ReadOnlySequence转化器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface ISequenceConverter<TSource, out TDest>
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    TDest Convert(ReadOnlySequence<TSource> source);
}