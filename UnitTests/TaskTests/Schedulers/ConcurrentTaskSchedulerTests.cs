using Hand.Concurrent;
using Hand.Job;
using Hand.Tasks;
using Xunit.Abstractions;

namespace TaskTests.Schedulers;

public class ConcurrentTaskSchedulerTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async Task Timeout()
    {
        var options = new ConcurrentOptions { ConcurrencyLevel = 10 };
        var scheduler = new ConcurrentTaskScheduler(options);
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Multiply(1, 2));
        await Assert.ThrowsAsync<TimeoutException>(async () => await TimeoutHelper.ThrowIfTimeout(task, TimeSpan.FromSeconds(1)));
    }
    [Fact]
    public async Task TestRun()
    {
        var options = new ConcurrentOptions { ConcurrencyLevel = 10 };
        var scheduler = new ConcurrentTaskScheduler(options);
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Multiply(1, 2));
        scheduler.Run();
        var result = await TimeoutHelper.ThrowIfTimeout(task, TimeSpan.FromSeconds(1));
        Assert.Equal(3, result);
    }
    [Fact]
    public async void TestTask()
    {
        var options = new ConcurrentOptions { ConcurrencyLevel = 10 };
        var scheduler = new ConcurrentTaskScheduler(options);
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Multiply(1, 2));
        var jobService = new ThreadJobService(scheduler);
        jobService.Start();
        var result = await task;
        Assert.Equal(3, result);
    }
    [Fact]
    public void TestConcurrent0()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var scheduler = new ConcurrentTaskScheduler(options);
        var factory = new TaskFactory(scheduler);
        var jobService = new ConcurrentJobService(scheduler, options);
        jobService.Start();
        Start(factory);
        Thread.Sleep(5000);
    }
    [Fact]
    public void TestConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var scheduler = new ConcurrentTaskScheduler(options);
        var factory = new TaskFactory(scheduler);
        var jobService = new ConcurrentJobService(scheduler, options);
        jobService.Start();
        Start(factory);
        Wait(jobService);
        Start(factory);
        Wait(jobService);
        Thread.Sleep(5000);
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
    private void Wait(ConcurrentJobService jobService)
    {
        var pool = jobService.Pool;
        var scheduler = (jobService.Processor as ConcurrentTaskScheduler)!;
        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(50);
            var count = scheduler.Concurrency;
            var activeCount = pool.ActiveCount;
            var poolCount = pool.PoolCount;
            Assert.True(count + activeCount + poolCount > 0);
            _output.WriteLine($"task:{count}/{scheduler.TaskCount}, pool:{activeCount}/{poolCount}");
        }
    }


    int Multiply(int a, int b)
    {
        var result = a * b;
        _output.WriteLine($"{a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
        Thread.Sleep(100);
        return result;
    }
}
