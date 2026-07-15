using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class JsonReaderTests
{
    [Fact]
    public void Text()
    {
        string json = "{\"root\":{\"name\":\"111\", \"value\": 222, \"active\": true, \"description\": null }}";
        //JsonSerializer.Deserialize<object>(json);
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        //Utf8Parser.TryParse(Encoding.UTF8.GetBytes("2023-01-01T00:00:00Z"), out DateTimeOffset dateTimeOffset, out int bytesConsumed);
        //Utf8Formatter.TryFormat(DateTimeOffset.Now, Encoding.UTF8.GetBytes(""), out int bytesWritten);
        while (reader.Read())
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartObject:
                    Console.WriteLine("Start Object");
                    break;
                case JsonTokenType.EndObject:
                    Console.WriteLine("End Object");
                    break;
                case JsonTokenType.PropertyName:
                    var propertyName = reader.GetString();
                    Console.WriteLine("Property Name:" + propertyName);
                    break;
                case JsonTokenType.String:
                    var value = reader.GetString();
                    Console.WriteLine("String Value:" + value);
                    Console.WriteLine("Has Value Sequence:" + reader.HasValueSequence);
                    //var intValue = reader.GetInt32();
                    //Console.WriteLine("Int Value:" + intValue);
                    //var timeValue = reader.GetDateTimeOffset();
                    //Console.WriteLine("DateTime Value:" + timeValue);
                    break;
                case JsonTokenType.Number:
                    var number = reader.GetDouble();
                    //var span = reader.ValueSpan;
                    Console.WriteLine("Number Value:" + number);
                    break;
                case JsonTokenType.True:
                    Console.WriteLine("True Value");
                    break;
                case JsonTokenType.False:
                    Console.WriteLine("False Value");
                    break;
                case JsonTokenType.Comment:
                    Console.WriteLine("Comment Value" + reader.GetComment());
                    break;
                case JsonTokenType.Null:
                    Console.WriteLine("Null Value");
                    break;
                default:
                    Console.WriteLine("Default Value");
                    break;
            }
        }
    }
    [Fact]
    public void Deserialize()
    {
        string json = "{\"id\":111, \"value\": 3.5, \"active\": true}";
        if (JsonSerializer.Deserialize<object>(json) is not JsonElement element)
        {
            Assert.Fail();
            return;
        }            
        //foreach (var item in element.EnumerateObject())
        //{
        //    var name = item.Name;
        //    var value = item.Value;
        //}
    }
}
