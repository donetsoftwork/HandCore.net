namespace Hand.Tasks.Internal;

internal class FuncTaskState<TResult>(QueueTaskScheduler scheduler, Func<Task<TResult>> func, TaskCompletionSource<TResult> source)
{
    #region 配置
    private readonly QueueTaskScheduler _scheduler = scheduler;
    private readonly Func<Task<TResult>> _func = func;
    ///// <summary>
    ///// 并发控制
    ///// </summary>
    //private readonly ConcurrentControl _control = control;
    /// <summary>
    /// 
    /// </summary>
    private readonly TaskCompletionSource<TResult> _source = source;

    //public Func<Task<TResult>> Func => _func;
    private TaskFactory factory = null;
    private Task _parent = null;
    //System.Runtime.CompilerServices.AsyncMethodBuilderCore.
    #endregion
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task<TResult> StartTask(ConcurrentTaskFactory factory, Func<Task<TResult>> func)
    {
        var source = new TaskCompletionSource<TResult>();
        var scheduler = factory.ConcurrentScheduler;
        var state = new FuncTaskState<TResult>(scheduler, func, source);
        state.factory = factory;
        factory.StartNew(Action, state);
        //scheduler.Enqueue(task);
        return source.Task;
    }
    /// <summary>
    /// 执行
    /// </summary>
    private void Run()
    {              
        //_scheduler.Enqueue(_parent);
        var task = _func();
        _scheduler.Queue.Enqueue(task);
        //_control.Increment();
        // ContinueWith不能使用当前TaskScheduler,可能会导致死锁
        //task.ContinueWith(ContinueAction, this, _scheduler);
    }
    /// <summary>
    /// 执行静态委托
    /// </summary>
    /// <param name="state"></param>
    private static void Action(object state)
    {
        var taskState = (FuncTaskState<TResult>)state;
        taskState.Run();
    }
    /// <summary>
    /// ContinueWith静态委托
    /// </summary>
    /// <param name="task"></param>
    /// <param name="state"></param>
    private static void ContinueAction(Task<TResult> task, object state)
    {
        Continue(task, (FuncTaskState<TResult>)state);
    }
    /// <summary>
    /// ContinueWith逻辑
    /// </summary>
    /// <param name="task"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    private static void Continue(Task<TResult> task, FuncTaskState<TResult> state)
    {
        //state._control.Decrement();
        var source = state._source;
        if (task.IsCanceled)
        {
            source.SetCanceled();
        }
        else if (task.IsFaulted)
        {
            source.SetException(task.Exception);
        }
        else
        {
            source.SetResult(task.Result);
        }
        //if (task.IsCanceled)
        //{
        //    throw new TaskCanceledException();
        //}
        //else if (task.IsFaulted)
        //{
        //    throw task.Exception;
        //}
        //else
        //{
        //    return task.Result;
        //}
    }
}
