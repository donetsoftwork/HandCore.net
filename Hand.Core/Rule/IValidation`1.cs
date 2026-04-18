namespace Hand.Rule;

/// <summary>
/// 验证规则
/// </summary>
public interface IValidation<TArgument>
{
    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="argument"></param>
    /// <returns></returns>
    bool Validate(TArgument argument);
}
