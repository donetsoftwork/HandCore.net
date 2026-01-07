using Hand.ConcurrentCollections;
using Hand.Job;
using System.Diagnostics;
using Xunit.Abstractions;

namespace JobTests;

/// <summary>
/// ActionThreadPool测试
/// </summary>
/// <param name="output"></param>
public class ActionThreadPoolTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void Add()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = options.CreateJob(ActionProcessor.Instance);
        pool.Add(() => Hello("张三"));
        pool.Add(() => Hello("李四"));
        await Task.Delay(1000);
    }
    [Fact]
    public async void OneByOne()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = options.CreateJob(ActionProcessor.Instance);
        //pool.Start();
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            pool.Add(() => Hello(user));
        }
        await Task.Delay(1000);
    }
    [Fact]
    public async void Concurrent()
    {
        var sw = Stopwatch.StartNew();
        Hello("Item");
        sw.Stop();
        _output.WriteLine("Item Span :" + sw.Elapsed.TotalMilliseconds);

        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var pool = options.CreateJob(ActionProcessor.Instance);
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            pool.Add(() => Hello(user));
        }
        await Task.Delay(1000);
    }
    [Fact]
    public async void Stop()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = options.CreateJob(ActionProcessor.Instance);
        var queue = (pool.Queue as ConcurrentQueueAdapter<Action>)!;
        //pool.Start();
        pool.Add(() => Hello("张三"));
        await Task.Delay(100);
        Assert.Equal(0, queue.Count);
        pool.Stop();
        pool.Add(() => Hello("李四"));
        await Task.Delay(100);
        Assert.Equal(1, queue.Count);
    }

    void Hello(string name)
    {
        Thread.Sleep(10);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
    }
}
