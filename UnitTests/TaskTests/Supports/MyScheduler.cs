namespace TaskTests.Supports;

internal class MyScheduler : TaskScheduler
{
    protected override IEnumerable<Task>? GetScheduledTasks()
        => [];
    protected override void QueueTask(Task task)
    {
        ThreadPool.QueueUserWorkItem((_) => TryExecuteTask(task));
    }
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        => TryExecuteTask(task);
}
