using Hand.Job;
using Hand.Tasks;
using Xunit.Abstractions;

namespace TaskTests.Schedulers;

public class FriendTest2(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async Task TestRun()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 3 };
        var factory = new ConcurrentTaskFactory(options);
        var task = factory.StartTask(HelloAsync);
        var task2 = factory.StartNew(Hello);
        Assert.NotNull(task);
        Assert.NotNull(task2);
        await Task.WhenAll(task, task2);
    }

    void Hello()
    {
        Thread.Sleep(10);
        _output.WriteLine($"Hello:{DateTime.Now:HH:mm:ss.fff}");
    }
    async Task HelloAsync()
    {
        await Task.Delay(10);
        _output.WriteLine($"HelloAsync:{DateTime.Now:HH:mm:ss.fff}");
    }
}
