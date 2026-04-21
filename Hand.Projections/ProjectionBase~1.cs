using Hand.Rule;

namespace Hand.Maping;

/// <summary>
/// 同类投影基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ProjectionBase<T> : IValidation<T>
{
    /// <inheritdoc />
    public virtual bool TryConvert(T source, out T value)
    {
        if (Validate(source))
        {
            value = Convert(source);
            return true;
        }
        value = source;
        return false;
    }
    //#endregion
    /// <summary>
    /// 转化
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public abstract T Convert(T source);
    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public abstract bool Validate(T source);
}
