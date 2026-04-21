using Hand.Rule;
using System.Linq;

namespace Hand.Maping;

/// <summary>
/// 结尾字符去除投影
/// </summary>
public class TrimEndProjection(char[] ends)
    : ProjectionBase<string>, IProjection<string>
{
    #region 配置
    /// <summary>
    /// 结尾字符
    /// </summary>
    private readonly char[] _ends = ends;
    /// <summary>
    /// 验证规则
    /// </summary>
    private readonly IValidation<char> _validation = Logic.Included(ends);
    /// <summary>
    /// 结尾字符
    /// </summary>
    public char[] Ends
        => _ends;
    #endregion
    #region IValidation<string>
    /// <inheritdoc />
    public override bool Validate(string source)
        => source.Length > 0 && _validation.Validate(source.Last());
    #endregion
    /// <inheritdoc />
    public override string Convert(string source)
        => source.TrimEnd(_ends);
}
