namespace Hand.Storage;

/// <summary>
/// 数据获取接口
/// </summary>
/// <typeparam name="TSpec">规格类型</typeparam>
/// <typeparam name="TData">数据类型</typeparam>
public interface IDataGet<in TSpec, out TData>
{
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="spec">规格(条件)</param>
    /// <returns></returns>
    TData Get(TSpec spec);
}
