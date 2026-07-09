using Hand.Rule;
using Hand.Text;
using Hand.Words;
using System.Text;

namespace Hand.Converters;

/// <summary>
/// 默认路径转化
/// </summary>
/// <param name="separators"></param>
/// <param name="destRule"></param>
public class DefaultPathConverter(IEnumerable<char> separators, IWordRule destRule)
    : StringConverter<string>, IStringSpliter
{
    #region 配置
    private readonly IEnumerable<char> _separators = separators;
    private readonly IValidation<char> _validation = Logic.Included(separators);
    private readonly IWordRule _destRule = destRule;

    /// <summary>
    /// 路径分割符
    /// </summary>
    public IEnumerable<char> Separators
        => _separators;
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
    #region IStringConverter<string>
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
            if (_validation.Validate(item))
            {
                first = true;
                continue;
            }
            if (first)
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
        var list = new List<string>();
        var builder = new StringBuilder(count);
        var first = true;
        var depth = 0;
        foreach (char item in fullPath)
        {
            if (_validation.Validate(item))
            {
                first = true;
                continue;
            }
            if (first)
            {
                if (builder.Length > 0)
                {
                    var path = builder.ToString();
                    list.Add(path);
                    builder.Clear();
                }
                _destRule.CheckFirst(builder, item, depth++);
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
