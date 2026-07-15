# XMl解析器
>* 相当于自定义xml反序列化

## 一、读取单个节点
### 1. 读取原始值
>* 使用First方法读取单个节点原始值

~~~csharp
var summaryReader = HandXml.Default.First("summary");
string summary = summaryReader.Parse(xmlReader);
~~~

### 2. 读取强类型值
>* 使用First泛型方法读取单个节点强类型值
~~~csharp
var idParser = HandXml.Default.First<int>("Id");
int id = idParser.Parse(xmlReader);
~~~

## 二、读取单个属性
### 1. 读取原始值
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

## 三、读取单个节点名
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

## 四、解析到实体
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

// Id、Name和Age节点转化为实体User的属性
 var userParser = HandXml.Default.Entity<User>()
     .WithItem<int>(nameof(User.Id))
     .WithItem(nameof(User.Name))
     .WithItem<int>(nameof(User.Age));
 User result = userParser.Get(text);
~~~

### 2. 从属性解析
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

### 3. 从本节点解析原始值
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

## 五、解析集合
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

## 六、解析字典
### 1. 把列表解析为字典
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

### 2. 把子节点解析给字典
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

## 七、包装类型转化
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

## 八、复杂类型
>* 通过WithItem可以一个解析器绑定到主解析器的成员上
>* 通过WithEache可以把集合解析器绑定到主解析器的成员上

### 1. 复杂类型的Case
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

### 2. 支持绑定子解析器的WithItem重载方法
>* item为子解析器
>* node为调用该解析器的子节点名
>* member为子解析器解析结果绑定实体成员名
>* TItem为子解析器解析结果和实体成员共同的类型
>* 如果类型不一致结果会抛异常或被抛弃

~~~csharp
EntityParser<TEntity> WithItem<TItem>(IParser<XmlReader, TItem> item, string node, string member);
~~~
