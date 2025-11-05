using Hand.Events;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace EventBusTests;

public class ServiceProviderTests
{
    private readonly IEventBus _eventBus;
    private readonly ITestOutputHelper _output;
    public ServiceProviderTests(ITestOutputHelper output)
    {
        var services = new ServiceCollection()
            .ScanEventHandler(ServiceLifetime.Scoped, typeof(ServiceProviderTests).Assembly)
            //.AddEventHandlerProvider()
            .AddSingleton(new EventBusOptions { ConcurrencyLevel = 1 })
            .AddSingleton<EventDispatcher>()
            .AddEventBus<EventBus>()
            .AddSingleton(output);
        var serviceProvider = services.BuildServiceProvider();
        _eventBus = serviceProvider.GetRequiredService<IEventBus>();
        _output = output;
    }
    [Fact]
    public void Publish()
    {
        for (int i = 0; i < 100; i++)
            _eventBus.Publish("Event" + i);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Sleep");
        Thread.Sleep(1000);
    }
}
