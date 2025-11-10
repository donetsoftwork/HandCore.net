using Hand.Job;
using Hand.States;
using Hand.Tasks;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Tasks;

public class ConcurrentTaskFactoryTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async void ActionState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 3 };
        var factory = new ConcurrentTaskFactory(options);
        var sw = Stopwatch.StartNew();
        var task = factory.StartNew(() => Hello("张三", 1000));
        var task2 = factory.StartNew(() => Hello("李四", 1000));
        var task3 = factory.StartNew(() => Hello("王二", 1000));
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void ActionItemLife()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
        var factory = new ConcurrentTaskFactory(options);
        var sw = Stopwatch.StartNew();
        var task = factory.StartNew(() => Hello("张三", 2000));
        Assert.NotNull(task);
        try
        {
            await task;
            sw.Stop();
        }
        catch (Exception ex)
        {
            sw.Stop();
            _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
        await Task.Delay(1000);
    }
    [Fact]
    public async void TaskState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 3 };
        var factory = new ConcurrentTaskFactory(options);
        var sw = Stopwatch.StartNew();
        var task = factory.StartTask(() => HelloAsync("张三", 1000));
        var task2 = factory.StartTask(() => HelloAsync("李四", 1000));
        var task3 = factory.StartTask(() => HelloAsync("王二", 1000));
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void TaskItemLife()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var sw = Stopwatch.StartNew();
        var task = factory.StartTask(() => HelloAsync("张三", 2000, tokenSource.Token));
        Assert.NotNull(task);
        try
        {
            await task;
            sw.Stop();
        }
        catch (Exception ex)
        {
            sw.Stop();
            _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
        await Task.Delay(2000);
    }
    [Fact]
    public async void FuncResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var task = factory.StartNew(() => Count(3));
        Assert.NotNull(task);
        var count = await task;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void TaskResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var task = factory.StartTask((t) => CountAsync(3, t), tokenSource.Token);
        var count = await task;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void ActionCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        var task = factory.StartNew(() => Hello("张三", 1000), tokenSource.Token);
        Assert.NotNull(task);
        await Task.Delay(100);
        tokenSource.Cancel();
        //await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        try
        {
            await task;
            sw.Stop();
        }
        catch (Exception ex)
        {
            sw.Stop();
            _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
        await Task.Delay(1000);
    }
    [Fact]
    public async void ActionTimeout()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var task = factory.StartNew(() => Hello("张三", 2000), tokenSource.Token);
        Assert.NotNull(task);
        //await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        try
        {
            await task;
            sw.Stop();
        }
        catch (Exception ex)
        {
            sw.Stop();
            _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
        await Task.Delay(1000);
    }
    [Fact]
    public async void TaskCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        var task = factory.StartTask((t) => HelloAsync("张三", 500, t), tokenSource.Token);
        tokenSource.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
    }
    [Fact]
    public async void TaskCancel0()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var token = tokenSource.Token;
        var task = factory.StartTask((t) => HelloAsync("张三", 500, t), token);
        var task2 = factory.StartTask((t) => HelloAsync("李四", 500, t), token);
        var task3 = factory.StartTask((t) => HelloAsync("王二", 500, t), token);
        try
        {
            await Task.WhenAll(task, task2, task3);
            sw.Stop();
        }
        catch (Exception ex)
        {
            sw.Stop();
            _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
        await Task.Delay(2000);
    }
    [Fact]
    public async void TestTask()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var task = factory.StartNew(() => Multiply(2, 3));
        var result = await task;
        Assert.Equal(6, result);
    }
    [Fact]
    public async void Cancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromMicroseconds(1));
        var task = factory.StartNew(() => Multiply(2, 3), tokenSource.Token);
        await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
    }
    [Fact]
    public async void TestConcurrent0()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var factory = new ConcurrentTaskFactory(options);
        Start(factory);
        await Task.Delay(5000);
    }
    [Fact]
    public async void OneByone()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        Start(factory);
        await Task.Delay(10000);
    }
    [Fact]
    public async void TestConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var factory = new ConcurrentTaskFactory(options);
        Start(factory);
        Wait(factory.Job);
        //Start(factory);
        //Wait(jobService);
        await Task.Delay(5000);
    }

    [Fact]
    public async void TaskOneByone()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        StartTask(factory);
        await Task.Delay(10000);
    }
    [Fact]
    public async void TaskConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var factory = new ConcurrentTaskFactory(options);
        StartTask(factory);
        await Task.Delay(10000);
    }
    private void StartTask(ConcurrentTaskFactory factory)
    {
        for (int i = 1; i < 100; i++)
        {
            var user = "User" + i;
            factory.StartTask(() => HelloAsync(user));
        }
    }

    private void Start(TaskFactory factory)
    {
        for (int i = 1; i < 10; i++)
        {
            for (int j = 1; j < 10; j++)
            {
                int a = i, b = j;
                factory.StartNew(() => Multiply(a, b));
            }
        }
    }
    private void Wait(ReduceJobService<IState<bool>> jobService)
    {
        var pool = jobService.Pool;
        var scheduler = (jobService.Processor as QueueTaskScheduler)!;
        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(50);
            //var count = scheduler.Concurrency;
            var activeCount = pool.ActiveCount;
            var poolCount = pool.PoolCount;
            if (activeCount + poolCount > 0)
            {
                _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} task:{scheduler.Count}, pool:{activeCount}/{poolCount}");
            }
        }
    }


    int Multiply(int a, int b)
    {
        var result = a * b;
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} {a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
        Thread.Sleep(100);
        return result;
    }
    void Hello(string name, int time = 10)
    {
        Thread.Sleep(time);
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
    }
    async Task HelloAsync(string name, int time = 10, CancellationToken token = default)
    {
        await Task.Delay(time, token);
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
