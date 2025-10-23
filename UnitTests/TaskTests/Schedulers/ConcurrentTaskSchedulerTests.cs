using Hand.Concurrent;
using Hand.Job;
using Hand.Tasks;
using Xunit.Abstractions;

namespace TaskTests.Schedulers;

public class ConcurrentTaskSchedulerTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async Task TestRun()
    {
        var concurrency = new ConcurrentControl(10);
        var scheduler = new ConcurrentTaskScheduler(concurrency);
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Add(1, 2));
        scheduler.Run();
        var result = await task;
        Assert.Equal(3, result);
    }
    [Fact]
    public static async void TestTask()
    {
        var concurrency = new ConcurrentControl(10);
        var scheduler = new ConcurrentTaskScheduler(concurrency);
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Add(1, 2));
        var jobService = new ThreadJobService(scheduler);
        jobService.Start();
        var result = await task;
        Assert.Equal(3, result);
    }
    [Fact]
    public void TestConcurrent()
    {
        var concurrency = new ConcurrentControl(3);
        var scheduler = new ConcurrentTaskScheduler(concurrency);
        var factory = new TaskFactory(scheduler);
        var jobService = new ConcurrentJobService(scheduler, TimeSpan.FromMicroseconds(50), concurrency.Limit);
        jobService.Start();
        Start(factory);
        Wait(jobService, concurrency);
        Start(factory);
        Wait(jobService, concurrency);
    }

    private static void Start(TaskFactory factory)
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                factory.StartNew(() => Add(i, j));
            }
        }
    }
    private void Wait(ConcurrentJobService jobService, ConcurrentControl concurrency)
    {
        var pool = jobService.Pool;
        var scheduler = (jobService.Processor as ConcurrentTaskScheduler)!;
        for (int i = 0; i < 30; i++)
        {
            Thread.Sleep(50);
            var count = concurrency.Count;
            var activeCount = pool.ActiveCount;
            var poolCount = pool.PoolCount;
            Assert.True(count + activeCount + poolCount > 0);
            _output.WriteLine($"task:{count}/{scheduler.TaskCount}, pool:{activeCount}/{poolCount}");
        }
    }


    public static int Add(int a, int b)
    {
        var result = a + b;
        Console.WriteLine($"reslut:{result}");
        Thread.Sleep(result % 20 + 20);
        return result;
    }
}
