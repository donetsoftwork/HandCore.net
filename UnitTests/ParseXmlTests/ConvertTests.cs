using Hand.ParseXml;
using ParseXmlTests.Supports;
using System.Xml;

namespace ParseXmlTests;

public class ConvertTests
{
    [Fact]
    public void Convert()
    {
        long expected = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{expected}"">{name}</User>";

        var idReader = HandXml.Default.Attribute<long>("Id")
            .First()
            .Convert(id => new UserId(id));
        UserId result = idReader.Parse(text);
        Assert.Equal(expected, result.Original);
    }
    [Fact]
    public void Convert0()
    {
        long expected = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{expected}"">{name}</User>";

        var idReader = HandXml.Default.Attribute<long>("Id")
            .First()
            .Element("User")
            .First()
            .Convert(id => new UserId(id));
        UserId result = idReader.Parse(text);
        Assert.Equal(expected, result.Original);
    }

    [Fact]
    public void ToBoolean()
    {
        Assert.True(XmlConvert.ToBoolean("true"));
        Assert.False(XmlConvert.ToBoolean("false"));
    }
    [Fact]
    public void ToInt32()
    {
        Assert.Equal(42, XmlConvert.ToInt32("42"));
    }
    [Fact]
    public void ToDouble()
    {
        Assert.Equal(42.0, XmlConvert.ToDouble("42.0"));
    }
    [Fact]
    public void ToDecimal()
    {
        Assert.Equal(42.0m, XmlConvert.ToDecimal("42.0"));
    }
    [Fact]
    public void ToDateTime()
    {
        var dateTime = XmlConvert.ToDateTime("2024-06-01T12:34:56Z", XmlDateTimeSerializationMode.Utc);
        Assert.Equal(new DateTime(2024, 6, 1, 12, 34, 56, DateTimeKind.Utc), dateTime);
    }

}
