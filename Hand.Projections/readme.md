# 投影算法之扑风捉影

## 一、IProjection\<S, T\>接口
>* TryConvert参数映射,不符合规则就不映射

## 二、基础投影
### 1. PrefixProjection
>* 增加前缀投影

~~~csharp
var projection = Projection.Prefix("User");
~~~

### 2. SuffixProjection
>* 增加后缀投影

~~~csharp
var projection = Projection.Suffix("s");
~~~

### 3. RemovePrefixProjection
>* 前缀去除投影

~~~csharp
var projection = Projection.RemovePrefix("User");
~~~

### 4. RemoveSuffixProjection
>* 后缀去除投影

~~~csharp
var projection = Projection.RemoveSuffix("s");
~~~

### 5. ReplaceProjection
>* 替换投影

~~~csharp
var projection = Projection.Replace("-", "_");
~~~

### 6. ReplacePrefixProjection
>* 替换前缀投影

~~~csharp
var projection = Projection.ReplacePrefix("Customer", "User");
~~~

### 7. ReplaceSuffixProjection
>* 替换后缀投影

~~~csharp
var projection = Projection.ReplaceSuffix("y", "ies");
~~~

### 8. TrimProjection
>* 前后字符去除投影

~~~csharp
var projection = Projection.Trim(' ');
~~~


### 9. TrimStartProjection
>* 前导字符去除投影

~~~csharp
var projection = Projection.TrimStart('_');
~~~

### 10. TrimEndProjection
>* 结尾字符去除投影

~~~csharp
var projection = Projection.TrimStart('s');
~~~

### 11. VerifyProjection
>* 校验投影



### 12. NamingProjection
>* 命名规则投影


### 13. DictionaryProjection
>* 字典投影

## 三、复合投影
### 1. ChainProjection
>* 链表投影

### 2. FirstReturnProjection
>* 快速结束投影

### 3. EachInProjection
>* 逐个投影

