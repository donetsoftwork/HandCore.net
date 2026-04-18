# 投影算法之扑风捉影

## 1、IProjection\<S, T\>接口很简单
>* TryConvert尝试映射,不符合规则就不映射
>* 其中大部分投影都是同类型

~~~csharp
interface IProjection<S, T>
{
    bool TryConvert(S source, out T value);
}
~~~
