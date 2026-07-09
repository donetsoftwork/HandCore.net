# XMl解析器
>* 相当于自定义xml反序列化

## 一、读取单个节点
### 1. 读取原始值
>* 使用Single方法读取单个节点原始值

~~~csharp
var summaryReader = HandXml.Default.First("summary");
string summary = summaryReader.Get(xmlReader);
~~~

### 2. 读取强类型值
>* 使用Single泛型方法读取单个节点强类型值
~~~csharp
var idParser = HandXml.Default.First<int>("Id");
int id = idParser.Get(xmlReader);
~~~

## 二、读取单个属性
### 1. 读取原始值
>* 使用Attribute方法读取属性原始值

~~~csharp
var text = @"<member name = ""F:GenerateConvertTests.Supports.ColumnType.Identity"">
    <summary>
    自增列
    </summary>
</member>";
using var stringReader = new StringReader(text);
using var xmlReader = XmlReader.Create(stringReader);
var nameReader = HandXml.Default.Attribute("name")
    .First("member");
string name = nameReader.Get(xmlReader);
~~~

### 2. 读取强类型值
>* 使用Attribute泛型方法读取属性强类型值

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"">Jxj</User>";
using var stringReader = new StringReader(text);
using var xmlReader = XmlReader.Create(stringReader);
var idReader = HandXml.Default.Attribute<int>("Id")
    .First("User");
int result = idReader.Get(xmlReader);
~~~

## 三、解析到实体
>* xml反序列化

### 1. 从子节点解析
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
 using var stringReader = new StringReader(text);
 using var xmlReader = XmlReader.Create(stringReader);
 var userParser = HandXml.Default.Entity<User>()
     .WithItem<int>(nameof(User.Id))
     .WithItem(nameof(User.Name))
     .WithItem<int>(nameof(User.Age))
     .First();
 User result = userParser.Get(xmlReader);
~~~

### 2. 从属性解析
>* Entity泛型方法指定实体类型
>* WithAttribute方法读取属性原始值并绑到实体
>* WithAttribute泛型方法读取属性强类型值并绑到实体

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"" Name=""Jxj"" Age=""20"" />";
using var stringReader = new StringReader(text);
using var xmlReader = XmlReader.Create(stringReader);
var config = HandXml.Default;
var userParser = config.Entity<User>()
    .WithAttribute<int>(nameof(User.Id))
    .WithAttribute(nameof(User.Name))
    .WithAttribute<int>(nameof(User.Age))
    .First("User");
User result = userParser.Get(xmlReader);
~~~

### 3. 从本节点解析原始值
>* Entity可选参数contentName指定本节点原始值绑定实体的成员名

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"" Age=""age"">Jxj</User>";
using var stringReader = new StringReader(text);
using var xmlReader = XmlReader.Create(stringReader);
var config = HandXml.Default;
var userParser = config.Entity<User>(nameof(User.Name))
    .WithAttribute<int>(nameof(User.Id))
    .WithAttribute<int>(nameof(User.Age))
    .First("User");
User result = userParser.Get(xmlReader);
~~~

## 四、解析集合
>* 通过Repeat方法定义集合解析

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
using var stringReader = new StringReader(text);
using var xmlReader = XmlReader.Create(stringReader);
var config = HandXml.Default;
var elementReader = config.Entity<User>()
    .WithItem<int>(nameof(User.Id))
    .WithItem(nameof(User.Name))
    .Repeat(nameof(User));
User[] result = elementReader.Get(xmlReader)
    .ToArray();
~~~

## 五、包装类型转化
>* Convert方法转入一个转化器可以变成另一个类型的解析器

~~~csharp
var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
    <User Id=""123"">Jxj</User>";
using var stringReader = new StringReader(text);
using var xmlReader = XmlReader.Create(stringReader);
var idReader = HandXml.Default.Attribute<long>("Id")
    .Convert(id => new UserId(id))
    .First("User");
UserId result = idReader.Get(xmlReader);
~~~

## 六、复杂类型
>* 通过WithItem可以一个解析器绑定到主解析器的成员上
>* 通过WithRepeate可以把集合解析器绑定到主解析器的成员上

### 1. 复杂类型的Case
~~~csharp
using var fs = new FileStream("rss.xml", FileMode.Open, FileAccess.Read);
using var xmlReader = XmlReader.Create(fs);
var config = HandXml.Default;
var imageParser = config.Entity<RssImage>()
    .WithItem("url", nameof(RssImage.Url))
    .WithItem("name", nameof(RssImage.Name));
var itemParser = config.Entity<RssItem>()
    .WithItem("title", nameof(RssItem.Title))
    .WithItem("link", nameof(RssItem.Link))
    .Repeat("item");
var rssParser = config.Entity<Rss>()
    .WithItem<string>("title", nameof(Rss.Title))
    .WithItem(imageParser, "image", nameof(Rss.Image))
    .WithRepeat(itemParser, nameof(Rss.Items));
Rss rss = rssParser.Get(xmlReader);
~~~

### 2. 支持绑定子解析器的WithItem重载方法
>* item为子解析器
>* node为调用该解析器的子节点名
>* member为子解析器解析结果绑定实体成员名
>* TItem为子解析器解析结果和实体成员共同的类型
>* 如果类型不一致结果会被抛弃

~~~csharp
EntityParser<TEntity> WithItem<TItem>(IXmlParser<TItem> item, string node, string member);
~~~

### 3. WithRepeat方法
>* repeate为集合解析器
>* member为绑定实体成员名
>* 类型为TItem[]
>* 如果实体成员类型不是TItem[]不能用该方法,应该是WithItem结合Convert来使用

~~~csharp
EntityParser<TEntity> WithRepeat<TItem>(RepeatReader<TItem> repeat, string member);
~~~

