using Hand.Paths;
using Hand.Words;
using System.Text;

namespace Hand.Converters;

/// <summary>
/// 默认路径转化
/// </summary>
/// <param name="separators"></param>
/// <param name="destRule"></param>
public class DefaultPathConverter(IEnumerable<char> separators, IWordRule destRule)
    : DefaultSplitRule(separators), INameConverter, IPathRule
{
    #region 配置
    private readonly IWordRule _destRule = destRule;
    /// <summary>
    /// 目标转化规则
    /// </summary>
    public IWordRule DestRule
        => _destRule;
    #endregion
    #region INameConverter
    /// <inheritdoc />
    public string Convert(string name)
    {
        if (string.IsNullOrEmpty(name))
            return string.Empty;
        var builder = new StringBuilder(name.Length);
        var first = true;
        var depth = 0;
        foreach (char item in name)
        {
            if (Validate(item))
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
    /// <inheritdoc />
    public string Convert(ReadOnlySpan<char> name)
    {
        var count = name.Length;
        if (count == 0)
            return string.Empty;
        var builder = new StringBuilder(count);
        var first = true;
        var depth = 0;
        foreach (char item in name)
        {
            if (Validate(item))
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
    #region IPathConverter
    /// <inheritdoc />
    public IEnumerable<string> Split(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
            yield break;
        var builder = new StringBuilder(fullPath.Length);
        var first = true;
        var depth = 0;
        foreach (char item in fullPath)
        {
            if (Validate(item))
            {
                first = true;
                continue;
            }
            if (first)
            {
                if (builder.Length > 0)
                {
                    var path = builder.ToString();
                    yield return path;
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
            yield return builder.ToString();
    }
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
            if (Validate(item))
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
