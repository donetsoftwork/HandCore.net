using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Tasks;

public class TaskTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async void RunAction()
    {
        var sw = Stopwatch.StartNew();
        var task = Task.Run(() => Hello("张三", 1000));
        var task2 = Task.Run(() => Hello("李四", 1000));
        var task3 = Task.Run(() => Hello("王二", 1000));
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void ActionTimeout()
    {
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        // 此处CancelAfter对Task.Run没效果
        // 除非在Run之前已取消
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var task = Task.Run(() => Hello("张三", 2000), tokenSource.Token);
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
        //await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void RunAsync()
    {
        var sw = Stopwatch.StartNew();
        var task = Task.Run(() => HelloAsync("张三", 1000));
        var task2 = Task.Run(() => HelloAsync("李四", 1000));
        var task3 = Task.Run(() => HelloAsync("王二", 1000));
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
    }

    [Fact]
    public async void Async()
    {
        var sw = Stopwatch.StartNew();
        var task = HelloAsync("张三", 1000);
        var task2 = HelloAsync("李四", 1000);
        var task3 = HelloAsync("王二", 1000);
        await Task.WhenAll(task, task2, task3);
        sw.Stop();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
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
}
