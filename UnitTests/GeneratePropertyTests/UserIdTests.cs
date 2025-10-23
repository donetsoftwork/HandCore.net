using Hand.Models;

namespace GeneratePropertyTests;

public class UserIdTests
{
    [Fact]
    public void Test()
    {
        var value = 1L;
        var userId = new UserId(value);
        Assert.Equal(value, userId.Value);
        var userId2 = new UserId(1);
        Assert.Equal(userId, userId2);
        Assert.True(userId == userId2);
    }
    [Fact]
    public void NotEqual()
    {
        var userId = new UserId(1L);
        var userId2 = new UserId(2L);
        Assert.NotEqual(userId, userId2);
        Assert.True(userId != userId2);
        Assert.False(userId == userId2);
    }
}

public partial class UserId : IEntityId;