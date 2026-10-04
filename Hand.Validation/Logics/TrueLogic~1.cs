namespace Hand.Rule.Logics;

/// <summary>
/// 恒真逻辑
/// </summary>
public sealed class TrueLogic<TArgument> : IValidation<TArgument>
{
    private TrueLogic() { }
    /// <inheritdoc />
    public bool Validate(TArgument argument)
        => true;
    /// <summary>
    /// 默认规则
    /// </summary>
    public static readonly IValidation<TArgument> Instance = new TrueLogic<TArgument>();
}
