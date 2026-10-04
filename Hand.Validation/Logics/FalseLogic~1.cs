namespace Hand.Rule.Logics;

/// <summary>
/// 恒假逻辑
/// </summary>
public sealed class FalseLogic<TArgument> : IValidation<TArgument>
{
    private FalseLogic() { }
    /// <inheritdoc />
    public bool Validate(TArgument argument)
        => false;
    /// <summary>
    /// 默认规则
    /// </summary>
    public static readonly IValidation<TArgument> Instance = new FalseLogic<TArgument>();
}
