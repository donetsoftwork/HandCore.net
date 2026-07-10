using BenchmarkDotNet.Attributes;
using Hand.ParseXml;
using ParseXmlBench.Supports;
using System.Xml.Serialization;

namespace ParseXmlBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 1000000)]
public class UserSingleBench
{
    private static readonly XmlSerializer _serializer = new(typeof(User));
    //private static readonly IMemberBuilderProvider _builderProvider = MemberBuilderProvider.Instance/*.UseCreator(new UserCreater())*/;
    //private static readonly HandXml _xmlConfig = new(_builderProvider, DefaultValueBuilder.Instance);

    private static readonly EntityParser<User> _parser = HandXml.Default.Entity<User>()
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser1 = HandXml.Default.Entity<User, UserBuilder>()
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser2 = HandXml.Default.Entity(UserBuilder.Creater)
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser3 = HandXml.Default.Entity(UserBuilder2.Creater)
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age));
    private static readonly UserParser _customParser = new(HandXml.Default);
    private static readonly string text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
    	<Id>1</Id>
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>";

    [Benchmark(Baseline = true)]
    public User? Deserialize()
    {
        using var stringReader = new StringReader(text);
        return (User?)_serializer.Deserialize(stringReader);
    }
    [Benchmark]
    public User GetResult()
    {
        return _parser.Get(text);
    }
    [Benchmark]
    public User GetResult1()
    {
        return _parser1.Get(text);
    }
    [Benchmark]
    public User GetResult2()
    {
        return _parser2.Get(text);
    }
    [Benchmark]
    public User GetResult3()
    {
        return _parser3.Get(text);
    }
    [Benchmark]
    public User Custom()
    {
        return _customParser.Get(text);
    }
}
