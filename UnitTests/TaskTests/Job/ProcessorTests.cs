using Hand.Job;
using Hand.Tasks;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Job;

public class ProcessorTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void ActionState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 3 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var sw = Stopwatch.StartNew();
        var task = processor.StartNew(() => Hello("张三", 1000));
        var task2 = processor.StartNew(() => Hello("李四", 1000));
        var task3 = processor.StartNew(() => Hello("王二", 1000));
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void TaskState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 3 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var sw = Stopwatch.StartNew();
        var task = processor.StartTask(() => HelloAsync("张三", 1000, CancellationToken.None));
        var task2 = processor.StartTask(() => HelloAsync("李四", 1000, CancellationToken.None));
        var task3 = processor.StartTask(() => HelloAsync("王二", 1000, CancellationToken.None));
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void FuncResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var task = processor.StartNew(() => Count(3));
        var count = await task;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void TaskResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var task = processor.StartTask((t) => CountAsync(3, t), tokenSource.Token);
        var count = await task;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void ActionCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        var task = processor.StartNew(() => Hello("张三"), tokenSource.Token);
        tokenSource.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(() => task);
    }
    [Fact]
    public async void TaskCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var task = processor.StartTask((t) => HelloAsync("张三", 1000, t), token);
        tokenSource.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(() => task);
    }
    [Fact]
    public async void ActionOneByOne()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tasks = new List<Task>(10);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            var task = processor.StartNew(() => Hello(user));
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }
    [Fact]
    public async void ActionConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tasks = new List<Task>(100);
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            var task = processor.StartNew(() => Hello(user));
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }
    [Fact]
    public async void TaskOneByOne()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tasks = new List<Task>(10);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            var task = processor.StartTask(() => HelloAsync(user));
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }
    [Fact]
    public async void TaskConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tasks = new List<Task>(100);
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            var task = processor.StartTask(() => HelloAsync(user));
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }

    [Fact]
    public async void CurrentItem()
    {
        //var sw = Stopwatch.StartNew();
        //Hello("Item");
        //sw.Stop();
        //_output.WriteLine("Item Span :" + sw.Elapsed.TotalMilliseconds);

        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tasks = new List<Task>(100);
        for (int i = 0; i < 10; i++)
        {
            var user = "User" + i;
            var task = processor.StartNew(() => Hello(user, 20));
            tasks.Add(task);
        }
        var bugToken = new CancellationTokenSource();
        bugToken.CancelAfter(TimeSpan.FromMilliseconds(1000));
        var bug = TaskWrapper.Wrap(() => Hello("Bug", 2000), bugToken.Token);
        pool.Add(bug);
        for (int i = 10; i < 100; i++)
        {
            var user = "User" + i;
            var task = processor.StartNew(() => Hello(user, 20));
            tasks.Add(task);
        }
        await Assert.ThrowsAsync<TaskCanceledException>(() => bug.Task);
        await Task.WhenAll(tasks);
    }
    [Fact]
    public async void TaskItemLife()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var sw = Stopwatch.StartNew();
        var task = processor.StartTask(() => HelloAsync("张三", 2000, tokenSource.Token));
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
