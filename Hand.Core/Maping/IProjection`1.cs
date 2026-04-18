namespace Hand.Maping;

/// <summary>
/// 投影转化接口
/// </summary>
public interface IProjection<T>
{
    /// <summary>
    /// 尝试转化
    /// </summary>
    /// <param name="source"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryConvert(T source, out T result);
}