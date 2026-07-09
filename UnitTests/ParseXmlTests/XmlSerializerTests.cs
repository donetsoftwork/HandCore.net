using ParseXmlTests.Supports;
using System.Xml.Serialization;

namespace ParseXmlTests;

public class XmlSerializerTests
{
    private static readonly XmlSerializer _serializer = new(typeof(Person));

    [Fact]
    public void Deserialize()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root PersonId=""1"">
    	        <PersonName>张三</PersonName>
            </Root>";
        using var stringReader = new StringReader(text);
        Person? person = _serializer.Deserialize(stringReader) as Person;
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
        Assert.Equal("张三", person.Name);
    }
    [Fact]
    public void Throw()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Person>
                <Id>1</Id>
    	        <Name>张三</Name>
            </Person>";
        using var stringReader = new StringReader(text);
        Assert.ThrowsAny<InvalidOperationException>(() => _serializer.Deserialize(stringReader));
    }
}
