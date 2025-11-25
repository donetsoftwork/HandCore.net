namespace Hand.Storage;

/// <summary>
/// 异步数据获取接口
/// </summary>
/// <typeparam name="TSpec">规格类型</typeparam>
/// <typeparam name="TData">数据类型</typeparam>
public interface IAsyncGet<in TSpec, TData>
{
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="spec">规格(条件)</param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<TData> Get(TSpec spec, CancellationToken token);
}
