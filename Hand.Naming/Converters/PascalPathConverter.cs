using Hand.Paths;
using Hand.Rule;
using Hand.Text;
using Hand.Words;
using System.Text;

namespace Hand.Converters;

/// <summary>
/// 帕斯卡路径转化
/// </summary>
/// <param name="destRule"></param>
public class PascalPathConverter(IWordRule destRule)
    : StringConverter<string>, IStringSpliter
{
    #region 配置
    private readonly IValidation<char> _validation = PascalSplitRule.Validation;
    private readonly IWordRule _destRule = destRule;

    /// <summary>
    /// 分割符验证器
    /// </summary>
    public IValidation<char> Validation
        => _validation;
    /// <summary>
    /// 目标转化规则
    /// </summary>
    public IWordRule DestRule
        => _destRule;
    #endregion
    #region ISpanConverter<char, string>
    /// <inheritdoc />
    public override string Convert(ReadOnlySpan<char> name)
    {
        var count = name.Length;
        if (count == 0)
            return string.Empty;
        var builder = new StringBuilder(count);
        var first = true;
        var depth = 0;
        foreach (char item in name)
        {
            if (char.IsUpper(item) || first)
            {
                _destRule.CheckFirst(builder, item, depth++);
                first = false;
            }
            else
            {
                builder.Append(item);
            }
        }
        return builder.ToString();
    }
    #endregion
    #region IStringSpliter    
    /// <inheritdoc />
    public IEnumerable<string> Split(ReadOnlySpan<char> fullPath)
    {
        var count = fullPath.Length;
        if (count == 0)
            return [];
        var first = true;
        var list = new List<string>();
        var builder = new StringBuilder(count);
        foreach (char item in fullPath)
        {
            if (char.IsUpper(item) || first)
            {
                if (builder.Length > 0)
                {
                    var path = builder.ToString();
                    list.Add(path);
                    builder.Clear();
                }
                _destRule.CheckFirst(builder, item, list.Count);
                first = false;
            }
            else
            {
                builder.Append(item);
            }
        }

        if (builder.Length > 0)
            list.Add(builder.ToString());
        return list;
    }
    #endregion
}
