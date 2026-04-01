using Hand.Reflection;

namespace ReflectionTests;

public class ReflectionTypeTests
{
    [Fact]
    public void ClassIsGenericType()
    {
        Assert.True(ReflectionType.IsGenericType(typeof(List<int>), typeof(List<>)));
        Assert.False(ReflectionType.IsGenericType(typeof(List<int>), typeof(IEnumerable<>)));
    }
    [Fact]
    public void InterfaceIsGenericType()
    {
        Assert.True(ReflectionType.IsGenericType(typeof(IList<string>), typeof(IList<>)));
        Assert.False(ReflectionType.IsGenericType(typeof(IList<string>), typeof(IEnumerable<>)));
    }
    [Fact]
    public void ListHasGenericType()
    {
        Assert.True(ReflectionType.HasGenericType(typeof(List<int>), typeof(List<>)));
        Assert.True(ReflectionType.HasGenericType(typeof(List<int>), typeof(IList<>)));
        Assert.True(ReflectionType.HasGenericType(typeof(List<int>), typeof(ICollection<>)));
        Assert.True(ReflectionType.HasGenericType(typeof(List<int>), typeof(IEnumerable<>)));
    }
    [Fact]
    public void StringHasGenericType()
    {
        Assert.True(ReflectionType.HasGenericType(typeof(string), typeof(IEnumerable<>)));
    }
    [Fact]
    public void ListGetGenericCloseInterfaces()
    {
        var interfaces = ReflectionType.GetGenericCloseInterfaces(typeof(List<int>), typeof(IEnumerable<>))
            .ToArray();
        Assert.Single(interfaces);
        var @interface = interfaces[0];
        Assert.NotNull(@interface);
        Assert.True(@interface.IsGenericType);
        var elementType = @interface.GetGenericArguments()[0];
        Assert.Equal(typeof(int), elementType);
    }
    [Fact]
    public void StringGetGenericCloseInterfaces()
    {
        var interfaces = ReflectionType.GetGenericCloseInterfaces(typeof(string), typeof(IEnumerable<>))
            .ToArray();
        Assert.Single(interfaces);
        var @interface = interfaces[0];
        Assert.NotNull(@interface);
        Assert.True(@interface.IsGenericType);
        var elementType = @interface.GetGenericArguments()[0];
        Assert.Equal(typeof(char), elementType);
    }
}
