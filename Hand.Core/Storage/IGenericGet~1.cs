namespace Hand.Storage;

/// <summary>
/// 泛型数据获取接口
/// </summary>
/// <typeparam name="TSpec"></typeparam>
public interface IGenericGet<in TSpec>
{
    /// <summary>
    /// 获取数据
    /// </summary>
    /// <param name="spec">规格(条件)</param>
    /// <returns></returns>
    TResult Get<TResult>(TSpec spec);
}
