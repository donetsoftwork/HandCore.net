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
    #region Instance
    /// <summary>
    /// 默认实例
    /// </summary>
    public static IValidation<TArgument> Instance
        => Inner.Instance;
    internal static class Inner
    {
        /// <summary>
        /// 默认规则
        /// </summary>
        public static readonly IValidation<TArgument> Instance = new FalseLogic<TArgument>();
    }
    #endregion
}
