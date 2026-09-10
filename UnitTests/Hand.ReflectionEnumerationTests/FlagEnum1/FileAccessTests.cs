using Hand.Enumerations;

namespace Hand.ReflectionEnumerationTests.FlagEnum1;

public class FileAccessTests
{
    [Fact]
    public void GetEnumprovider()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        Assert.Equal(3, provider.Items.Length);
        Assert.Equal(2, provider.Flags.Length);
        FileAccess[] values = Enum.GetValues<FileAccess>();
        Assert.Equal(3, values.Length);
        string[] names = provider.Names.ToArray();
        Assert.Equal(3, names.Length);
        string[] names0 = Enum.GetNames<FileAccess>();
        Assert.Equal(3, names0.Length);
    }
    [Fact]
    public void FromName()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        // 按枚举名获取
        FlagEnumeration? read = provider.Get(nameof(FileAccess.Read));
        Assert.NotNull(read);
        Assert.Equal(nameof(FileAccess.Read), read.Name);
        Assert.Empty(read.Description);
        var red0 = Enum.Parse<FileAccess>(nameof(FileAccess.Read));
        Assert.Equal(FileAccess.Read, red0);
    }
    [Fact]
    public void FromOriginal()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        // 按枚举值获取
        FlagEnumeration? write = provider.Get(2);
        Assert.NotNull(write);
        Assert.Equal(nameof(FileAccess.Write), write.Name);
        Assert.Empty(write.Description);
        var write0 = (FileAccess)2;
        Assert.Equal(FileAccess.Write, write0);
    }
    [Fact]
    public void FromEnum()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        FlagEnumeration? fileAccess = provider.Get((long)FileAccess.Read);
        Assert.NotNull(fileAccess);
        Assert.Equal(nameof(FileAccess.Read), fileAccess.Name);
        Assert.Empty(fileAccess.Description);
    }
    [Fact]
    public void HasFlag()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        FlagEnumeration? read = provider.Get(nameof(FileAccess.Read));
        Assert.NotNull(read);
        var readWrite = provider.Get(nameof(FileAccess.ReadWrite));
        Assert.NotNull(readWrite);
        Assert.True(readWrite.HasFlag(read));
        Assert.True(readWrite.HasFlag((long)FileAccess.Write));
        Assert.True(readWrite.HasFlag(nameof(FileAccess.Write)));
        Assert.True(FileAccess.ReadWrite.HasFlag(FileAccess.Read));
    }
    [Fact]
    public void IsDefined()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        Assert.True(provider.IsDefined(nameof(FileAccess.Read)));
        Assert.True(provider.IsDefined((long)FileAccess.Write));
        Assert.False(provider.IsDefined("Append"));
    }
    [Fact]
    public void Empty()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        FlagEnumeration empty = provider.Empty;
        Assert.Equal(0L, empty.Original);
    }
    [Fact]
    public void TryParseByName()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>(StringComparer.OrdinalIgnoreCase);
        Assert.True(provider.TryParse("read,write", out var result));
        FlagEnumeration? readWrite = provider.Get(nameof(FileAccess.ReadWrite));
        Assert.NotNull(readWrite);
        Assert.Equal(readWrite, result);
        Assert.True(Enum.TryParse<FileAccess>("read,write", true, out var result0));
        Assert.Equal(FileAccess.ReadWrite, result0);
    }
    [Fact]
    public void TryParseByOriginal()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        Assert.True(provider.TryParse((long)FileAccess.Read, out var read));
        Assert.Equal(nameof(FileAccess.Read), read.Name);
        Assert.True(provider.TryParse((long)(FileAccess.Read | FileAccess.Write), out var readWrite));
        Assert.True(readWrite.HasFlag((long)FileAccess.Read));
    }
    [Fact]
    public void And()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        var read = provider.Get(nameof(FileAccess.Read));
        Assert.NotNull(read);
        var write = provider.Get(nameof(FileAccess.Write));
        Assert.NotNull(write);
        var readWrite = provider.Get(nameof(FileAccess.ReadWrite));
        Assert.NotNull(readWrite);
        var result = provider.And(read, readWrite);
        Assert.Equal(read, result);
        var access = provider.And(read, write, readWrite);
        Assert.Equal(0L, access.Original);
        var result0 = FileAccess.ReadWrite & FileAccess.Read;
        Assert.Equal(FileAccess.Read, result0);
    }
    [Fact]
    public void Or()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
        var read = provider.Get(nameof(FileAccess.Read));
        Assert.NotNull(read);
        var write = provider.Get(nameof(FileAccess.Write));
        Assert.NotNull(write);
        var readWrite = provider.Get(nameof(FileAccess.ReadWrite));
        Assert.NotNull(readWrite);
        var result = provider.Or(read, write);
        Assert.Equal(readWrite, result);
        var access = provider.Or(read, write, readWrite);
        Assert.Equal(readWrite, access);
        var access0 = FileAccess.Read | FileAccess.Write;
        Assert.Equal(FileAccess.ReadWrite, access0);
    }
    [Fact]
    public void IgnoreCase()
    {
        IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>(StringComparer.OrdinalIgnoreCase);
        // 按枚举名获取
        Enumeration? fileAccess = provider.Get("read");
        Assert.NotNull(fileAccess);
        Assert.True(provider.IsDefined("readwrite"));
        Assert.True(provider.TryParse("read,write", out var result));
        var readWrite = provider.Get(nameof(FileAccess.ReadWrite));
        Assert.NotNull(readWrite);
        Assert.Equal(readWrite, result);
    }
}
