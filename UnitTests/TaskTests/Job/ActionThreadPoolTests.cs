using Hand.Job;
using Xunit.Abstractions;

namespace TaskTests.Job;

public class ActionThreadPoolTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async Task Action()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = options.CreateJob(ActionProcessor.Instance);
        pool.Add(() => Hello("张三"));
        pool.Add(() => Hello("李四"));
        await Task.Delay(1000);
    }
    [Fact]
    public async Task Concurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var pool = options.CreateJob(ActionProcessor.Instance);
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            pool.Add(() => Hello(user));
        }
        await Task.Delay(1000);
    }


    void Hello(string name)
    {
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
        Thread.Sleep(1);
    }
}
