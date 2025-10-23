using Hand.Events;
using Hand.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace EventBusTests;

public class ServiceProviderWhenAllTests
{
    private readonly IEventAsyncBus _eventBus;
    private readonly ITestOutputHelper _output;
    public ServiceProviderWhenAllTests(ITestOutputHelper output)
    {
        var services = new ServiceCollection()
            .ScanEventHandler(ServiceLifetime.Scoped, typeof(ServiceProviderTests).Assembly)
            //.AddEventHandlerProvider()
             //.AddSingleton<ITaskWait>(new TaskWhenAllWaiter())
             .AddSingleton(new EventAsyncBusOptions() { TasksWhenAll = true })
            .AddEventAsyncBus<EventAsyncBus>()
            .AddSingleton(output);
        var serviceProvider = services.BuildServiceProvider();
        _eventBus = serviceProvider.GetRequiredService<IEventAsyncBus>();
        _output = output;
    }
    [Fact]
    public void PublishAsync()
    {
        for (int i = 0; i < 10; i++)
            _eventBus.PublishAsync("Event" + i);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        Thread.Sleep(1000);
    }
}
