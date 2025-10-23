namespace Hand.Dataflow.Feeders;

/// <summary>
/// 进料器接口
/// </summary>
public interface IFeeder<TData>
{
    /// <summary>
    /// 进料
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    bool Feed(TData data);
}
