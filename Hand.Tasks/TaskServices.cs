using Hand.Collections;
using Hand.Job;
using TaskItem = Hand.States.IState<bool>;

namespace Hand.Tasks;

/// <summary>
/// Task扩展方法
/// </summary>
public static class TaskServices
{
    #region Processor
    #region StartNew
    #region State
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task StartNew(this Processor processor, Action action)
        => processor.Queue.StartNew(action);
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task StartNew(this Processor processor, Action action, CancellationToken token)
         => processor.Queue.StartNew(action, token);
    #endregion
    #region Result
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartNew<TResult>(this Processor processor, Func<TResult> func)
        => processor.Queue.StartNew(func);
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<TResult> StartNew<TResult>(this Processor processor, Func<TResult> func, CancellationToken token)
        => processor.Queue.StartNew(func, token);
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
        => processor.Queue.StartTask(func);
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    public static Task StartTask(this Processor processor, Func<CancellationToken, Task> func, CancellationToken token)
        => processor.Queue.StartTask(func, token);
    #endregion
    #region Result
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask<TResult>(this Processor processor, Func<Task<TResult>> func)
        => processor.Queue.StartTask(func);
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="processor"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask<TResult>(this Processor processor, Func<CancellationToken, Task<TResult>> func, CancellationToken token)
        => processor.Queue.StartTask(func, token);
    #endregion
    #endregion
    #endregion
    #region IQueue
    #region StartNew
    #region State
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Task StartNew(this IQueue<TaskItem> queue, Action action)
    {
        var state = TaskWrapper.Wrap(action);
        queue.Enqueue(state);
        return state.Task;
    }
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="action"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task StartNew(this IQueue<TaskItem> queue, Action action, CancellationToken token)
    {
#if NET45
        var state = TaskWrapper.Wrap(action, token);
        if (token.IsCancellationRequested)
        {            
            state.OnCancel();
            return state.Task;
        }
#else
        if (token.IsCancellationRequested)
            return Task.FromCanceled(token);
        var state = TaskWrapper.Wrap(action, token);
#endif
        Processor.Enqueue(queue, state);
        return state.Task;
    }
    #endregion
    #region Result
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartNew<TResult>(this IQueue<TaskItem> queue, Func<TResult> func)
    {
        var result = TaskWrapper.Wrap(func);
        queue.Enqueue(result);
        return result.Task;
    }
    /// <summary>
    /// 启动任务
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="queue"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<TResult> StartNew<TResult>(this IQueue<TaskItem> queue, Func<TResult> func, CancellationToken token)
    {
#if NET45
        var result = TaskWrapper.Wrap(func, token);
        if (token.IsCancellationRequested)
        {
            result.OnCancel();
            return result.Task;
        }
#else
        if (token.IsCancellationRequested)
            return Task.FromCanceled<TResult>(token);
        var result = TaskWrapper.Wrap(func, token);
#endif
        Processor.Enqueue(queue, result);
        return result.Task;
    }
    #endregion
    #endregion
    #region StartTask
    #region State
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task StartTask(this IQueue<TaskItem> queue, Func<Task> func)
    {
        var state = TaskWrapper.Wrap(func);
        queue.Enqueue(state);
        return state.Task;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    public static Task StartTask(this IQueue<TaskItem> queue, Func<CancellationToken, Task> func, CancellationToken token)
    {
#if NET45
        var state = TaskWrapper.Wrap(func, token);
        if (token.IsCancellationRequested)
        {
            state.OnCancel();
            return state.Task;
        }
#else
        if (token.IsCancellationRequested)
            return Task.FromCanceled(token);
        var state = TaskWrapper.Wrap(func, token);
#endif
        Processor.Enqueue(queue, state);
        return state.Task;
    }
    #endregion
    #region Result
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="queue"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask<TResult>(this IQueue<TaskItem> queue, Func<Task<TResult>> func)
    {
        var result = TaskWrapper.Wrap(func);
        queue.Enqueue(result);
        return result.Task;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="queue"></param>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask<TResult>(this IQueue<TaskItem> queue, Func<CancellationToken, Task<TResult>> func, CancellationToken token)
    {
#if NET45
        var result = TaskWrapper.Wrap(func, token);
        if (token.IsCancellationRequested)
        {
            result.OnCancel();
            return result.Task;
        }
#else
        if (token.IsCancellationRequested)
            return Task.FromCanceled<TResult>(token);
        var result = TaskWrapper.Wrap(func, token);
#endif
        Processor.Enqueue(queue, result);
        return result.Task;
    }
    #endregion
    #endregion
    #endregion
}
