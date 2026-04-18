namespace Hand.Rule.Logics;

/// <summary>
/// 非逻辑
/// </summary>
/// <typeparam name="TArgument"></typeparam>
/// <param name="checker"></param>
public sealed class NotLogic<TArgument>(IValidation<TArgument> checker)
    : IValidation<TArgument>
{
    #region 配置
    private readonly IValidation<TArgument> _checker = checker;
    /// <summary>
    /// 验证器
    /// </summary>
    public IValidation<TArgument> Checker 
        => _checker;
    #endregion
    #region IValidation<TArgument>
    /// <inheritdoc />
    public bool Validate(TArgument argument)
        => !_checker.Validate(argument);
    #endregion
}
