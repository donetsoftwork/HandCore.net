# Json解析器
>* 相当于自定义json反序列化

## 一、读取单个属性
### 1. 读取文本原始值
>* 使用First或String方法读取属性文本原始值

~~~csharp
string json = "{\"id\": 123}";
var config = HandJson.Default;
var idReader = config.First("id");
var result = idReader.Parse(json);

string json2 = "{\"name\": \"jxj\"}";
var idReader2 = config.First("name", config.String());
var result2 = idReader.Parse(json2);
~~~

### 2. 读取布尔原始值
>* 使用Bool方法读取属性布尔原始值

~~~csharp
string json = "{\"state\": true}";
var config = HandJson.Default;
var stateReader = config.First("state", config.Bool());
var result = stateReader.Parse(json);
~~~

### 3. 读取强类型值
>* 使用First泛型或Value泛型方法读取属性强类型值

~~~csharp
string json = "{\"id\": 123}";
var config = HandJson.Default;
var idReader = config.First<int>("id");
var result = idReader.Parse(json);

var idReader2 = config.First("id", config.Value<int>());
var result2 = idReader2.Parse(json);
~~~

## 二、解析到实体
>* Entity泛型方法指定实体类型
>* WithProperty泛型方法读取属性强类型值并绑到实体

~~~csharp
string json = "{ \"Id\": 123, \"Name\": \"Jxj\",  \"State\": true}";
var userParser = HandJson.Default.Entity<User>()
    .WithProperty<int>(nameof(User.Id))
    .WithProperty<string>(nameof(User.Name))
    .WithProperty<bool>(nameof(User.State));
var result = userParser.Parse(json);
~~~

## 三、解析集合
>* 通过Each方法定义集合解析

~~~csharp
string json = "[{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}, { \"Id\": 2, \"Name\": \"李四\",  \"State\": false}]";
var repeatReader = HandJson.Default.Entity<User>()
    .WithProperty<int>(nameof(User.Id))
    .WithProperty<string>(nameof(User.Name))
    .WithProperty<bool>(nameof(User.State))
    .Each();
var result = repeatReader.Parse(json);
~~~

## 四、解析字典
### 1. 把列表解析为字典
>* 先定义键的解析规则
>* 使用Dictionary扩展方法解析字典,acceptDefault可选参数配置是否包含默认值

~~~csharp
string json = "[{ \"Id\": 1, \"Name\": \"张三\"}, { \"Id\": 2, \"Name\": \"李四\"}]";

var config = HandJson.Default;
var dictionaryReader = config.Property<int>(nameof(User.Id))            
    .Dictionary(config.Property("Name").First());
IDictionary<int, string> result = dictionaryReader.Parse(json);
~~~

### 2.默认字典
>* 使用Dictionary方法解析字典,acceptDefault可选参数配置是否包含默认值
>* 支持一个值类型参数的重载
>* 支持键类型和值类型参数的重载

~~~csharp
        string json = "{ \"Id\": 1, \"Name\": \"张三\",  \"State\": true}";

        var dictionaryReader = HandJson.Default.Dictionary();
        IDictionary<string, object> result = dictionaryReader.Parse(json);
~~~

## 五、包装类型转化
>* Convert方法转入一个转化器可以变成另一个类型的解析器

~~~csharp
string json = "{\"id\": 123}";
var config = HandJson.Default;
var idReader = config.First<int>("id")
    .Convert(id => new UserId(id));
UserId result = idReader.Parse(json);
~~~

## 六、复杂类型
>* 通过WithProperty可以一个解析器绑定到主解析器的成员上

~~~csharp
var userId = 123;
var roleId = 1;
string json = @$"{{""User"": {{ ""Id"": {userId}, ""Name"": ""Jxj"",  ""State"": true}}, ""Role"": {{ ""Id"": {roleId}, ""Name"": ""Admin""}}}}";
var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
var config = HandJson.Default;
var userParser = config.Entity<User>()
    .WithProperty<int>(nameof(User.Id))
    .WithProperty<string>(nameof(User.Name))
    .WithProperty<bool>(nameof(User.State));
var roleParser = config.Entity<Role>()
    .WithProperty<int>(nameof(Role.Id))
    .WithProperty<string>(nameof(Role.Name));

var complexParser = config.Entity<Complex>()
    .WithProperty(userParser, nameof(Complex.User))
    .WithProperty(roleParser, nameof(Complex.Role));
var result = complexParser.Parse(reader);
~~~
