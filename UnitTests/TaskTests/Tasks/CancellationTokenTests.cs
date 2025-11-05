using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Tasks;

public class CancellationTokenTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact]
    public async void ThrowIfCancellationRequested()
    {
        int result = 0;
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        var sw = Stopwatch.StartNew();
        var task = CountAsynWithThrowIfCancellationRequested(1000, tokenSource.Token);
        await Task.Delay(1000, CancellationToken.None);
        tokenSource.Cancel();
        try
        {
            result = await task;
        }
        catch (Exception ex)
        {
            _output.WriteLine(ex.ToString());
        }
        sw.Stop();
        _output.WriteLine($"Result: {result} Elapsed:{sw.Elapsed.TotalMilliseconds}");
    }
    [Fact]
    public async void IsCancellationRequested()
    {
        var tokenSource = new CancellationTokenSource();
        tokenSource.CancelAfter(TimeSpan.FromSeconds(10));
        var sw = Stopwatch.StartNew();
        var task = CountAsynWithIsCancellationRequested(1000, tokenSource.Token);
        await Task.Delay(1000, CancellationToken.None);
        tokenSource.Cancel();
        var result = await task;
        sw.Stop();
        _output.WriteLine($"Result: {result} Elapsed:{sw.Elapsed.TotalMilliseconds}");
    }

    private static async Task<int> CountAsynWithThrowIfCancellationRequested(int num, CancellationToken token)
    {
        var count = 0;
        for (int i = 0; i < num; i++)
        {
            await Task.Delay(i, CancellationToken.None);
            token.ThrowIfCancellationRequested();
            count += i;
        }
        return count;
    }
    private static async Task<int> CountAsynWithIsCancellationRequested(int num, CancellationToken token)
    {
        var count = 0;
        for (int i = 0; i < num; i++) 
        {            
            await Task.Delay(i, CancellationToken.None);
            if (token.IsCancellationRequested)
                break;
            count += i;
        }
        return count;
    }
}
