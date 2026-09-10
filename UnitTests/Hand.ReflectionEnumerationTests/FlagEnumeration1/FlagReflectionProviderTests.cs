using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.FlagEnumeration1;

public class FlagReflectionProviderTests
{
    [Fact]
    public void GetConstructor()
    {
        var constructor = FlagReflectionProvider<DaysOfWeek>.GetConstructor();
        Assert.NotNull(constructor);
    }
}
