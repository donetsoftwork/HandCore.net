using System.Globalization;

namespace Hand;

/// <summary>
/// 数值解析器
/// </summary>
/// <param name="style"></param>
/// <param name="provider"></param>
public class NumberParser(NumberStyles style, IFormatProvider provider)
{
    /// <summary>
    /// 数值解析器
    /// </summary>
    /// <param name="style"></param>
    public NumberParser(NumberStyles style)
        : this(style, NumberFormatInfo.CurrentInfo)
    {
    }
    #region 配置
    /// <summary>
    /// 数字样式    
    /// </summary>
    protected readonly NumberStyles _style = style;
    /// <summary>
    /// 格式提供程序
    /// </summary>
    protected readonly IFormatProvider _provider = provider;
    /// <summary>
    /// 数字样式
    /// </summary>
    public NumberStyles Style 
        => _style;
    /// <summary>
    /// 格式提供程序
    /// </summary>
    public IFormatProvider Provider 
        => _provider;
    #endregion


    /// <summary>
    /// 尝试解析为byte类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out byte result)
        => byte.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为sbyte类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out sbyte result)
        => sbyte.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为short类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out short result)
        => short.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为ushort类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out ushort result)
        => ushort.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为int类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out int result)
        => int.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为uint类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out uint result)
        => uint.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为long类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out long result)
        => long.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为ulong类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out ulong result)
        => ulong.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为float类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out float result)
        => float.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为double类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out double result)
        => double.TryParse(value, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为decimal类型
    /// </summary>
    /// <param name="value"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(string value, out decimal result)
        => decimal.TryParse(value, _style, _provider, out result);

#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 尝试解析为byte类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out byte result)
        => byte.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为sbyte类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out sbyte result)
        => sbyte.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为short类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out short result)
        => short.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为ushort类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out ushort result)
        => ushort.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为int类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out int result)
        => int.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为uint类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out uint result)
        => uint.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为long类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out long result)
        => long.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为ulong类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out ulong result)
        => ulong.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为float类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out float result)
        => float.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为double类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out double result)
        => double.TryParse(span, _style, _provider, out result);
    /// <summary>
    /// 尝试解析为decimal类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<char> span, out decimal result)
        => decimal.TryParse(span, _style, _provider, out result);
#endif
}
