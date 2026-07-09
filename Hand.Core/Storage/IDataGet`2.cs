namespace Hand.Storage;

/// <summary>
/// 数据获取接口
/// </summary>
/// <typeparam name="TSpec">规格类型</typeparam>
/// <typeparam name="TResult">数据类型</typeparam>
public interface IDataGet<in TSpec, out TResult>
{
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="spec">规格(条件)</param>
    /// <returns></returns>
    TResult Get(TSpec spec);
}
