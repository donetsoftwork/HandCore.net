using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Tasks;

public class AsyncTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async void Async()
    {
        var sw = Stopwatch.StartNew();
        var one = await One();
        var two = await Two();
        var tree = await Tree();
        var sum = await Sum(one, two, tree);
        Assert.Equal(6, sum);
        sw.Stop();
        _output.WriteLine($"Async Total Time: {sw.ElapsedMilliseconds} ms");
    }
    [Fact]
    public async void Parallel()
    {
        var sw = Stopwatch.StartNew();
        var oneTask = One();
        var twoTask = Two();
        var treeTask = Tree();
        var one = await oneTask;
        var two = await twoTask;
        var tree = await treeTask;
        var sum = await Sum(one, two, tree);
        Assert.Equal(6, sum);
        sw.Stop();
        _output.WriteLine($"Parallel Async Total Time: {sw.ElapsedMilliseconds} ms");
    }
    [Fact]
    public async void WhenAll()
    {
        var sw = Stopwatch.StartNew();
        var oneTask = One();
        var twoTask = Two();
        var treeTask = Tree();
        var list = await Task.WhenAll(oneTask, twoTask, treeTask);
        var sum = await Sum(list);
        Assert.Equal(6, sum);
        sw.Stop();
        _output.WriteLine($"WhenAll Async Total Time: {sw.ElapsedMilliseconds} ms");
    }

    public static async Task<int> Sum(params int[] input)
    {
        await Task.Delay(1000);
        return input.Sum();
    }
    public static async Task<int> One()
    {
        await Task.Delay(1000);
        return 1;
    }
    public static async Task<int> Two()
    {
        await Task.Delay(1000);
        return 2;
    }
    public static async Task<int> Tree()
    {
        await Task.Delay(1000);
        return 3;
    }
}
