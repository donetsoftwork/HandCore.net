using Hand.Reflection;

namespace ReflectionTests;

public class PairTypeKeyTests
{
    [Fact]
    public void CheckValueType()
    {
        Assert.True(PairTypeKey.CheckValueType(typeof(string), typeof(object)));
        Assert.False(PairTypeKey.CheckValueType(typeof(object), typeof(string)));
    }
}
