namespace Hand.Dataflow.Feeders;

/// <summary>
/// 进料器
/// </summary>
/// <typeparam name="TData"></typeparam>
public class Feeder<TData>
    : IFeeder<TData>
{
    private TaskCompletionSource<TData> _source = new();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Feed(TData data)
    {
        _source.SetResult(data);
        return true;
    }
    /// <summary>
    /// 结果
    /// </summary>
    public Task<TData> Result
        => _source.Task;
}
