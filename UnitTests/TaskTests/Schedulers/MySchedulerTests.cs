
using TaskTests.Supports;

namespace TaskTests.Schedulers;

public class MySchedulerTests
{
    [Fact]
    public async void TestSquareAsyn()
    {
        var reslut = await SquareAsyn(2);
        Assert.Equal(4, reslut);
    }

    [Fact]
    public async void TestAsyncFunc()
    {
        var reslut = await AsyncFunc(1, 1);
        Assert.Equal(4, reslut);
    }
    [Fact]
    public async void TestScheduler()
    {
        var scheduler = new MyScheduler();
        var factory = new TaskFactory(scheduler);
        var task1 = factory.StartNew(() => AsyncFunc(1, 1));
        Assert.NotNull(task1);
        var task2 = await task1;
        Assert.NotNull(task2);
        var reslut = await task2;
        Assert.Equal(4, reslut);
    }

    private static Task<int> AsyncFunc(int a, int b)
    {
        var num = a + b;
        return SquareAsyn(num);
    }
    private static async Task<int> SquareAsyn(int num)
    {
        await Task.Delay(1);
        return num + num;
    }
}