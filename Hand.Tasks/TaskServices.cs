using Hand.Job;

namespace Hand.Tasks;

/// <summary>
/// Task扩展方法
/// </summary>
public static class TaskServices
{
    #region StartNew
    #region State
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task StartNew(this Processor processor, Action action)
    {
        var state = TaskWrapper.Wrap(action);
        processor.Queue.Enqueue(state);
        return state.Task;
    }
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task StartNew(this Processor processor, Action action, CancellationToken token)
    {
        var state = TaskWrapper.Wrap(action, token);
        Processor.Enqueue(processor.Queue, state);
        return state.Task;
    }
    #endregion
    #region Result
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartNew<TResult>(this Processor processor, Func<TResult> func)
    {
        var result = TaskWrapper.Wrap(func);
        processor.Queue.Enqueue(result);
        return result.Task;
    }
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<TResult> StartNew<TResult>(this Processor processor, Func<TResult> func, CancellationToken token)
    {
        var result = TaskWrapper.Wrap(func, token);
        Processor.Enqueue(processor.Queue, result);
        return result.Task;
    }
    #endregion
    #endregion
    #region StartTask
    #region State
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task StartTask(this Processor processor, Func<Task> func)
    {
        var state = TaskWrapper.Wrap(func);
        processor.Queue.Enqueue(state);
        return state.Task;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    public static Task StartTask(this Processor processor, Func<CancellationToken, Task> func, CancellationToken token)
    {
        var state = TaskWrapper.Wrap(func, token);
        Processor.Enqueue(processor.Queue, state);
        return state.Task;
    }
    #endregion
    #region Result
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask<TResult>(this Processor processor, Func<Task<TResult>> func)
    {
        var result = TaskWrapper.Wrap(func);
        processor.Queue.Enqueue(result);
        return result.Task;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask<TResult>(this Processor processor, Func<CancellationToken, Task<TResult>> func, CancellationToken token)
    {
        var result = TaskWrapper.Wrap(func, token);
        Processor.Enqueue(processor.Queue, result);
        return result.Task;
    }
    #endregion
    #endregion
}
