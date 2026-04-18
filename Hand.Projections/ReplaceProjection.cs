using Hand.Rule;
using System;

namespace Hand.Maping;

/// <summary>
/// 替换投影
/// </summary>
/// <param name="target">目标</param>
/// <param name="replacement">替身</param>
/// <param name="start">开始位置</param>
/// <param name="comparison"></param>
public class ReplaceProjection(string target, string replacement, int start = 0, StringComparison comparison = StringComparison.Ordinal)
    : ProjectionBase<string>, IConverter<string, string>
{
    #region 配置
    private readonly string _target = target;
    private readonly string _replacement = replacement;
    /// <summary>
    /// 验证规则
    /// </summary>
    private readonly IValidation<string> _validation = Logic.Include(target, start, comparison);
    /// <summary>
    /// 目标
    /// </summary>
    public string Target
        => _target;
    /// <summary>
    /// 替身
    /// </summary>
    public string Replacement 
        => _replacement;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => _validation.Validate(_target);
    #endregion
    #region IConverter<string, string>
    /// <inheritdoc />
    public override string Convert(string source)
        => source.Replace(_target, _replacement);
    #endregion
}
