using Hand.EventHandlers;
using Xunit.Abstractions;

namespace EventBusTests.Supports;

internal class Handler(ITestOutputHelper output) : IEventHandler<string>
{
    private readonly ITestOutputHelper _output = output;

    public void Handle(string @event)
    {
        Thread.Sleep(10);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Handler: {@event},{DateTime.Now:HH:mm:ss.fff}");
    }
}
internal class AsyncHandler(ITestOutputHelper output) : IEventHandler<string>
{
    private readonly ITestOutputHelper _output = output;

    public async void Handle(string @event)
    {
        await Task.Delay(10);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} AsyncHandler: {@event},{DateTime.Now:HH:mm:ss.fff}");
    }
}
internal class TaskHandler(ITestOutputHelper output) : ITaskEventHandler<string>
{
    private readonly ITestOutputHelper _output = output;

    public async Task TaskHandle(string @event, CancellationToken cancellationToken)
    {
        await Task.Delay(10, cancellationToken);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} TaskHandler: {@event},{DateTime.Now:HH:mm:ss.fff}");
    }
}
