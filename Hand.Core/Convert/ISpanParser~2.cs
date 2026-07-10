namespace Hand.Convert;

/// <summary>
/// ReadOnlySpan解析器
/// </summary>
/// <typeparam name="TResource"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface ISpanParser<TResource, TResult>
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryParse(ReadOnlySpan<TResource> resource, out TResult result);
}
