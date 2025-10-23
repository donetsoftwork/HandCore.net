namespace TaskTests.Tasks;

public class ContinueWithTests
{
    [Fact]
    public async void FromResult()
    {
        int result = 0;
        var task = Task.FromResult(1);
        await Task.Delay(1);
        var task1 = task.ContinueWith(t => 
        {
            result = t.Result;
            Console.WriteLine(result);
        });
        await Task.Delay(1);
        Assert.Equal(1, result);
    }
}
