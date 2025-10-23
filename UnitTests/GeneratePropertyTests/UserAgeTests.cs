using Hand.Models;

namespace GeneratePropertyTests;

public class UserAgeTests
{
    [Fact]
    public void HasValue()
    {
        var value = 30;
        var userAge = new UserAge(value);
        Assert.Equal(value, userAge.Value);
        var userAge2 = new UserAge(30);
        Assert.Equal(userAge, userAge2);
        Assert.True(userAge == userAge2);
    }
    [Fact]
    public void NoValue()
    {
        var userAge = new UserAge(null);
        Assert.Null(userAge.Value);
        var userAge2 = new UserAge(null);
        Assert.Equal(userAge, userAge2);
        Assert.True(userAge == userAge2);
    }
    [Fact]
    public void NotEqual()
    {
        var userAge = new UserAge(30);
        var userAge2 = new UserAge(null);
        Assert.NotEqual(userAge, userAge2);
        Assert.True(userAge != userAge2);
        var userAge3 = new UserAge(20);
        Assert.NotEqual(userAge, userAge3);
        Assert.True(userAge != userAge3);
        Assert.False(userAge == userAge3);
    }
}

public partial class UserAge : IEntityProperty<int?>;
