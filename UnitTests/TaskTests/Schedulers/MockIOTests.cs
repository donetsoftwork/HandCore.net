using Hand.Concurrent;
using Hand.Job;
using Hand.Tasks;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace TaskTests.Schedulers;

public class MockIOTests(ITestOutputHelper output)
{
    private const int _concurrentLimit = 6;
    private readonly ConcurrentControl _control = new();
    private readonly ITestOutputHelper _output = output;

    internal async Task<Product> GetProductAsync(int id)
    {
        _control.Increment();
        await Task.Delay(100);
        var concurrent = _control.Count;
        _control.Decrement();
        if (concurrent > _concurrentLimit)
        {
            throw new Exception("Server is busy!!!");
        }
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} GetProductAsync({id}),{DateTime.Now:HH:mm:ss.fff}");
        return new(id);
    }
    [Fact]
    public async void Foreach()
    {
        _output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
        Stopwatch sw = Stopwatch.StartNew();
        List<Product> products = new(10);
        for (int i = 0; i < 10; i++)
        {
            var id = i;
            var item = await GetProductAsync(id);
            products.Add(item);
        }
        sw.Stop();
        _output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
        Assert.True(sw.ElapsedMilliseconds > 1000);
    }

    [Fact]
    public async void WhenAll()
    {
        _output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
        Stopwatch sw = Stopwatch.StartNew();
        List<Task<Product>> tasks = new(10);
        for (int i = 0; i < 10; i++)
        {
            var id = i;
            var task = GetProductAsync(id);
            tasks.Add(task);
        }
        var products = await Task.WhenAll(tasks);
        // await Assert.ThrowsAsync<Exception>(async () => await Task.WhenAll(tasks));
        sw.Stop();
        _output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
        Assert.True(sw.ElapsedMilliseconds < 200);
    }
    [Fact]
    public async void StartTask()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 6 };
        var factory = new ConcurrentTaskFactory(options);
        _output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
        Stopwatch sw = Stopwatch.StartNew();
        List<Task<Product>> tasks = new(10);
        for (int i = 0; i < 10; i++)
        {
            var id = i;
            var task = factory.StartTask(() => GetProductAsync(id));
            tasks.Add(task);
        }
        var products = await Task.WhenAll(tasks);
        sw.Stop();
        _output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
        Assert.True(sw.ElapsedMilliseconds < 300);
    }
    [Fact]
    public async void Single()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var factory = new ConcurrentTaskFactory(options);
        _output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
        Stopwatch sw = Stopwatch.StartNew();
        List<Task<Product>> tasks = new(10);
        for (int i = 0; i < 10; i++)
        {
            var id = i;
            var task = factory.StartTask(() => GetProductAsync(id));
            tasks.Add(task);
        }
        var products = await Task.WhenAll(tasks);
        sw.Stop();
        _output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
        Assert.NotNull(products);
        Assert.Equal(10, products.Length);
    }
    [Fact]
    public async void Concurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 4 };
        var factory = new ConcurrentTaskFactory(options);
        _output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
        Stopwatch sw = Stopwatch.StartNew();
        List<Task<Product>> tasks = new(100);
        for (int i = 0; i < 100; i++)
        {
            var id = i;
            var task = factory.StartTask(() => GetProductAsync(id));
            tasks.Add(task);
        }
        var products = await Task.WhenAll(tasks);
        sw.Stop();
        _output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
        Assert.NotNull(products);
        Assert.Equal(100, products.Length);
    }


    internal record Product(int Id);
}
