using Hand.Collections;
using Hand.Job;
using Xunit.Abstractions;

namespace JobTests;

/// <summary>
/// 自定义处理器测试
/// </summary>
/// <param name="output"></param>
public class MyProcessorTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async Task Add()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var processor = new MyProcessor(_output);
        var pool = options.CreateJob(processor);
        pool.Add("张三");
        pool.Add("李四");
        await Task.Delay(1000);
    }

    [Fact]
    public async Task Concurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var processor = new MyProcessor(_output);
        var pool = options.CreateJob(processor);
        for (int i = 0; i < 100; i++)
        {
            pool.Add("User" + i);
        }
        await Task.Delay(1000);
        await Task.Delay(1000);
    }

    /// <summary>
    /// 自定义处理器
    /// </summary>
    /// <param name="output"></param>
    class MyProcessor(ITestOutputHelper output)
         : IQueueProcessor<string>
    {
        private readonly ITestOutputHelper _output = output;
        /// <inheritdoc />
        public async void Run(IQueue<string> queue, ThreadJobService<string> service, CancellationToken token)
        {
            while (queue.TryDequeue(out var item))
            {
                if (service.Activate(item))
                    await RunItemAsync(item, token);
                if (token.IsCancellationRequested)
                    break;
            }
            // 用完释放(回收)
            service.Dispose();
        }
        /// <summary>
        /// 执行单个
        /// </summary>
        /// <param name="item"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task RunItemAsync(string item, CancellationToken token)
        {
            _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {item},{DateTime.Now:HH:mm:ss.fff}");
            try
            {
                await Task.Delay(10, token);
            }
            catch { }
        }
    }
}
