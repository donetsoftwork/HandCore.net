namespace TaskTests.Tasks;

public class StepTests
{
    [Fact]
    public async Task Step()
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(1));

        var tokenSource1 = new CancellationTokenSource();
        tokenSource1.CancelAfter(TimeSpan.FromSeconds(800));
        var token1 = tokenSource1.Token;
        var taskA = A(500, token1);
        var taskB = B(400, token1);
        var a = await taskA;
        var b = await taskB;

        var cancellationToken2 = new CancellationTokenSource();
        cancellationToken2.CancelAfter(TimeSpan.FromSeconds(600));
        var linked = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, cancellationToken2.Token);
        var taskC = C(400, a, b, linked.Token);
        var c = await taskC;
        Assert.Equal(3, c);
    }

    private static async Task<int> A(int arg, CancellationToken token)
    {
        await Task.Delay(arg, token);
        return 1;
    }
    private static async Task<int> B(int arg, CancellationToken token)
    {
        await Task.Delay(arg, token);
        return 2;
    }
    private static async Task<int> C(int arg, int a, int b, CancellationToken token)
    {
        await Task.Delay(arg, token);
        return a + b;
    }
}
