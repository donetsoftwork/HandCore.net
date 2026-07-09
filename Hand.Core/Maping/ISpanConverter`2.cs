namespace Hand.Maping;

/// <summary>
/// ReadOnlySpan转化器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface ISpanConverter<TSource, out TDest>
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    TDest Convert(ReadOnlySpan<TSource> source);
}