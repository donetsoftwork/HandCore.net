# 反射枚举组件
>* 使用枚举类而不是枚举类型

## 1、官方建议使用枚举类而不是枚举类型
>* 原生枚举缺乏行为封装
>* 类型安全性不足（可强制转换非法值）
>* 违反开闭原则（需大量 switch-case）的问题

## 2. 反射系统enum类型 ConsoleColor
## 2.1 获取枚举项
>* 使用方法ReflectionEnumeration.GetEnumProvider反射enum
>* IEnumerationProvider对象存储了枚举信息避免重复反射
>* 可以把IEnumerationProvider对象注入IOC容器方便随时使用
>* 通过Items属性获取所有枚举项,相当于Enum.GetValues方法
>* 通过Names属性获取所有枚举名,相当于Enum.GetNames方法
>* 16个枚举字段转化为16个枚举类(Enumeration)对象
>* 通过Get方法获取单个枚举项
>* Get方法可以通过枚举名获取,相当于Enum.Parse方法
>* Get方法也可以通过枚举值获取,相当于强转为enum
>* 枚举接口是IEnumeration
>* 枚举类是Enumeration

~~~csharp
IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
Enumeration[] items = provider.Items;
Assert.Equal(16, items.Length);
ConsoleColor[] values = Enum.GetValues<ConsoleColor>();
Assert.Equal(16, values.Length);
string[] names = provider.Names.ToArray();
Assert.Equal(16, names.Length);
string[] names0 = Enum.GetNames<ConsoleColor>();
Assert.Equal(16, names0.Length);
// 按枚举名获取
Enumeration? red = provider.Get(nameof(ConsoleColor.Red));
Assert.NotNull(red);
Assert.Equal(nameof(ConsoleColor.Red), red.Name);
var red0 = Enum.Parse<ConsoleColor>(nameof(ConsoleColor.Red));
Assert.Equal(ConsoleColor.Red, red0);
// 按枚举值获取
Enumeration? green = provider.Get(10);
Assert.NotNull(green);
Assert.Equal(nameof(ConsoleColor.Green), green.Name);
var green0 = (ConsoleColor)10;
Assert.Equal(ConsoleColor.Green, green0);
Enumeration? blue = provider.Get((long)ConsoleColor.Blue);
Assert.NotNull(blue);
Assert.Equal(nameof(ConsoleColor.Blue), blue.Name);
~~~

## 2.2 校验枚举项
>* 通过IsDefined校验枚举是否定义
>* 可以通过枚举名校验也可以通过枚举值校验
>* 相当于Enum.IsDefined方法

~~~csharp
IEnumerationProvider<Enumeration> provider = ReflectionEnumeration.GetEnumProvider<ConsoleColor>();
Assert.True(provider.IsDefined(nameof(ConsoleColor.Red)));
Assert.True(provider.IsDefined((long)ConsoleColor.Green));
Assert.False(provider.IsDefined("Purple"));
Assert.False(provider.IsDefined(100L));
Assert.True(Enum.IsDefined(typeof(ConsoleColor), nameof(ConsoleColor.Red)));
Assert.True(Enum.IsDefined(typeof(ConsoleColor), (int)ConsoleColor.Green));
~~~

## 3. 反射位标记枚举类型 FileAccess
## 3.1 获取位标记枚举项
>* 使用方法ReflectionEnumeration.GetFlagEnumProvider反射位标记enum
>* 这里说的位标记枚举类型就是添加了Flags标记的enum
>* IFlagEnumerationProvider对象存储了位标记枚举信息避免重复反射
>* 可以把IFlagEnumerationProvider对象注入IOC容器方便随时使用
>* IFlagEnumerationProvider包含IEnumerationProvider的所有功能,并支持位运算
>* IFlagEnumerationProvider派生自枚举接口IEnumerationProvider
>* 位标记枚举接口是IFlagEnumeration,派生自枚举接口IEnumeration
>* 位标记枚举类是FlagEnumeration,派生自枚举类Enumeration
>* IFlagEnumeration比IEnumeration多个Flags属性
>* Flags为单位标记,0和复合位标记除外
>* 本Case中ReadWrite就是复合位,等效于 Read | Write,所以Flags比Items少

