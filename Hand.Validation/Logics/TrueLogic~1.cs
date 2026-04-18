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
        public static readonly IValidation<TArgument> Instance = new TrueLogic<TArgument>();
    }
    #endregion
}
