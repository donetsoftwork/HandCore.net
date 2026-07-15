# 开源完美模块组件化可扩展的Xml解析器Hand.ParseXml

## 一、首先对比Xml反序列化
### 1. 性能对比
#### 1.1 性能测试结果如下
>* Deserialize是反序列化结果
>* GetResult是基于反射的对象构建方法,比反序列化快33%
>* GetResult2是定制对象构建组件,避免了反射,比GetResult快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult2更快一点

| Method      | Mean       | Error   | StdDev  | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |-----------:|--------:|--------:|------:|-------:|-------:|----------:|------------:|
| Deserialize | 1,355.5 ns | 4.44 ns | 4.75 ns |  1.00 | 0.7240 | 0.0300 |   12.2 KB |        1.00 |
| GetResult   |   912.7 ns | 5.07 ns | 5.84 ns |  0.67 | 0.6750 | 0.0250 |  11.38 KB |        0.93 |
| GetResult2  |   798.3 ns | 0.40 ns | 0.42 ns |  0.59 | 0.6500 |      - |  10.95 KB |        0.90 |
| Custom      |   779.4 ns | 6.46 ns | 7.43 ns |  0.57 | 0.6500 |      - |  10.95 KB |        0.90 

### 1.2 相关代码如下
~~~csharp
    private static readonly XmlSerializer _serializer = new(typeof(User));
    private static readonly EntityParser<User> _parser = HandXml.Default.Entity<User>()
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age));
    private static readonly EntityParser<User> _parser2 = HandXml.Default.Entity(UserBuilder.Creater)
        .WithItem<int>(nameof(User.Id))
        .WithItem(nameof(User.Name))
        .WithItem<int>(nameof(User.Age));
    private static readonly UserParser _customParser = new(HandXml.Default);

    [Benchmark(Baseline = true)]
    public User? Deserialize()
    {
        using var stringReader = new StringReader(text);
        return (User?)_serializer.Deserialize(stringReader);
    }
    [Benchmark]
    public User GetResult()
    {
        return _parser.Get(text);
    }
    [Benchmark]
    public User GetResult2()
    {
        return _parser2.Get(text);
    }
    [Benchmark]
    public User Custom()
    {
        return _customParser.Get(text);
    }
~~~

