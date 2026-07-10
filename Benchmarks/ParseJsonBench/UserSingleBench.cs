using BenchmarkDotNet.Attributes;
using Hand.ParseJson;
using ParseJsonBench.Supports;
using System.Text.Json;

namespace ParseJsonBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 5000000)]
public class UserSingleBench
{
    private static readonly EntityParser<User> _parser = HandJson.Default.Entity<User>()
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser1 = HandJson.Default.Entity<User, UserBuilder>()
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser2 = HandJson.Default.Entity(UserBuilder.Creater)
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser3 = HandJson.Default.Entity(UserBuilder2.Creater)
        .WithProperty<int>(nameof(User.Id))
        .WithProperty<string>(nameof(User.Name))
        .WithProperty<int>(nameof(User.Age));
    private static readonly UserParser _customParser = new(HandJson.Default);
    private static readonly string _text = @"{ ""Id"": 1, ""Name"": ""张三"",  ""Age"": 9}";
    //private static readonly byte[] _bytes = Encoding.UTF8.GetBytes(_text);

    [Benchmark(Baseline = true)]
    public User? Deserialize()
    {
        return JsonSerializer.Deserialize<User>(_text);
    }
    [Benchmark]
    public User? Generated()
    {
        return JsonSerializer.Deserialize(_text, UserContext.Default.User);
    }
    [Benchmark]
    public User GetResult()
    {
        return _parser.Parse(_text);
    }
    [Benchmark]
    public User GetResult1()
    {
        return _parser1.Parse(_text);
    }
    [Benchmark]
    public User GetResult2()
    {
        return _parser2.Parse(_text);
    }
    [Benchmark]
    public User GetResult3()
    {
        return _parser3.Parse(_text);
    }
    [Benchmark]
    public User Custom()
    {
        return _customParser.Parse(_text);
    }
}
