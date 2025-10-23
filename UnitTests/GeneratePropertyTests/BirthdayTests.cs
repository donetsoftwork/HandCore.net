using Hand.Models;

namespace GeneratePropertyTests;

public class BirthdayTests
{
    [Fact]
    public void Test()
    {
        var value = new DateOnly(1988, 8, 8);
        var birthday = new Birthday(value);
        Assert.Equal(value, birthday.Value);
        var birthday2 = new Birthday(value);
        Assert.Equal(birthday, birthday2);
        Assert.True(birthday == birthday2);
    }
}

public readonly partial struct Birthday : IEntityProperty<DateOnly>;