using BenchmarkDotNet.Attributes;
using Hand.ParseJson;
using ParseJsonBench.Supports;
using System.Text.Json;

namespace ParseJsonBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 3000000)]
public class UserListBench
{
    private static readonly RepeatReader<User> _parser = HandJson.Default.Entity<User>()
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _parser1 = HandJson.Default.Entity<User, UserBuilder>()
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _parser2 = HandJson.Default.Entity(UserBuilder.Creater)
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _parser3 = HandJson.Default.Entity(UserBuilder2.Creater)
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _customParser = new UserParser(HandJson.Default)
        .Repeat();
    private static readonly string _text = @"[
{ ""Id"": 1, ""Name"": ""张三"",  ""Age"": 9},
{ ""Id"": 2, ""Name"": ""李四"",  ""Age"": 10},
{ ""Id"": 3, ""Name"": ""王二"",  ""Age"": 11}
]";
    [Benchmark(Baseline = true)]
    public List<User>? Deserialize()
    {
        return JsonSerializer.Deserialize<List<User>>(_text);
    }
    [Benchmark]
    public List<User>? Generated()
    {
        return JsonSerializer.Deserialize(_text, UserContext.Default.ListUser);
    }
    [Benchmark]
    public List<User> GetResult()
    {
        return _parser.Get(_text);
    }
    [Benchmark]
    public List<User> GetResult1()
    {
        return _parser1.Get(_text);
    }
    [Benchmark]
    public List<User> GetResult2()
    {
        return _parser2.Get(_text);
    }
    [Benchmark]
    public List<User> GetResult3()
    {
        return _parser3.Get(_text);
    }
    [Benchmark]
    public List<User> Custom()
    {
        return _customParser.Get(_text);
    }
}
