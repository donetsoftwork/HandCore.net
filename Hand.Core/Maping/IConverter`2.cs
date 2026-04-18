namespace Hand.Maping;

/// <summary>
/// 转化器
/// </summary>
/// <typeparam name="TSource"></typeparam>
/// <typeparam name="TDest"></typeparam>
public interface IConverter<in TSource, out TDest>
{
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    TDest Convert(TSource source);
}