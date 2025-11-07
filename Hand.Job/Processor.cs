using Hand.Collections;
using Hand.ConcurrentCollections;
using Hand.Job.Internal;
using Hand.States;
using JobItem = Hand.States.IState<bool>;

namespace Hand.Job;

/// <summary>
/// 任务处理器(支持可取消任务)
/// </summary>
public sealed class Processor(IQueue<JobItem> queue)
    : IQueueProcessor<JobItem>
{
    /// <summary>
    /// 任务处理器(支持可取消任务)
    /// </summary>
    public Processor()
        : this(new ConcurrentQueueAdapter<JobItem>())
    {
    }
    #region 配置
    private readonly IQueue<JobItem> _queue = queue;
    /// <summary>
    /// 队列
    /// </summary>
    public IQueue<JobItem> Queue
        => _queue;
    #endregion
    #region Add
    #region IJobState
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="action"></param>
    public IJobState Add(Action action)
    {
        var state = new ActionState(action);
        _queue.Enqueue(state);
        return state;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="action"></param>
    /// <param name="token"></param>
    public IJobState Add(Action action, CancellationToken token)
    {
        var state = new CancelableActionState(action, token);
        Enqueue(_queue, state);
        return state;
    }
    #endregion
    #region IJobResult
    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="func"></param>
    public IJobResult<TResult> Add<TResult>(Func<TResult> func)
    {
        var result = new FuncResult<TResult>(func);
        _queue.Enqueue(result);
        return result;
    }
    /// <summary>
    /// 添加
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public IJobResult<TResult> Add<TResult>(Func<TResult> func, CancellationToken token)
    {
        var result = new CancelableFuncResult<TResult>(func, token);
        Enqueue(_queue, result);
        return result;
    }
    #endregion
    #endregion
    #region AddTask
    #region IJobState
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="func"></param>
    public IJobState AddTask(Func<Task> func)
    {
        var state = new TaskState(func);
        _queue.Enqueue(state);
        return state;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <param name="func"></param>
    /// <param name="token"></param>
    public IJobState AddTask(Func<CancellationToken, Task> func, CancellationToken token)
    {
        var state = new CancelableTaskState(func, token);
        Enqueue(_queue, state);
        return state;
    }
    #endregion
    #region IJobResult
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public IJobResult<TResult> AddTask<TResult>(Func<Task<TResult>> func)
    {
        var result = new TaskResult<TResult>(func);
        Enqueue(_queue, result);
        return result;
    }
    /// <summary>
    /// 添加异步
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public IJobResult<TResult> AddTask<TResult>(Func<CancellationToken, Task<TResult>> func, CancellationToken token)
    {
        var result = new CancelableTaskResult<TResult>(func, token);
        Enqueue(_queue, result);
        return result;
    }
    #endregion
    #endregion
    #region IQueueProcessor<JobItem>
    /// <inheritdoc />
    async void IQueueProcessor<JobItem>.Run(IQueue<JobItem> queue, ThreadJobService<JobItem> service, CancellationToken token)
    {
        while (queue.TryDequeue(out var item))
        {
            if (service.Activate(item))
            {
                if (item is IJobItem job)
                {
                    try
                    {
                        job.Run();
                    }
                    catch (Exception ex)
                    {
                        Exception(job, ex);
                    }
                }
                else if (item is IAsyncJobItem asyncJob)
                {
                    try
                    {
                        await asyncJob.RunAsync(token);
                    }
                    catch (Exception ex)
                    {
                        Exception(asyncJob, ex);
                    }
                }
            }
            else
            {
                if (item is ICancelable cancelable)
                    Cancel(cancelable);
                break;
            }
            if (token.IsCancellationRequested)
                break;
        }
        // 线程用完释放(回收)
        service.Dispose();
    }
    #endregion
    /// <summary>
    /// 异常处理
    /// </summary>
    /// <param name="callBack"></param>
    /// <param name="ex"></param>

    public static void Exception(IExceptionable callBack, Exception ex)
    {
        try
        {
            callBack.OnException(ex);
        }
        catch { }
    }
    /// <summary>
    /// 取消
    /// </summary>
    /// <param name="cancelable"></param>
    public static void Cancel(ICancelable cancelable)
    {
        try
        {
            cancelable.OnCancel();
        }
        catch { }
    }
    /// <summary>
    /// 入队
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <param name="queue"></param>
    /// <param name="item"></param>
    public static void Enqueue<TItem>(IQueue<JobItem> queue, TItem item)
        where TItem : JobItem, ICancelable
    {
        if(item.Status)
        {
            queue.Enqueue(item);
        }
        else
        {
            Cancel(item);
        }
    }
}