~~~csharp
IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
Assert.Equal(3, provider.Items.Length);
Assert.Equal(2, provider.Flags.Length);
FileAccess[] values = Enum.GetValues<FileAccess>();
Assert.Equal(3, values.Length);
string[] names = provider.Names.ToArray();
Assert.Equal(3, names.Length);
string[] names0 = Enum.GetNames<FileAccess>();
Assert.Equal(3, names0.Length);
// 按枚举名获取
FlagEnumeration? read = provider.Get(nameof(FileAccess.Read));
Assert.NotNull(read);
Assert.Equal(nameof(FileAccess.Read), read.Name);
Assert.Empty(read.Description);
var red0 = Enum.Parse<FileAccess>(nameof(FileAccess.Read));
Assert.Equal(FileAccess.Read, red0);
// 按枚举值获取
FlagEnumeration? write = provider.Get(2);
Assert.NotNull(write);
Assert.Equal(nameof(FileAccess.Write), write.Name);
Assert.Empty(write.Description);
var write0 = (FileAccess)2;
Assert.Equal(FileAccess.Write, write0);
~~~

## 3.2 Empty属性
>* 由于位标记枚举支持位运算,所以很容易出现枚举值为0的情况
>* 所以定义标记enum尽量定义一个0值字段
>* 0值字段就会转化为Empty属性
>* 如果没有定义0值字段也会生成一个0值枚举类对象
>* 因为0值枚举会参与到位运算逻辑中

~~~csharp
IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
var empty = provider.Empty;
Assert.Equal(0L, empty.Original);
~~~

## 3.3 与运算
>* And方法用于位标记枚举的与运算,用于平替enum的与(&)运算
>* And支持传多个同类型的位标记枚举

~~~csharp
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
~~~

## 3.4 或运算
>* Or方法用于位标记枚举的与运算,用于平替enum的或(|)运算
>* Or支持传多个同类型的位标记枚举

~~~csharp
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
Assert.True(access.HasFlag(read));
Assert.True(access.HasFlag((long)FileAccess.Write));
var result0 = FileAccess.Read | FileAccess.Write;
Assert.Equal(FileAccess.ReadWrite, result0);
~~~

## 3.5 HasFlag方法
>* IFlagEnumeration的HasFlag方法用于平替方法enum的HasFlag方法
>* HasFlag支持传枚举、枚举名和枚举值
>* enum的HasFlag只支持枚举

~~~csharp
IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>();
FlagEnumeration? read = provider.Get(nameof(FileAccess.Read));
Assert.NotNull(read);
var readWrite = provider.Get(nameof(FileAccess.ReadWrite));
Assert.NotNull(readWrite);
Assert.True(readWrite.HasFlag(read));
Assert.True(readWrite.HasFlag(nameof(FileAccess.Write)));
Assert.True(readWrite.HasFlag((long)FileAccess.Write));
Assert.True(FileAccess.ReadWrite.HasFlag(FileAccess.Read));
~~~

## 3.6 TryParse方法
>* TryParse支持多个枚举名(逗号分割)和枚举值
>* TryParse方法用于平替Enum.TryParse方法
>* TryParse与Get有区别
>* Get只能获取定义的枚举项
>* TryParse可以获取位运算后的枚举项(能按位拆解)

~~~csharp
IFlagEnumerationProvider<FlagEnumeration> provider = ReflectionEnumeration.GetFlagEnumProvider<FileAccess>(StringComparer.OrdinalIgnoreCase);
Assert.True(provider.TryParse("read,write", out var result));
FlagEnumeration? readWrite = provider.Get(nameof(FileAccess.ReadWrite));
Assert.NotNull(readWrite);
Assert.Equal(readWrite, result);
Assert.True(provider.TryParse((long)FileAccess.Read, out var read));
Assert.Equal(nameof(FileAccess.Read), read.Name);
Assert.True(Enum.TryParse<FileAccess>("read,write", true, out var result0));
Assert.Equal(FileAccess.ReadWrite, result0);
~~~

