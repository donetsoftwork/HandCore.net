namespace Hand.Convert;

/// <summary>
/// 解析器
/// </summary>
/// <typeparam name="TResource"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IParser<TResource, TResult>
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryParse(TResource resource, out TResult result);
}
