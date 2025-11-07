using Hand.Job;
using Hand.Tasks;
using Xunit.Abstractions;

namespace TaskTests.Job;

public class ProcessorTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void ActionState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var state = TaskWrapper.Wrap(() => Hello("张三"));
        pool.Add(state);
        pool.Start();
        await state.Task;
    }
    [Fact]
    public async void TaskState()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var state = TaskWrapper.Wrap(() => HelloAsync("张三"));
        pool.Add(state);
        pool.Start();
        await state.Task;
    }
    [Fact]
    public async void FuncResult()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var result = TaskWrapper.Wrap(() => Count(3));
        pool.Add(result);
        pool.Start();
        var count = await result.Task;
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
        var result = TaskWrapper.Wrap((t) => CountAsync(3, t), tokenSource.Token);
        pool.Add(result);
        pool.Start();
        var count = await result.Task;
        Assert.Equal(6, count);
    }
    [Fact]
    public async void ActionCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        var state = TaskWrapper.Wrap(() => Hello("张三"), tokenSource.Token);
        pool.Add(state);
        pool.Start();
        tokenSource.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(() => state.Task);
    }
    [Fact]
    public async void TaskCancel()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
        var processor = new Processor();
        var pool = options.CreateJob(processor);
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        var state = TaskWrapper.Wrap((t) => HelloAsync("张三", t), token);
        pool.Add(state);
        pool.Start();
        tokenSource.Cancel();
        await Assert.ThrowsAsync<TaskCanceledException>(() => state.Task);
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
            var state = TaskWrapper.Wrap(() => Hello(user));
            pool.Add(state);
            tasks.Add(state.Task);
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
            var state = TaskWrapper.Wrap(() => Hello(user));
            pool.Add(state);
            tasks.Add(state.Task);
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
            var state = TaskWrapper.Wrap(() => HelloAsync(user));
            pool.Add(state);
            tasks.Add(state.Task);
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
            var state = TaskWrapper.Wrap(() => HelloAsync(user));
            pool.Add(state);
            tasks.Add(state.Task);
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
            var state = TaskWrapper.Wrap(() => Hello(user, 20));
            pool.Add(state);
            tasks.Add(state.Task);
        }
        var bugToken = new CancellationTokenSource();
        bugToken.CancelAfter(TimeSpan.FromMilliseconds(1000));
        var bug = TaskWrapper.Wrap(() => Hello("Bug", 2000), bugToken.Token);
        pool.Add(bug);
        for (int i = 10; i < 100; i++)
        {
            var user = "User" + i;
            var state = TaskWrapper.Wrap(() => Hello(user, 20));
            pool.Add(state);
            tasks.Add(state.Task);
        }
        await Assert.ThrowsAsync<TaskCanceledException>(() => bug.Task);
        await Task.WhenAll(tasks);
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
