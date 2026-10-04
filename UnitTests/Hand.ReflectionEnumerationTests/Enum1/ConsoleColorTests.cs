using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.Enum1;

public class ConsoleColorTests
{
    [Fact]
    public void GetEnumprovider()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
        Enumeration[] items = [.. provider.Items];
        Assert.Equal(16, items.Length);
        ConsoleColor[] values = Enum.GetValues<ConsoleColor>();
        Assert.Equal(16, values.Length);
        string[] names = provider.Names.ToArray();
        Assert.Equal(16, names.Length);
        string[] names0 = Enum.GetNames<ConsoleColor>();
        Assert.Equal(16, names0.Length);
    }
    [Fact]
    public void FromName()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
        // 按枚举名获取
        Assert.True(provider.TryGet(nameof(ConsoleColor.Red), out var red));
        Assert.Equal(nameof(ConsoleColor.Red), red.Name);
        Assert.Empty(red.Description);
        Enumeration? green = provider.Get(nameof(ConsoleColor.Green));
        Assert.NotNull(green);
        Assert.Equal(nameof(ConsoleColor.Green), green.Name);
        var red0 = Enum.Parse<ConsoleColor>(nameof(ConsoleColor.Red));
        Assert.Equal(ConsoleColor.Red, red0);
        Assert.True(Enum.TryParse<ConsoleColor>(nameof(ConsoleColor.Red), out var red2));
        Assert.Equal(ConsoleColor.Red, red2);
    }
    [Fact]
    public void FromOriginal()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
        // 按枚举值获取
        Assert.True(provider.TryGet(10, out var green));
        Assert.Equal(nameof(ConsoleColor.Green), green.Name);
        Assert.Empty(green.Description);
        Enumeration? red = provider.Get((long)ConsoleColor.Red);
        Assert.NotNull(red);
        Assert.Equal(nameof(ConsoleColor.Red), red.Name);
        var green0 = (ConsoleColor)10;
        Assert.Equal(ConsoleColor.Green, green0);
    }
    [Fact]
    public void FromEnum()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
        Assert.True(provider.TryGet((long)ConsoleColor.Blue, out var blue));
        Assert.Equal(nameof(ConsoleColor.Blue), blue.Name);
        Assert.Empty(blue.Description);
    }
    [Fact]
    public void IsDefined()
    {
        IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
        Assert.True(provider.IsDefined(nameof(ConsoleColor.Red)));
        Assert.True(provider.IsDefined((long)ConsoleColor.Green));
        Assert.False(provider.IsDefined("Purple"));
        Assert.False(provider.IsDefined(100L));
        Assert.True(Enum.IsDefined(typeof(ConsoleColor), nameof(ConsoleColor.Red)));
        Assert.True(Enum.IsDefined(typeof(ConsoleColor), (int)ConsoleColor.Green));
    }
}