~~~csharp
[Serializable]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
/// <summary>
/// 建造者模式
/// </summary>
public class UserBuilder2
    : IMemberBuilder<User>
{
    private readonly User _original = new();
    /// <summary>
    /// 原始对象
    /// </summary>
    public User Original
        => _original;
    /// <inheritdoc />
    public User Build()
        => _original;
    /// <inheritdoc />
    public void Save<TMember>(string name, TMember value)
    {
        switch (name)
        {
            case nameof(User.Id):
                if (value is int idValue)
                    _original.Id = idValue;
                break;
            case nameof(User.Name):
                if (value is string nameValue)
                    _original.Name = nameValue;
                break;
            case nameof(User.Age):
                if (value is int ageValue)
                    _original.Age = ageValue;
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 工厂模式
    /// </summary>
    public static readonly ICreator<UserBuilder2> Creater = new DefaultCreater<UserBuilder2>();
}
public class UserParser(HandXml xml)
    : EntityParser<User>(xml, UserBuilder2.Creater, null, true)
{
    #region 配置
    private readonly IXmlParser<int> _id = xml.Content<int>();
    private readonly ContentReader _name = xml.Content();
    private readonly IXmlParser<int> _age = xml.Content<int>();
    #endregion
    /// <inheritdoc />
    public override void ReadAttributes(IMemberStore entity, XmlReader reader) { }
    /// <inheritdoc />
    public override void ReadItem(IMemberBuilder<User> entity, XmlReader reader, string name)
    {
        if (entity is UserBuilder2 builder)
            ReadItem(builder.Original, reader, name);
        else
            base.ReadItem(entity, reader, name);
    }
    /// <summary>
    /// 使用自定义构造器
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    public void ReadItem(User entity, XmlReader reader, string name)
    {        
        switch (name)
        {
            case nameof(User.Id):
                if (_id.TryParse(reader, out var idResult))
                    entity.Id = idResult;
                break;
            case nameof(User.Name):
                if (_name.TryParse(reader, out var nameResult))
                    entity.Name = nameResult;
                break;
            case nameof(User.Age):
                if (_age.TryParse(reader, out var ageResult))
                    entity.Age = ageResult;
                break;
        }
    }
}
~~~

### 1.3 性能分析
>* ParseXml开箱即用性能就比XmlSerializer好不少
>* 通过定制对象构建组件或扩展重写能进一步提高性能
>* 定制对象构建组件和扩展重写解析器可以通过SG来生成,之后会开发相应的源码生成器

### 2. 通用性对比
#### 2.1 XmlSerializer通过Attribute标记映射
>* 通过XmlRoot、XmlAttribute和XmlElement来映射属性
>* 一个类只能支持一种格式的xml文件

~~~csharp
[XmlRoot("Root")]
public class Person
{
    [XmlAttribute("PersonId")]
    public int Id { get; set; }
    [XmlElement("PersonName")]
    public string Name { get; set; }

    private static readonly XmlSerializer _serializer = new(typeof(Person));

    [Fact]
    public void Deserialize()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Root PersonId=""1"">
    	        <PersonName>张三</PersonName>
            </Root>";
        using var stringReader = new StringReader(text);
        Person? person = _serializer.Deserialize(stringReader) as Person;
        Assert.NotNull(person);
        Assert.Equal(1, person.Id);
        Assert.Equal("张三", person.Name);
    }
    [Fact]
    public void Throw()
    {
        var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Person>
                <Id>1</Id>
    	        <Name>张三</Name>
            </Person>";
        using var stringReader = new StringReader(text);
        Assert.ThrowsAny<InvalidOperationException>(() => _serializer.Deserialize(stringReader));
    }
}
~~~

### 2.2 ParseXml通过Fluent API配置
>* WithItem配置节点
>* WithAttribute配置属性
>* Entity支持可选参数配置当前标签映射
>* 映射的成员可以是属性也可以是字段和构造函数的参数
>* 配置的解析Xml对象可以定义为静态或注入IOC容器,该对象解析Xml不产生状态支持多线程同时调用(线程安全)
>* 同一个类可以配置多个解析对象用来把不同格式的Xml转化为相同类型的对象

~~~csharp
public record User(int Id, string Name);
.
    [Fact]
    public void WithNode()
    {
        var id = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<User>
	<Id>{id}</Id>
	<Name>{name}</Name>
</User>";

        var userParser = HandXml.Default.Entity<User>()
            .WithItem<int>(nameof(User.Id))
            .WithItem(nameof(User.Name));

        User result = userParser.Get(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
    }
    [Fact]
    public void WithAttribute()
    {
        var id = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{id}"" Name=""{name}"" />";

        var userParser = HandXml.Default.Entity<User>()
            .WithAttribute<int>(nameof(User.Id))
            .WithAttribute(nameof(User.Name))
            .First("User");
        User result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
    }
    [Fact]
    public void Content()
    {
        var id = 123;
        var name = "Jxj";
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
            <User Id=""{id}"">{name}</User>";

        var userParser = HandXml.Default.Entity<User>(nameof(User.Name))
            .WithAttribute<int>(nameof(User.Id))
            .First("User");
        User result = userParser.Parse(text);
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
    }
~~~

### 2.3 通用性分析

| 特性        | XmlSerializer     | ParseXml                       |
|------------ |:----------------- |:------------------------------ |
| 同类Xml格式 | 只支持一种格式    | 支持多种                       |
| 映射对象    | 只映射属性        | 映射属性、字段和构造函数参数   |
| 映射Xml结构 | 支持属性和子节点  | 支持属性、子节点和当前节点     |
| 结果类型    | object需要转化    | 当前类型,直接使用              |

## 二、ParseXml主要功能
### 1. 读取单个节点
#### 1.1 读取原始值
>* 使用First方法读取单个节点原始值

~~~csharp
var summaryReader = HandXml.Default.First("summary");
string summary = summaryReader.Parse(xmlReader);
~~~

#### 1.2 读取强类型值
>* 使用First泛型方法读取单个节点强类型值
~~~csharp
var idParser = HandXml.Default.First<int>("Id");
int id = idParser.Parse(xmlReader);
~~~

### 2. 读取单个属性
#### 2.1 读取原始值
>* 使用Attribute方法读取属性原始值
>* 支持使用属性名或属性索引

~~~csharp
var text = @"<member name = ""F:GenerateConvertTests.Supports.ColumnType.Identity"">
    <summary>
    自增列
    </summary>
</member>";

var nameReader = HandXml.Default.Attribute("name")
    .First();
string name = nameReader.Parse(text);
~~~

### 2. 读取强类型值
>* 使用Attribute泛型方法读取属性强类型值
>* 支持使用属性名或属性索引

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"">Jxj</User>";

var idReader = HandXml.Default.Attribute<int>(0)
    .Element("User")
    .First();
int result = idReader.Parse(text);
~~~

### 3. 读取单个节点名
>* 使用Name方法读取节点名

~~~csharp
var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
        <Id>1</Id>
    </User>";
var eachParser = HandXml.Default.Name()
    .First();
var result = eachParser.Parse(text);
~~~

### 4. 解析到实体
>* xml反序列化

#### 4.1 从子节点解析
>* Entity泛型方法指定实体类型
>* WithItem方法读取子节点原始值并绑到实体
>* WithItem泛型方法读取子节点强类型值并绑到实体

~~~csharp
var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
	    <Id>123</Id>
	    <Name>Jxj</Name>
	    <Age>20</Age>
    </User>";

// Id、Name和Age节点转化为实体User的属性
 var userParser = HandXml.Default.Entity<User>()
     .WithItem<int>(nameof(User.Id))
     .WithItem(nameof(User.Name))
     .WithItem<int>(nameof(User.Age));
 User result = userParser.Get(text);
~~~

#### 4.2 从属性解析
>* Entity泛型方法指定实体类型
>* WithAttribute方法读取属性原始值并绑到实体
>* WithAttribute泛型方法读取属性强类型值并绑到实体

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"" Name=""Jxj"" Age=""20"" />";

// 第一个User节点的属性转化为实体User的属性
var userParser = HandXml.Default.Entity<User>()
    .WithAttribute<int>(nameof(User.Id))
    .WithAttribute(nameof(User.Name))
    .WithAttribute<int>(nameof(User.Age))
    .Element(nameof(User))
    .First();
User result = userParser.Parse(text);
~~~

#### 4.3 从本节点解析原始值
>* Entity可选参数contentName指定本节点原始值绑定实体的成员名

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"" Age=""age"">Jxj</User>";

var config = HandXml.Default;
// 第一个User节点的属性转化为实体User的属性,User节点的文本转化为实体User的Name
var userParser = config.Entity<User>(nameof(User.Name))
    .WithAttribute<int>(nameof(User.Id))
    .WithAttribute<int>(nameof(User.Age))
    .Element(nameof(User))
    .First();
User result = userParser.Parse(text);
~~~

### 5. 解析集合
>* 通过Each方法定义集合解析

~~~csharp
var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User>
        <Id>1</Id>
        <Name>张三</Name>
    </User>
    <User>
        <Id>2</Id>
        <Name>李四</Name>
    </User>
    <User>
        <Id>3</Id>
        <Name><![CDATA[<B>王二</B>]]></Name>
    </User>
    </Users>";

var config = HandXml.Default;
// 每一个User节点转化为实体User
var repeatReader = config.Entity<User>()
    .WithItem<int>(nameof(User.Id))
    .WithItem(nameof(User.Name))
    .Element(nameof(User))
    .Each();
User[] result = repeatReader.Get(text)
    .ToArray();
~~~

### 6. 解析字典
#### 6.1 把列表解析为字典
>* 使用Dictionary扩展方法解析字典,acceptDefault可选参数配置是否包含默认值
>* 先定义键的解析规则
>* 以子节点Id为键,子节点Name为值

~~~csharp
var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <Users>
    <User>
        <Id>1</Id>
        <Name>张三</Name>
    </User>
    <User>
        <Id>2</Id>
    </User>
    <User>
        <Id>3</Id>
        <Name><![CDATA[<B>王二</B>]]></Name>
    </User>
    </Users>";
var config = HandXml.Default;
var dictionaryReader = config.Element<int>(nameof(User.Id))
    .Dictionary(config.Element("Name").First());
IDictionary<int,string> result = dictionaryReader.Get(text);
~~~

#### 6.2 把子节点解析给字典
>* 通过Name获取节点名
>* 使用Dictionary扩展方法解析字典,acceptDefault可选参数配置是否包含默认值

~~~csharp
var text = @"<?xml version=""1.0"" encoding=""utf-8""?>
    <User>
        <Id>1</Id>
        <Name>张三</Name>
        <Age>10</Age>
    </User>";
var config = HandXml.Default;
var dictionaryReader = config.Name()
    .Dictionary(config.Content().First())
    // 跳过当前User节点
    .MoveIn()
    .Element("User")
    .First();
IDictionary<string,string> result = dictionaryReader.Parse(text);
~~~

### 7. 包装类型转化
>* Convert方法转入一个转化器可以变成另一个类型的解析器

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"">Jxj</User>";

// 第一个User节点的Id属性转化为UserId
var idReader = HandXml.Default.Attribute<long>("Id")
    .Element(nameof(User))
    .First()
    .Convert(id => new UserId(id));
UserId result = idReader.Parse(text);
~~~

### 8. 复杂类型
>* 通过WithItem可以一个解析器绑定到主解析器的成员上
>* 通过WithEache可以把集合解析器绑定到主解析器的成员上

#### 8.1 复杂类型的Case
~~~csharp
using var fs = new FileStream("rss.xml", FileMode.Open, FileAccess.Read);
using var xmlReader = XmlReader.Create(fs);
var config = HandXml.Default;
// url和title节点分别转化为RssImage实体的Url和Title
var imageParser = config.Entity<RssImage>()
    .WithItem("url", nameof(RssImage.Url))
    .WithItem("title", nameof(RssImage.Title));
// 每一个item节点的title和link分别转化为RssItem实体的Title和Link
var itemParser = config.Entity<RssItem>()
    .WithItem("title", nameof(RssItem.Title))
    .WithItem("link", nameof(RssItem.Link))
    .Element("item")
    .Each();
var rssParser = config.Entity<Rss>()
    // title节点转化为Rss实体的Title
    .WithItem<string>("title", nameof(Rss.Title))
    // image节点交给imageParser处理,处理的结果转化为Rss实体的Image
    .WithItem(imageParser, "image", nameof(Rss.Image))
    // item节点交给itemParser处理,处理的结果转化为Rss实体的Items
    .With(itemParser, "item", nameof(Rss.Items));
Rss rss = rssParser.Get(xmlReader);
~~~

#### 8.2 支持绑定子解析器的WithItem重载方法
>* item为子解析器
>* node为调用该解析器的子节点名
>* member为子解析器解析结果绑定实体成员名
>* TItem为子解析器解析结果和实体成员共同的类型
>* 如果类型不一致结果会抛异常或被抛弃

~~~csharp
EntityParser<TEntity> WithItem<TItem>(IParser<XmlReader, TItem> item, string node, string member);
~~~

## 三、ParseXml的模块化
### 1. HandXml是ParseXml的配置基座
>* HandXml包含Builders、Converters和DefaultValues
>* Builders、Converters和DefaultValues参与到解析Xml的各个过程内
>* 通过Builders、Converters和DefaultValues配置可以调整Xml解析的结果和性能

~~~csharp
class HandXml
{
    /// <summary>
    /// 构造器提供者
    /// </summary>
    IMemberBuilderProvider Builders { get; }
        /// <summary>
    /// 转化器
    /// </summary>
    XmlConvertCacher Converters { get; }
        /// <summary>
    /// 默认值提供者
    /// </summary>
    DefaultValueBuilder DefaultValues { get; }
}
~~~

#### 1.1 Builders负责对象构建
>* 默认使用反射构建
>* 支持配置

~~~csharp
    /// <summary>
    /// 注册工厂
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="provider"></param>
    /// <param name="creator"></param>
    /// <returns></returns>
    TProvider UseCreator<TProvider, TEntity>(this TProvider provider, ICreator<IMemberBuilder<TEntity>> creator);
        /// <summary>
    /// 注册工厂
    /// </summary>
    /// <typeparam name="TProvider"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="provider"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    TProvider UseCreator<TProvider, TEntity>(this TProvider provider, Func<IMemberBuilder<TEntity>> func);
~~~

#### 1.2 Converters负责xml节点原始处理
>* 默认使用XmlConvert处理
>* 默认支持18种基础类型(bool、byte、short、char、sbyte、ushort、int、uint、long、ulong、float、double、decimal、DateTime、string、Guid、DateTimeOffset和TimeSpan等)
>* 支持配置

~~~csharp
/// <summary>
/// 配置转化器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TConverter"></typeparam>
/// <param name="converter"></param>
HandXml Use<TValue, TConverter>(TConverter converter);
/// <summary>
/// 配置转化器
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="converter"></param>
HandXml Use<TValue>(Converter<string, TValue> converter);
~~~

#### 1.3 DefaultValues负责默认值
>* 各类型的默认值默认是default
>* 支持配置

~~~csharp
/// <summary>
/// 设置默认值
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <param name="value"></param>
/// <returns></returns>
DefaultValueBuilder Use<TValue>(TValue value);
~~~


## 四、ParseXml的组件化
### 1. Xml解析接口IXmlParser
>* 对Xml单节点、属性、重复节点或者复杂节点的解析类都实现了IXmlParser

~~~csharp
/// <summary>
/// Xml解析接口
/// </summary>
/// <typeparam name="TResult"></typeparam>
interface IXmlParser<TResult>
{
    /// <summary>
    /// 解析
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    bool TryParse(XmlReader reader, out TResult result);
}
~~~

### 2. IXmlParser的主要实现类
>* AttributeReader和ContentReader是基础实现
>* 其他类都是通过组装IXmlParser实例来实现
>* 通过完美的组件化可以模拟和解析复杂的Xml文件

| 类                              | 作用                                                                       |
|-------------------------------- | :------------------------------------------------------------------------- |
| AttributeReader                 | 读取属性的原始文本                                                         |
| ContentReader                   | 读取节点的原始文本                                                         |
| PrimitiveReader                 | 调用AttributeReader或ContentReader,使用Converters把文本转化为需要的类型    |
| ConvertParser\<TSource, TDest\> | 把一种类型的解析器封装为另一种类型的解析器                                 |
| FirstReader\<TResult\>          | 解析碰到的第一个该节点                                                     |
| RepeatReader\<TResult\>         | 解析连续重复的该节点                                                       | 
| EntityParser<TEntity\>          | 解析包含多个属性或子节点为实体类型的成员                                   | 

~~~csharp
class PrimitiveReader<TPrimitive>(IXmlParser<string> original, IConverter<string, TPrimitive> converter, TPrimitive defaultValue);
class ConvertParser<TSource, TDest>(HandXml xml, IXmlParser<TSource> original, IConverter<TSource, TDest> converter, TDest defaultValue);
class FirstReader<TResult>(string element, IXmlParser<TResult> original, TResult defaultValue);
class RepeatReader<TResult>(HandXml xml, string name, IXmlParser<TResult> item);
class EntityParser<TEntity>(HandXml xml, ICreator<IMemberBuilder<TEntity>> creator, IMemberParser? content, bool hasItem = false);
~~~

## 五、ParseXml的可扩展性
### 1. EntityParser\<TEntity\>的扩展性
>* EntityParser可以用来处理复杂Xml和复杂类型
>* 可扩展性在处理复杂问题时可简化配置或提供性能

### 2. UserParser扩展EntityParser的示例
>* UserParser封装了对象解析器,提高性能
>* UserParser封装了节点配置,简化使用
>* 重写ReadAttributes跳过属性解析提高性能
>* 重写ReadItem,使用switch代替字典查找提高性能

~~~csharp
public class UserParser(HandXml xml)
    : EntityParser<User>(xml, UserBuilder2.Creater, null, true)
{
    #region 配置
    private readonly IXmlParser<int> _id = xml.Content<int>();
    private readonly ContentReader _name = xml.Content();
    private readonly IXmlParser<int> _age = xml.Content<int>();
    #endregion
    /// <inheritdoc />
    public override void ReadAttributes(IMemberStore entity, XmlReader reader) { }
    /// <inheritdoc />
    public override void ReadItem(IMemberBuilder<User> entity, XmlReader reader, string name)
    {
        if (entity is UserBuilder2 builder)
            ReadItem(builder.Original, reader, name);
        else
            base.ReadItem(entity, reader, name);
    }
    /// <summary>
    /// 使用自定义构造器
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="reader"></param>
    /// <param name="name"></param>
    public void ReadItem(User entity, XmlReader reader, string name)
    {        
        switch (name)
        {
            case nameof(User.Id):
                if (_id.TryParse(reader, out var idResult))
                    entity.Id = idResult;
                break;
            case nameof(User.Name):
                if (_name.TryParse(reader, out var nameResult))
                    entity.Name = nameResult;
                break;
            case nameof(User.Age):
                if (_age.TryParse(reader, out var ageResult))
                    entity.Age = ageResult;
                break;
        }
    }
}
~~~

nuget安装: dotnet add package Hand.ParseXml --version 0.3.1.3-alpha
源码托管地址: https://github.com/donetsoftwork/HandCore.net/tree/master/Hand.ParseXml ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net/tree/master/Hand.ParseXml

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！
