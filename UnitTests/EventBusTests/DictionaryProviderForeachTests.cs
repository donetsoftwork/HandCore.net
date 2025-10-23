using EventBusTests.Supports;
using Hand.Events;
using Xunit.Abstractions;

namespace EventBusTests;

public class DictionaryProviderForeachTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void EventHandler()
    {
        var (bus, provider) = CreateEventBus();
        var handler = new Handler(_output);
        provider.AddHandler(handler);
        for (int i = 0; i < 100; i++)
        {
            await bus.PublishAsync("Event" + i);
            await Task.Delay(1);
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        await Task.Delay(1000);
    }
    [Fact]
    public async void TaskEventHandler()
    {
        //TaskInfo
        var (bus, provider) = CreateEventBus();
        var taskHandler = new TaskHandler(_output);
        provider.AddEventHandler(taskHandler);
        for (int i = 0; i < 100; i++)
        {
            await bus.PublishAsync("Event" + i);
            await Task.Delay(1);
        }
            
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        await Task.Delay(1000);
    }
    [Fact]
    public async void AsyncEventHandler()
    {
        var (bus, provider) = CreateEventBus();
        var asyncHandler = new AsyncHandler(_output);
        provider.AddHandler(asyncHandler);
        for (int i = 0; i < 100; i++)
        {
            await bus.PublishAsync("Event" + i);
            await Task.Delay(1);
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        await Task.Delay(1000);
    }
    [Fact]
    public async void PublishAsync()
    {
        var (bus, provider) = CreateEventBus();
        var handler = new Handler(_output);
        provider.AddHandler(handler);
        var taskHandler = new TaskHandler(_output);
        provider.AddEventHandler(taskHandler);
        var asyncHandler = new AsyncHandler(_output);
        provider.AddHandler(asyncHandler);
        for (int i = 0; i < 100; i++)
        {
            await bus.PublishAsync("Event" + i);
            await Task.Delay(1);
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        await Task.Delay(1000);
    }

    public static (IEventAsyncBus bus, EventHandlerDictionaryProvider provider) CreateEventBus()
    {
        EventHandlerDictionaryProvider provider = new();
        //var waiter = new TaskForeachWaiter();
        EventAsyncBus bus = new(provider, new EventAsyncBusOptions() { TasksWhenAll = false });
        return (bus, provider);
    }
}
