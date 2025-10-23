using Hand.Models;

namespace GeneratePropertyTests;

public class UserNameTests
{
    [Fact]
    public void Test()
    {
        var value = "Jxj";
        var userName = new UserName(value);        
        Assert.Equal(value, userName.Value);
        var userName2 = new UserName("Jxj");
        Assert.Equal(userName, userName2);
        Assert.True(userName == userName2);
    }
    [Fact]
    public void NotEqual()
    {
        var userName = new UserName("Jxj");
        var userName2 = new UserName("Jxj2");
        Assert.NotEqual(userName, userName2);
        Assert.True(userName != userName2);
        Assert.False(userName == userName2);
    }
}

public partial record UserName : IEntityProperty<string>;