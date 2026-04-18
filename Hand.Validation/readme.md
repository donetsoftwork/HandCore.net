# 验证规则

## 1.IValidation接口
>* Validate方法验证成员名是否合法

## 2.TrueLogic
>* 恒真逻辑

~~~csharp
IValidation<string> validation = Logic.True<string>();
~~~

## 3.FalseLogic
>* 恒假逻辑

~~~csharp
IValidation<string> validation = Logic.False<string>();
~~~

## 4.IncludeRule
>* 字符串包含规则,包含指定片段

## 5. IncludedRule
>* 

## 6.PrefixRule
>* 字符串前缀规则

## 7.SuffixRule
>* 字符串后缀规则

## 8.NotLogic
>* 非逻辑

### 8.1 Not扩展方法
>* 构建NotLogic
>* 如果NotLogic再Not恢复为原逻辑对象(负负得正)

~~~csharp
public static IValidation<TArgument> Not<TArgument>(this IValidation<TArgument> validation)
{
    if(validation is NotRule<TArgument> not)
        return not.Checker;
    return new NotRule<TArgument>(validation);
}
~~~

## 9. AndLogic
>* 与逻辑

### 9.1 And扩展方法
>* 构建AndLogic

## 10. OrLogic
>* 或逻辑

### 10.1 Or扩展方法
>* 构建OrLogic


