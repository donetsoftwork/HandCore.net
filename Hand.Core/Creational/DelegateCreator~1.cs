namespace Hand.Creational;

/// <summary>
/// 委托构造器
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="func"></param>
public class DelegateCreator<T>(Func<T> func)
    : ICreator<T>
{
    #region 配置
    private readonly Func<T> _func = func;
    /// <summary>
    /// 委托
    /// </summary>
    public Func<T> Func 
        => _func;
    #endregion

    /// <inheritdoc />
    public T Create()
        => _func();
}
