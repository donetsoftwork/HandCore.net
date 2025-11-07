using Hand;
using Hand.Concurrent;
using Hand.Job;
using Hand.States;
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
        var scheduler = new QueueTaskScheduler();
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Multiply(1, 2));
        await Assert.ThrowsAsync<TimeoutException>(async () => await TimeoutHelper.ThrowIfTimeout(task, TimeSpan.FromSeconds(1)));
    }
    [Fact]
    public async void TestTask()
    {
        var options = new ConcurrentOptions { ConcurrencyLevel = 10 };
        var scheduler = new QueueTaskScheduler();
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => Multiply(2, 3));
        var jobService = new ThreadJobService<IState<bool>> (scheduler.Queue, scheduler);
        //jobService.Start();
        var result = await task;
        Assert.Equal(6, result);
    }
    [Fact]
    public async void TestConcurrent0()
    {
        var options = new TaskFactoryOptions { ConcurrencyLevel = 10 };
        var factory = new ConcurrentTaskFactory(options);
        Start(factory);
        await Task.Delay(10000);
    }
    [Fact]
    public async void TestConcurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var scheduler = new QueueTaskScheduler();
        var factory = new TaskFactory(scheduler);
        var jobService = options.CreateJob(scheduler.Queue, scheduler);
        //jobService.Start();
        Start(factory);
        await Wait(jobService);
        //Start(factory);
        //Wait(jobService);
        await Task.Delay(5000);
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
    private async Task Wait(ReduceJobService<IState<bool>> jobService)
    {
        var pool = jobService.Pool;
        var scheduler = (jobService.Processor as QueueTaskScheduler)!;
        for (int i = 0; i < 30; i++)
        {
            await Task.Delay(50);
            var activeCount = pool.ActiveCount;
            var poolCount = pool.PoolCount;
            if (activeCount + poolCount > 0)
            {
                _output.WriteLine($"task:{scheduler.Count}, pool:{activeCount}/{poolCount}");
            }
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
