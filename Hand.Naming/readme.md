# 命名规则

## 一、 单词命名规则
### 1. IWordRule接口
>* 首字母处理规则

```csharp	
interface IWordRule
{
    /// <summary>
    /// 首字母处理
    /// </summary>
    void CheckFirst(StringBuilder builder, char first, int depth);
}
```

### 2. PascalWordRule类
>* 帕斯卡命名规则(每个单词首字母大写)

```csharp	
class PascalWordRule : IWordRule
{
    /// <summary>
    /// 首字母大写
    /// </summary>
    public static string FistToUpper(string original);
}
```

### 3. CamelWordRule类
>* 小驼峰(首字母小写‌‌)

```csharp	
class CamelWordRule : IWordRule
{
    /// <summary>
    /// 首字母小写
    /// </summary>
    public static string FistToLower(string original);
}
```

### 4. LowerWordRule类
>* 全小写

```csharp	
class UnderWordRule : IWordRule;
```

### 5. UnderWordRule类
>* 下划线开头

```csharp	
class UnderWordRule : IWordRule
{
    /// <summary>
    /// 下划线开头
    /// </summary>
    public static string Under(string original);
        /// <summary>
    /// 下换线次字母小写
    /// </summary>
    public static string UnderLower(string original);
}
```


## 二、 命名转化规则
### 1. IPathRule接口
>* 路径拆分规则

```csharp	
interface IPathRule
{
    /// <summary>
    /// 拆分
    /// </summary>
    IEnumerable<string> Split‌(string fullPath);
}
```

### 2. INameConverter接口
>* 命名转化接口

```csharp	
interface INameConverter
{
    /// <summary>
    /// 转化
    /// </summary>
    string Convert(string name);
}
```

### 3. DefaultPathConverter类
>* 默认路径转化
>* 按separators分割
>* 按destRule规则租个处理单词

```csharp
/// <summary>
/// 默认路径转化
/// </summary>
class DefaultPathConverter(IEnumerable<char> separators, IWordRule destRule)
    : INameConverter, IPathRule;
```

### 4. PascalPathConverter类
>* 帕斯卡路径转化
>* 按大写字母分割
>* 按destRule规则租个处理单词

```csharp
/// <summary>
/// 帕斯卡路径转化
/// </summary>
class PascalPathConverter(IWordRule destRule)
    : INameConverter, IPathRule;
```
