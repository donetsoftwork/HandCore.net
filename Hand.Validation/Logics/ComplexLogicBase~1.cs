using System.Collections.Generic;

namespace Hand.Rule.Logics;

/// <summary>
/// 符合逻辑基类
/// </summary>
/// <typeparam name="TArgument"></typeparam>
/// <param name="items"></param>
public abstract class ComplexLogicBase<TArgument>(List<IValidation<TArgument>> items)
    : IValidation<TArgument>
{
    #region 配置
    private readonly List<IValidation<TArgument>> _items = items;
    /// <summary>
    /// 子逻辑
    /// </summary>
    public IEnumerable<IValidation<TArgument>> Items
        => _items;
    /// <summary>
    /// 子逻辑数量
    /// </summary>
    public int ItemCount 
        => _items.Count;
    #endregion
    /// <summary>
    /// 添加子逻辑
    /// </summary>
    /// <param name="item"></param>
    public void Add(IValidation<TArgument> item)
        => _items.Add(item);
    /// <summary>
    /// 添加多个子逻辑
    /// </summary>
    /// <param name="items"></param>
    public void AddRange(IEnumerable<IValidation<TArgument>> items)
        => _items.AddRange(items);
    /// <summary>
    /// 校验
    /// </summary>
    /// <param name="items"></param>
    /// <param name="argument"></param>
    /// <returns></returns>
    protected abstract bool Validate(List<IValidation<TArgument>> items, TArgument argument);
    #region IValidation<TArgument>
    /// <inheritdoc />
    public virtual bool Validate(TArgument argument)
        => Validate(_items, argument);
    #endregion
}