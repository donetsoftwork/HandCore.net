using Hand.ConcurrentCollections;
using System.Diagnostics;
using Xunit.Abstractions;

namespace ConcurrentCollectionTests;

public class ConcurrentQueueTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public void Int()
    {
        var queue = new ConcurrentQueueAdapter<int>();
        TryDequeue(1);
        TryDequeue(10);
        TryDequeue(100);
        TryDequeue(1000);
        TryDequeue(10000);

        void TryDequeue(int num)
        {
            for (int i = 0; i < num; i++)
            {
                var item = i;
                queue.Enqueue(item);
            }
            var sw = Stopwatch.StartNew();
            queue.TryDequeue(out var first);
            sw.Stop();
            _output.WriteLine(first.ToString() + " Span :" + sw.Elapsed.TotalMilliseconds);
        }
    }
    [Fact]
    public void String()
    {
        var queue = new ConcurrentQueueAdapter<string>();
        TryDequeue(1);
        TryDequeue(10);
        TryDequeue(100);
        TryDequeue(1000);
        TryDequeue(10000);

        void TryDequeue(int num)
        {
            for (int i = 0; i < num; i++)
            {
                var item = "String"+ i;
                queue.Enqueue(item);
            }
            var sw = Stopwatch.StartNew();
            queue.TryDequeue(out var first);
            sw.Stop();
            _output.WriteLine(first.ToString() + " Span :" + sw.Elapsed.TotalMilliseconds);
        }
    }
    [Fact]
    public void Object()
    {
        var queue = new ConcurrentQueueAdapter<object>();
        TryDequeue(1);
        TryDequeue(10);
        TryDequeue(100);
        TryDequeue(1000);
        TryDequeue(10000);

        void TryDequeue(int num)
        {
            for (int i = 0; i < num; i++)
            {
                queue.Enqueue(new object());
            }
            var sw = Stopwatch.StartNew();
            queue.TryDequeue(out var first);
            sw.Stop();
            _output.WriteLine(first.ToString() + " Span :" + sw.Elapsed.TotalMilliseconds);
        }
    }

}