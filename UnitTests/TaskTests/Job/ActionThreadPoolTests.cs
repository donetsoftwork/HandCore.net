using Hand.Job;
using Hand.Tasks;
using Xunit.Abstractions;

namespace TaskTests.Job;

public class ActionThreadPoolTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async Task Action()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = new ActionThreadPool(options);
        pool.Start();
        pool.Add(() => Hello("张三"));
        pool.Add(() => Hello("李四"));
        await Task.Delay(1000);
    }
    [Fact]
    public async Task Wrap()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = new ActionThreadPool(options);
        pool.Start();
        var wrapper1 = TaskWrapper.Wrap(() => Hello("张三"));
        var wrapper2 = TaskWrapper.Wrap(() => Hello("李四"));
        pool.Add(wrapper1.Run);
        pool.Add(wrapper2.Run);
        await Task.WhenAll(wrapper1.Original, wrapper2.Original);
    }
    [Fact]
    public async Task Func()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 1 };
        var pool = new ActionThreadPool(options);
        pool.Start();
        var wrapper = TaskWrapper.Wrap(() => Multiply(9, 9));
        pool.Add(wrapper.Run);
        var result = await wrapper.Original;
        Assert.Equal(81, result);
    }
  [Fact]
    public async Task Concurrent()
    {
        var options = new ReduceOptions { ConcurrencyLevel = 10 };
        var pool = new ActionThreadPool(options);
        pool.Start();
        for (int i = 0; i < 100; i++)
        {
            var user = "User" + i;
            pool.Add(() => Hello(user));
        }
        await Task.Delay(1000);
    }


    void Hello(string name)
    {
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
        Thread.Sleep(1);
    }
    int Multiply(int a, int b)
    {
        var result = a * b;
        _output.WriteLine($"{a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
        return result;
    }
}
