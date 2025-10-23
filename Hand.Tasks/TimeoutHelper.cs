namespace Hand.Tasks;

/// <summary>
/// 超时辅助类
/// </summary>
public static class TimeoutHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using var tokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));
        if (completedTask == task)
        {
            tokenSource.Cancel();
            return await task;
        }
        else
        {
            throw new TimeoutException("The operation has timed out.");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
    {
        using var tokenSource = new CancellationTokenSource();
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, tokenSource.Token));
        if (completedTask == task)
        {
            tokenSource.Cancel();
            await task;
        }
        else
        {
            throw new TimeoutException("The operation has timed out.");
        }
    }
}
