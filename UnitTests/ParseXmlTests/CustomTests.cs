using Hand.ParseXml;
using ParseXmlTests.Supports;

namespace ParseXmlTests;

public class CustomTests
{
    [Fact]
    public void UserTest()
    {
        var id = 123;
        var name = "Jxj";
        int age = 20;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User>
	            <Id>{id}</Id>
	            <Name>{name}</Name>
	            <Age>{age}</Age>
            </User>";

        var userParser = new UserParser(HandXml.Default);
        var result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(age, result.Age);
    }
    [Fact]
    public void CommentTest()
    {
        var text = @"
<member name=""M:AddEventBus"">
    <summary>
    注册事件总线
    </summary>
    <typeparam name=""TEventBus"">TEventBus2</typeparam>
    <param name=""services"">services2</param>
    <param name=""lifetime"">lifetime2</param>
    <returns>services</returns>
</member>
";
        var commentParser = new CommentParser(HandXml.Default)
            .Element("member")
            .First();
        var result = commentParser.Parse(text);
        Assert.NotNull(result);
    }
}
