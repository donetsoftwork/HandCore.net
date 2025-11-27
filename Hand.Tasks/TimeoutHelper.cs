namespace Hand.Tasks;

/// <summary>
/// 超时辅助类
/// </summary>
public static class TimeoutHelper
{
    /// <summary>
    /// 检查是否超时
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task<TResult> ThrowIfTimeout<TResult>(Task<TResult> task, TimeSpan timeout)
    {
        using var tokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));
        if (completedTask == task)
        {
#if NET8_0_OR_GREATER
            await tokenSource.CancelAsync();
#else
            tokenSource.Cancel();
#endif
            return await task;
        }
        throw new TimeoutException("The operation has timed out.");
    }
    /// <summary>
    /// 检查是否超时
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task ThrowIfTimeout(Task task, TimeSpan timeout)
    {
        using var tokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));
        if (completedTask == task)
        {
#if NET8_0_OR_GREATER
            await tokenSource.CancelAsync();
#else
            tokenSource.Cancel();
#endif
            await task;
        }
        else
        {
            throw new TimeoutException("The operation has timed out.");
        }            
    }
}