## 4. 枚举别名和备注
## 4.1 定义含别名和备注的enum
~~~csharp
/// <summary>
/// Vip卡类型
/// </summary>
public enum CardType
{
    [Description("银卡")]
    Silver = 1,
    Vip = Silver,
    [Description("金卡")]
    Gold = 2,
    Vip2 = Gold,
    [Description("白金卡")]
    Platinum = 3,
    Vip3 = Platinum,
    [Description("黑金卡")]
    [EnumMember(Value = "SVIP")]
    Black = 4,
    Vip4 = Black,
}
~~~

## 4.2 别名和备注使用示例如下
>* CardType定义了8个字段,解析为4个枚举项
>* 其中Silver、Gold、Platinum和Black为四个枚举项
>* Description标记解析为枚举项的备注(Description属性),作为扩展属性,一般用于展示
>* 可以通过GetByDescription方法按Description获取枚举项
>* Vip、Vip2、Vip3和Vip4等四个解析为别名
>* Black定义的EnumMember标记也解析为别名
>* 另外GetEnumerationProvider有可选参数comparer,传StringComparer.OrdinalIgnoreCase可以实现忽略大小写
>* comparer对Get、IsDefined和TryParse有效
>* comparer参数对前面的GetEnumProvider方法同样有效

~~~csharp
IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>(StringComparer.OrdinalIgnoreCase);
Enumeration[] cardTypes = provider.Items;
Assert.Equal(4, cardTypes.Length);
// 按枚举名获取
Enumeration? silver = provider.Get(nameof(CardType.Silver));
Assert.NotNull(silver);
Assert.Equal(nameof(CardType.Silver), silver.Name);
Assert.Equal("银卡", silver.Description);
// 按枚举别名获取
Enumeration? vip = provider.Get("vip");
Assert.NotNull(vip);
Assert.Equal(silver, vip);
// 按枚举值获取
Enumeration? gold = provider.Get(2);
Assert.NotNull(gold);
Assert.Equal(nameof(CardType.Gold), gold.Name);
Assert.Equal("金卡", gold.Description);
// 用EnumMember标记定义别名
Enumeration? svip = provider.Get("svip");
Assert.NotNull(svip);
// 支持用Description标记定义备注
Enumeration? description = provider.GetByDescription("银卡");
Assert.NotNull(description);
Assert.True(provider.IsDefined(nameof(CardType.Gold)));
Assert.True(provider.IsDefined(nameof(CardType.Vip)));
Assert.True(provider.IsDefined("svip");
var other = new Enumeration("VVip", 100L, "超级会员");
Assert.False(provider.IsDefined(other));
~~~

## 5. 反射普通枚举类
### 5.1 自定义简单的枚举类CardType
>* CardType除了构造函数,复杂度和enum差不多了

~~~csharp
/// <summary>
/// Vip卡类型
/// </summary>
public sealed class CardType : Enumeration
{
    private CardType(string name, long original, string description = "")
        : base(name, original, description)
    {
    }
    /// <summary>
    /// 银卡
    /// </summary>
    public static readonly CardType Silver = new(nameof(Silver), 1, "银卡");
    /// <summary>
    /// 金卡
    /// </summary>
    public static readonly CardType Gold = new(nameof(Gold), 2, "金卡");
    /// <summary>
    /// 铂金卡
    /// </summary>
    public static readonly CardType Platinum = new(nameof(Platinum), 3, "铂金卡");
    /// <summary>
    /// 黑金卡
    /// </summary>
    public static readonly CardType Black = new(nameof(Black), 4, "黑金卡");
    /// <inheritdoc cref="Silver" path="/summary"/>
    public static CardType Vip => Silver;
    /// <inheritdoc cref="Gold" path="/summary"/>
    public static CardType Vip2 => Gold;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    public static CardType Vip3 => Platinum;
    /// <inheritdoc cref="Black" path="/summary"/>
    public static CardType Vip4 => Black;
}
~~~

### 5.2 反射枚举类CardType
>* 使用ReflectionEnumeration.GetEnumerationProvider反射枚举类

~~~csharp
IEnumerationProvider<CardType> provider = ReflectionEnumeration.GetEnumerationProvider<CardType>();
CardType[] cardTypes = provider.Items;
Assert.Equal(4, cardTypes.Length);
// 按枚举名获取
var silver = provider.Get("Silver");
Assert.NotNull(silver);
Assert.Equal("银卡", silver.Description);
// 按别名获取
var vip = provider.Get("Vip");
Assert.NotNull(vip);
Assert.Equal(CardType.Silver, silver);
Assert.Equal(silver, vip);
// 获取不存在的枚举时指定默认值，返回默认值
var unknownDefault = provider.Get("xxx", CardType.Silver);
Assert.Equal(CardType.Silver, unknownDefault);
Assert.True(provider.IsDefined(nameof(CardType.Gold)));
Assert.True(provider.IsDefined(CardType.Silver.Original));
Assert.True(provider.IsDefined(CardType.Silver));
Assert.False(provider.IsDefined("SVip"));
~~~

## 6. 反射位标记枚举类
### 6.1 自定义位标记枚举类DaysOfWeek
>* DaysOfWeek除了构造函数,复杂度和enum差不多了


~~~csharp
public class DaysOfWeek
    : FlagEnumeration
{
    /// <summary>
    /// 这个构造函数用于反射,误删会报错
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    private DaysOfWeek(ISet<string> flags, long original, string description = "")
        : base(flags, original, description)
    {
    }
    private DaysOfWeek(IEqualityComparer<string> comparer, string name, long original, string description = "")
         : base(comparer, name, original, description)
    {
    }
    private static readonly IEqualityComparer<string> _comparer = StringComparer.OrdinalIgnoreCase;
    #region Days
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeek Sunday = new(_comparer, "Sunday", 1, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeek Monday = new(_comparer, "Monday", 1 << 1, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeek Tuesday = new(_comparer, "Tuesday", 1 << 2, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeek Wednesday = new(_comparer, "Wednesday", 1 << 3, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeek Thursday = new(_comparer, "Thursday", 1 << 4, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeek Friday = new(_comparer, "Friday", 1 << 5, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeek Saturday = new(_comparer, "Saturday", 1 << 6, "星期六");
    #endregion
}
~~~

### 6.2 反射位标记枚举类DaysOfWeek
>* 使用ReflectionEnumeration.GetEnumerationProvider反射枚举类

~~~csharp
IFlagEnumerationProvider<DaysOfWeek> provider = ReflectionEnumeration.GetFlagEnumerationProvider<DaysOfWeek>();
DaysOfWeek[] cardTypes = provider.Items;
Assert.Equal(7, cardTypes.Length);
Assert.True(provider.IsDefined(DaysOfWeek.Saturday));
Assert.True(provider.IsDefined(DaysOfWeek.Saturday.Original));
Assert.False(provider.IsDefined("DayEight"));
var other = new DaysOfWeek(StringComparer.Ordinal, "DayEight", 1 << 7, "星期八");
Assert.False(provider.IsDefined(other));
var weekend = provider.Or(DaysOfWeek.Saturday, DaysOfWeek.Sunday);
Assert.True(weekend.HasFlag(DaysOfWeek.Saturday));
Assert.False(weekend.HasFlag(DaysOfWeek.Monday));
// 假设员工一周一和周二值班
var duty1 = provider.Or(DaysOfWeek.Monday, DaysOfWeek.Tuesday);
// 假设员工二周二和周三值班
var duty2 = provider.Or(DaysOfWeek.Tuesday, DaysOfWeek.Wednesday);
/// 获取两员工共同值班的时间
var overlay = provider.And(duty1, duty2);
Assert.Equal(DaysOfWeek.Tuesday, overlay);
~~~
