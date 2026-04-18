using System.Collections.Generic;

namespace Hand.Rule;

/// <summary>
/// 被包含验证规则(成员之一)
/// </summary>
/// <param name="members"></param>
public sealed class IncludedRule<TMember>(ISet<TMember> members)
    : IValidation<TMember>
{
    #region 配置
    private readonly ISet<TMember> _members = members;
    /// <summary>
    /// 预设成员
    /// </summary>
    public ISet<TMember> Members 
        => _members;
    #endregion
    #region IValidation<TEntity>
    /// <inheritdoc />
    public bool Validate(TMember argument)
        => _members.Contains(argument);
    #endregion
}
