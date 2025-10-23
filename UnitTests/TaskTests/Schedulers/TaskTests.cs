using TaskTests.Supports;

namespace TaskTests.Schedulers;

public class TaskTests
{
    [Fact]
    public async void New()
    {
        var result = 0;
        
        var scheduler = new MyScheduler();
        var factory = new TaskFactory(scheduler);
        var task = factory.StartNew(() => result = Add(1, 2));
        await Task.Delay(100);
        Assert.Equal(3, result);
    }


    private static int Add(int a, int b)
    {
        return a + b;
    }
}
