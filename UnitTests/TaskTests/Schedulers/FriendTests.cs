using Hand.Job;
using Hand.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Schedulers;

/// <summary>
/// 网友测试
/// </summary>
public class FriendTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void TestRun()
    {
        var sw = Stopwatch.StartNew();
        var factory = new ConcurrentTaskFactory(new ReduceOptions { ConcurrencyLevel = 3 });
        List<Task<int>> tasks = new(30);
        for (int i = 0; i < 10; i++)
        {
            var index = i;
            var t1 = factory.StartTask(() => One(index));
            var t2 = factory.StartTask(() => Two(index));
            var t3 = factory.StartTask(() => Three(index));

            tasks.Add(t1);
            tasks.Add(t2);
            tasks.Add(t3);
        }

        await Task.WhenAll(tasks);
        sw.Stop();
        _output.WriteLine("Total Span :" + sw.Elapsed.TotalMilliseconds);
    }

    public async Task<int> One(int i, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} One Start {i}:{DateTime.Now:HH:mm:ss.fff}");
        await Task.Delay(100, token);
        token.ThrowIfCancellationRequested();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} One End {i}:{DateTime.Now:HH:mm:ss.fff}");
        return 1;
    }
    public async Task<int> Two(int i, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Two Start {i}:{DateTime.Now:HH:mm:ss.fff}");
        await Task.Delay(100, token);
        token.ThrowIfCancellationRequested();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Two End {i}:{DateTime.Now:HH:mm:ss.fff}");
        return 2;
    }
    public async Task<int> Three(int i, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Three Start {i}:{DateTime.Now:HH:mm:ss.fff}");
        await Task.Delay(100, token);
        token.ThrowIfCancellationRequested();
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Three End {i}:{DateTime.Now:HH:mm:ss.fff}");
        return 3;
    }
    [Fact]
    public async void Sum()
    {        
        var factory = new ConcurrentTaskFactory(new ReduceOptions { ConcurrencyLevel = 3 });
        var tokenSource = new CancellationTokenSource();
        var sw = Stopwatch.StartNew();
        var one = factory.StartTask(() => One(1, tokenSource.Token));
        var two = factory.StartTask(() => Two(2, tokenSource.Token));
        var three = factory.StartTask(() => Three(3, tokenSource.Token));
        //var one = One(1, tokenSource.Token);
        //var two = Two(2, tokenSource.Token);
        //var three = Three(3, tokenSource.Token);
        //await Task.Delay(100, tokenSource.Token);
        //tokenSource.Cancel();
        var list = await Task.WhenAll(one, two, three);
        var sum = list.Sum();
        sw.Stop();
        _output.WriteLine($"Total {sum} Span : {sw.Elapsed.TotalMilliseconds}");
        Assert.Equal(6, sum );
    }
    [Fact]
    public async void Cancel()
    {
        var factory = new ConcurrentTaskFactory(new ReduceOptions { ConcurrencyLevel = 10 });
        var tokenSource = new CancellationTokenSource(100);
        var sw = Stopwatch.StartNew();
        var onetTask = factory.StartTask(() => One(1, tokenSource.Token));
        //var twotTask = factory.StartTask(() => Two(2, tokenSource.Token));
        //var threeTask = factory.StartTask(() => Three(3, tokenSource.Token));
        await Task.Delay(50, tokenSource.Token);
        //tokenSource.Cancel();
        //var one = await onetTask;
        await Assert.ThrowsAsync<TaskCanceledException>(() => onetTask);
        //await Assert.ThrowsAsync<TaskCanceledException>(() => twotTask);
        //await Assert.ThrowsAsync<TaskCanceledException>(() => threeTask);
        //try
        //{
        //    await Task.WhenAll(onetTask, twotTask, threeTask);
        //    //await TimeoutHelper.ThrowIfTimeout(Task.WhenAll(onetTask, twotTask, threeTask), TimeSpan.FromMilliseconds(100));
        //}
        //catch (Exception ex)
        //{
        //    _output.WriteLine(ex.ToString());
        //}
        sw.Stop();
        _output.WriteLine("Total Span :" + sw.Elapsed.TotalMilliseconds);
    }
    [Fact]
    public async void Partial()
    {
        var factory = new ConcurrentTaskFactory(new ReduceOptions { ConcurrencyLevel = 1 });
        var tasks = new List<Task<int>>(100);
        var tokenSource = new CancellationTokenSource(9000);
        for (var i = 0; i < 10; i++)
        {
            var index = i;
            var task = factory.StartTask(async () => await One(index, tokenSource.Token));
            tasks.Add(task);
        }
        await Task.Delay(50, tokenSource.Token);
        var results = new ConcurrentBag<int>();
        try
        {
            //await Wait(tasks, results);
            foreach (var task in tasks)
            {
                var result = await task;
                results.Add(result);
            }
        }
        catch (Exception ex)
        {
            _output.WriteLine($"Exception: {ex}");
        }
        var sum = results.Sum();
        _output.WriteLine($"Sum：{sum}");
        Assert.True(sum > 0);
    }

    private static async Task Wait(List<Task<int>> tasks, ConcurrentBag<int> results)
    {
        foreach (var task in tasks)
        {
            var result = await task.ConfigureAwait(false);
            results.Add(result);
        }
    }
}
// Two Start 0:15:06:37.420
// Three Start 0:15:06:37.420
// One Start 0:15:06:37.420
// One End 0:15:06:38.425
// Two End 0:15:06:38.425
// Three End 0:15:06:38.425
// One Start 1:15:06:38.441
// Three Start 1:15:06:38.441
// Two Start 1:15:06:38.441
// Three End 1:15:06:39.448
// Two End 1:15:06:39.448
// One End 1:15:06:39.448
// One Start 2:15:06:39.464
// Three Start 2:15:06:39.464
// Two Start 2:15:06:39.464
// Two End 2:15:06:40.472
// One End 2:15:06:40.472
// Three End 2:15:06:40.472
// One Start 3:15:06:40.488
// Three Start 3:15:06:40.488
// Two Start 3:15:06:40.488
// Three End 3:15:06:41.494
// One End 3:15:06:41.494
// Two End 3:15:06:41.494
// One Start 4:15:06:41.510
// Three Start 4:15:06:41.510
// Two Start 4:15:06:41.510
// Three End 4:15:06:42.515
// One End 4:15:06:42.515
// Two End 4:15:06:42.515
// One Start 5:15:06:42.531
// Three Start 5:15:06:42.531
// Two Start 5:15:06:42.531
// Three End 5:15:06:43.537
// Two End 5:15:06:43.537
// One End 5:15:06:43.537
// One Start 6:15:06:43.553
// Three Start 6:15:06:43.553
// Two Start 6:15:06:43.553
// One End 6:15:06:44.561
// Three End 6:15:06:44.561
// Two End 6:15:06:44.561
// One Start 7:15:06:44.577
// Three Start 7:15:06:44.577
// Two Start 7:15:06:44.577
// One End 7:15:06:45.583
// Two End 7:15:06:45.583
// Three End 7:15:06:45.583
// One Start 8:15:06:45.599
// Three Start 8:15:06:45.599
// Two Start 8:15:06:45.599
// Two End 8:15:06:46.604
// One End 8:15:06:46.604
// Three End 8:15:06:46.604
// One Start 9:15:06:46.620
// Three Start 9:15:06:46.620
// Two Start 9:15:06:46.620
// One End 9:15:06:47.627
// Two End 9:15:06:47.627
// Three End 9:15:06:47.627
// Total Span :10267.3018
