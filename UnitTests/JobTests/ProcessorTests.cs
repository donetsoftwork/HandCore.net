using Hand.Job;
using System.Diagnostics;
using Xunit.Abstractions;

namespace JobTests;

public class ProcessorTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void ActionState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var state = processor.Add(() => Hello("张三"));
        Assert.False(state.IsSuccess);
        pool.Start();
        await Task.Delay(1000);
        Assert.True(state.IsSuccess);
    }
    [Fact]
    public async void TaskState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var state = processor.AddTask(() => HelloAsync("张三"));
        Assert.False(state.IsSuccess);
        pool.Start();
        await Task.Delay(1000);
        Assert.True(state.IsSuccess);
    }
    [Fact]
    public async void FuncResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var result = processor.Add(() => Count(3));
        Assert.False(result.IsSuccess);
        pool.Start();
        await Task.Delay(1000);
        Assert.True(result.IsSuccess);
        var count = result.Result;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void TaskResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var result = processor.AddTask((t) => CountAsync(3, t), tokenSource.Token);
        Assert.False(result.IsSuccess);
        pool.Start();
        await Task.Delay(1000);
        Assert.True(result.IsSuccess);
        var count = result.Result;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void ActionCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        var state = processor.Add(() => Hello("张三"), tokenSource.Token);
        pool.Start();
        tokenSource.Cancel();
        await Task.Delay(1000);
        Assert.True(state.IsCancel);
    }
    [Fact]
    public async void TaskCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var state = processor.AddTask((t) => HelloAsync("张三", t), token);
        pool.Start();
        tokenSource.Cancel();
        await Task.Delay(1000);
        Assert.True(state.IsCancel);
    }
    [Fact]
    public async void ActionOneByOne()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            processor.Add(() => Hello(user));
        }
        await Task.Delay(1000);
    }
    [Fact]
    public async void ActionConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            processor.Add(() => Hello(user));
        }
        await Task.Delay(1000);
    }
    [Fact]
    public async void TaskOneByOne()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            processor.AddTask(() => HelloAsync(user));
        }
        await Task.Delay(1000);
    }
    [Fact]
    public async void TaskConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10, ItemLife = TimeSpan.FromSeconds(0.1), ReduceTime = TimeSpan.FromMicroseconds(1) };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            processor.AddTask(() => HelloAsync(user));
        }
        await Task.Delay(1000);
    }

    [Fact]
    public async void CurrentItem()
    {
        var sw = Stopwatch.StartNew();
        Hello("Item");
        sw.Stop();
        _output.WriteLine("Item Span :" + sw.Elapsed.TotalMilliseconds);

        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            processor.Add(() => Hello(user, 20));
        }
        var bugToken = new CancellationTokenSource();
        bugToken.CancelAfter(TimeSpan.FromMilliseconds(1000));
        processor.Add(() => Hello("Bug", 2000), bugToken.Token);
        for (int i = 10; i < 100; i++)
        {
            var user = "User" + i;
            processor.Add(() => Hello(user, 20));
        }
        await Task.Delay(5000);
    }

    [Fact]
    public async void LastTime()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            processor.Add(() => Hello(user, 20));
        }
        processor.Add(() => Hello("Bug", 2000));
        for (int i = 10; i < 100; i++)
        {
            var user = "User" + i;
            processor.Add(() => Hello(user, 20));
        }
        await Task.Delay(5000);
    }
    void Hello(string name, int time = 10)
    {
        Thread.Sleep(time);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
    }
    async Task HelloAsync(string name, CancellationToken token = default)
    {
        await Task.Delay(10, token);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} HelloAsync {name},{DateTime.Now:HH:mm:ss.fff}");
    }
    static int Count(int num)
    {
        int result = 0;
        for (int i = 1; i <= num; i++)
            result += i;
        return result;
    }
    static async Task<int> CountAsync(int num, CancellationToken token = default)
    {
        int result = 0;
        for (int i = 1; i <= num; i++)
        {
            await Task.Delay(1, token);
            result += i;
        }    
        return result;
    }
}
