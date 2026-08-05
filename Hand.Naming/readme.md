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
    public static string FistToUpper(ReadOnlySpan<char> original);
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
    public static string FistToLower(ReadOnlySpan<char> original);
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
    /// 下换线次字母小写
    /// </summary>
    public static string UnderLower(ReadOnlySpan<char> original);
}
```

## 二、 命名转化规则
### 1. IStringSpliter接口
>* 字符拆分规则

```csharp	
interface IStringSpliter
{
    /// <summary>
    /// 拆分
    /// </summary>
    IEnumerable<string> Split(ReadOnlySpan<char> str);
}
```

### 2. ISpanConverter<char, string>接口
>* 字符转化接口

```csharp	
interface ISpanConverter<char, string>
{
    /// <summary>
    /// 转化
    /// </summary>
    string Convert(ReadOnlySpan<char> source);
}
```

### 3. DefaultPathConverter类
>* 默认路径转化
>* 按separators分割
>* 按destRule规则逐个处理单词

```csharp
/// <summary>
/// 默认路径转化
/// </summary>
class DefaultPathConverter(IEnumerable<char> separators, IWordRule destRule)
    : StringConverter<string>, IStringSpliter;
```

### 4. PascalPathConverter类
>* 帕斯卡路径转化
>* 按大写字母分割
>* 按destRule规则逐个处理单词

```csharp
/// <summary>
/// 帕斯卡路径转化
/// </summary>
class PascalPathConverter(IWordRule destRule)
    : StringConverter<string>, IStringSpliter;
```
