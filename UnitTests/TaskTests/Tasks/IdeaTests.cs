using System.Collections.Concurrent;
using System.Diagnostics;
using Xunit.Abstractions;

namespace TaskTests.Tasks;

public class IdeaTests(ITestOutputHelper output)
{
    private readonly ITestOutputHelper _output = output;
    [Fact]
    public async void Pass()
    {
        var deadlineTime = 1000;
        var idea = new Idea(deadlineTime, 5, 10, 50, 100, 500, 1000, 2000, 10000);
        var sw = Stopwatch.StartNew();
        var result = await idea.VoteAsync();
        sw.Stop();
        Assert.True(result);
        _output.WriteLine("Total Span :" + sw.Elapsed.TotalMilliseconds);
        
    }
    [Fact]
    public async void Fail()
    {
        var deadlineTime = 1000;
        var idea = new Idea(deadlineTime, 5, 11, 51, 100, 501, 911, 2000, 10000);
        var sw = Stopwatch.StartNew();
        var result = await idea.VoteAsync();
        sw.Stop();
        Assert.False(result);
        _output.WriteLine("Total Span :" + sw.Elapsed.TotalMilliseconds);
    }
    [Fact]
    public async void Fail2()
    {
        var deadlineTime = 1000;
        var idea = new Idea(deadlineTime, 99, 500, 1000, 5000, 8000, 9000, 10000);
        var sw = Stopwatch.StartNew();
        var result = await idea.VoteAsync();
        sw.Stop();
        Assert.False(result);
        _output.WriteLine("Total Span :" + sw.Elapsed.TotalMilliseconds);
    }
    /// <summary>
    /// 共识投票
    /// </summary>
    /// <param name="deadlineTime"></param>
    /// <param name="members"></param>
    class Idea(int deadlineTime, params int[] members)
    {
        private readonly int _deadlineTime = deadlineTime;
        private readonly int[] _members = members;
        private readonly ConcurrentBag<int> _agreed = [];
        private readonly ConcurrentBag<int> _rejected = [];
        /// <summary>
        /// 响应
        /// </summary>
        /// <param name="member"></param>
        /// <param name="state"></param>
        public void Response(int member, bool state)
        {
            if (state)
                _agreed.Add(member);
            else
                _rejected.Add(member);
        }
        /// <summary>
        /// 异步投票
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VoteAsync()
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(_deadlineTime);
            var token = tokenSource.Token;
            var tasks = new List<Task>();
            foreach (var item in _members.OrderBy(item => item))
            {
                var member = new Member(item);
                var task = member.RequestAsync(this, token);
                tasks.Add(task);
            }
            int count = _members.Length;
            var passCount = count/2 + 1;
            var failCount = count - passCount;
            //try
            //{
            //    await Task.Delay(_deadlineTime, token);
            //}
            //catch { }
            //tokenSource.Cancel();
            foreach (var task in tasks)
            {
                try
                {
                    await task;
                    if(_agreed.Count >= passCount)
                        return true;
                    if (_rejected.Count >= failCount)
                        return false;
                }
                catch { break; }
            }
            if (_agreed.Count >= passCount)
                return true;
            return false;
        }
    }
    /// <summary>
    /// 成员
    /// </summary>
    /// <param name="value"></param>
    class Member(int value)
    {
        public int Value { get; set; } = value;
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="idea"></param>
        public async Task RequestAsync(Idea idea, CancellationToken token)
        {
            await Task.Delay(Value, token);
            idea.Response(Value, Value % 2 == 0);
        }
    }
}
