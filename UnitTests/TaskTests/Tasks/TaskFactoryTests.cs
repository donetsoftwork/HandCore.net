using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Tasks;

public class TaskFactoryTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void ActionTimeout()
    {
        var factory = Task.Factory;
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
        var task = factory.StartNew(() => Hello("张三", 2000), tokenSource.Token);
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
    public async void ActionCancel()
    {
        var factory = Task.Factory;
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
    public async void ActionCancel2()
    {
        var factory = Task.Factory;
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        tokenSource.Cancel();
        var task = factory.StartNew(() => Hello("张三", 1000), tokenSource.Token);
        Assert.NotNull(task);
        await Task.Delay(100);
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
}
