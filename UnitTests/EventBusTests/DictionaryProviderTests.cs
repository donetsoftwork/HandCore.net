using EventBusTests.Supports;
using Hand.Events;
using Hand.Reflection;
using Xunit.Abstractions;

namespace EventBusTests;

public class DictionaryProviderTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public void EventHandler()
    {
        var (bus, provider) = CreateEventBus();
        var handler = new Handler(_output);
        provider.AddHandler(handler);
        for (int i = 0; i < 100; i++)
            bus.Publish("Event" + i);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        Thread.Sleep(1000);
    }
    [Fact]
    public void TaskEventHandler()
    {
        //TaskInfo
        var (bus, provider) = CreateEventBus();
        var taskHandler = new TaskHandler(_output);
        provider.AddTaskHandler(taskHandler);
        for (int i = 0; i < 100; i++)
            bus.Publish("Event" + i);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        Thread.Sleep(1000);
    }
    [Fact]
    public void AsyncEventHandler()
    {
        var (bus, provider) = CreateEventBus();
        var asyncHandler = new AsyncHandler(_output);
        provider.AddHandler(asyncHandler);
        for (int i = 0; i < 100; i++)
            bus.Publish("Event" + i);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        Thread.Sleep(1000);
    }
    [Fact]
    public void Publish()
    {
        var (bus, provider) = CreateEventBus();
        var handler = new Handler(_output);
        provider.AddHandler(handler);
        var taskHandler = new TaskHandler(_output);
        provider.AddTaskHandler(taskHandler);
        //var asyncHandler = new AsyncHandler(_output);
        //provider.AddHandler(asyncHandler);
        for (int i = 0; i < 100; i++)
            bus.Publish("Event" + i);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        Thread.Sleep(1000);
    }

    public static (IEventBus bus, EventHandlerDictionaryProvider provider) CreateEventBus()
    {
        EventHandlerDictionaryProvider provider = new();
        EventBus bus = new(provider, new EventBusOptions { ConcurrencyLevel = 1 });
        return (bus, provider);
    }
}