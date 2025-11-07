namespace Hand.Job;

/// <summary>
/// 异步对象操作
/// </summary>
/// <typeparam name="TInstance"></typeparam>
public interface IAsyncProcessor<TInstance>
{
    /// <summary>
    /// 异步执行
    /// </summary>
    /// <param name="item"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task RunAsync(TInstance item, CancellationToken token);
}
