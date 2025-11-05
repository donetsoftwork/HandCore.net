namespace Hand.Tasks.Internal;

/// <summary>
/// 异步状态处理
/// </summary>
/// <param name="scheduler"></param>
/// <param name="func"></param>
internal class TaskState(QueueTaskScheduler scheduler, Func<Task> func)
{
    #region 配置
    private readonly QueueTaskScheduler _scheduler = scheduler;
    private readonly Func<Task> _func = func;
    private Task _task;
    //private Task _continueTask;
    #endregion
    /// <summary>
    /// 启动异步任务
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static Task StartTask(ConcurrentTaskFactory factory, Func<Task> func)
        => factory.StartNew(Action, new TaskState(factory.ConcurrentScheduler, func));
    /// <summary>
    /// 执行
    /// </summary>
    private void Run()
    {
        _task = _func();
        _scheduler.Queue.Enqueue(_task);
        //// ContinueWith不能使用当前TaskScheduler,可能会导致死锁
        ////_continueTask = _task.ContinueWith(ContinueAction, _scheduler);
        //_ = _task.ContinueWith(ContinueAction, _scheduler);
    }
    /// <summary>
    /// 执行静态委托
    /// </summary>
    /// <param name="state"></param>
    private static void Action(object state)
    {
        var taskState = (TaskState)state;
        taskState.Run();
    }
    /// <summary>
    /// ContinueWith静态委托
    /// </summary>
    /// <param name="task"></param>
    private static void ContinueAction(Task task)
    {
    }
}
