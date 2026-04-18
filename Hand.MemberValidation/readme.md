# 成员规则解析
>* 通过MemberRuleParser解析

## 1. MemberRuleParser类
>* 前缀与配置项之间至少要有一个分隔符

~~~csharp
class MemberRuleParser(string include, string exclude, char[] separators);
~~~

## 2. 解析规则
>* 通过Parse方法把字符串配置解析为对应的规则实例
>* 配置为空或null时，返回Logic.True<string>()
>* 配置为"Empty"时，返回Logic.False<string>()
>* 配置以IncludePrefix开头时，返回IncludedRule实例
>* 配置以ExcludePrefix开头时，返回IncludedRule.Not()实例
>* IncludePrefix、ExcludePrefix和成员名之间分隔符可以自定义

## 3. Default配置
>* 解析为TrueLogic,Validate始终返回true
>* 支持null,ALL和空字符串

~~~csharp
[Theory]
[InlineData("")]
[InlineData(null)]
[InlineData("ALL")]
[InlineData("all")]
[InlineData("All")]
public void Default(string? text)
{
    var rule = MemberRuleParser.Default.Parse(text);
    Assert.IsType<TrueLogic<string>>(rule);
    Assert.True(rule.Validate("Id"));
}
~~~

## 4. Empty配置
>* 解析为FalseLogic,Validate始终返回false
>* 配置为Empty

~~~csharp
[Theory]
[InlineData("Empty")]
[InlineData("empty")]
public void Empty(string? text)
{
    var rule = MemberRuleParser.Default.Parse(text);
    Assert.IsType<FalseLogic<string>>(rule);
    Assert.False(rule.Validate("Id"));
}
~~~

## 5. Include配置
>* 配置以IncludePrefix开头时
>* 配置的项目返回true,其他为false

~~~csharp
var rule = MemberRuleParser.Default.Parse("Include: Id Name");
Assert.True(rule.Validate("Id"));
Assert.True(rule.Validate("Name"));
Assert.False(rule.Validate("Other"));
~~~

## 6. Exclude配置
>* 配置以ExcludePrefix开头时
>* 配置的项目返回false,其他为true

~~~csharp
var rule = MemberRuleParser.Default.Parse("Exclude: Id Name");
Assert.False(rule.Validate("Id"));
Assert.False(rule.Validate("Name"));
Assert.True(rule.Validate("Other"));
~~~

## 7. 配置前缀和分隔符及是否忽略大小写

~~~csharp
var parser = new MemberRuleParser("I:", "E:", [',', ' '], StringComparer.Ordinal);
var rule = parser.Parse("I: Id,Name");
Assert.True(rule.Validate("Id"));
Assert.True(rule.Validate("Name"));
Assert.False(rule.Validate("Other"));
~~~