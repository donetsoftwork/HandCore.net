using BenchmarkDotNet.Attributes;
using Hand.ParseXml;
using ParseXmlBench.Supports;
using System.Xml;
using System.Xml.Serialization;

namespace ParseXmlBench;

[MemoryDiagnoser, SimpleJob(launchCount: 2, warmupCount: 10, iterationCount: 10, invocationCount: 1000000)]
public class UserListBench
{
    private static readonly XmlSerializer _serializer = new(typeof(User[]));
    private static readonly RepeatReader<User> _parser = HandXml.Default.Entity<User>()
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _parser1 = HandXml.Default.Entity<User, UserBuilder>()
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _parser2 = HandXml.Default.Entity(UserBuilder.Creater)
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _parser3 = HandXml.Default.Entity(UserBuilder2.Creater)
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age))
        .Repeat();
    private static readonly RepeatReader<User> _customParser = new UserParser(HandXml.Default)
        .Repeat();
    private static readonly string text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <ArrayOfUser>
    <User>
    	<Id>1</Id>
    	<Name>张三</Name>
    	<Age>10</Age>
    </User>
    <User>
    	<Id>2</Id>
    	<Name>李四</Name>
    	<Age>11</Age>
    </User>
    <User>
    	<Id>3</Id>
    	<Name>王二</Name>
    	<Age>9</Age>
    </User>
    </ArrayOfUser>";

    [Benchmark(Baseline = true)]
    public User[]? Deserialize()
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return (User[]?)_serializer.Deserialize(xmlReader);
    }
    [Benchmark]
    public User[] GetResult()
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return [.. _parser.Get(xmlReader)];
    }
    [Benchmark]
    public User[] GetResult1()
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return [.. _parser1.Get(xmlReader)];
    }
    [Benchmark]
    public User[] GetResult2()
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return [.. _parser2.Get(xmlReader)];
    }
    [Benchmark]
    public User[] GetResult3()
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return [.. _parser3.Get(xmlReader)];
    }
    [Benchmark]
    public User[] Custom()
    {
        using var stringReader = new StringReader(text);
        using var xmlReader = XmlReader.Create(stringReader);
        return [.. _customParser.Get(xmlReader)];
    }
}
