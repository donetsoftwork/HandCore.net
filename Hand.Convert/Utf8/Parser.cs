using System.Buffers.Text;

namespace Hand.Utf8;

/// <summary>
/// UTF-8 解析器
/// </summary>
/// <param name="standardFormat"></param>
public class Parser(char standardFormat)
{
    #region 配置
    /// <summary>
    /// 格式
    /// </summary>
    private readonly char _standardFormat = standardFormat;
    /// <summary>
    /// 格式
    /// </summary>
    public char StandardFormat 
        => _standardFormat;
    #endregion
    /// <summary>
    /// 尝试解析为 bool 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out bool result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 byte 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out byte result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 sbyte 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out sbyte result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 short 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out short result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 ushort 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out ushort result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 int 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out int result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 uint 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out uint result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 long 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out long result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 ulong 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out ulong result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 float 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out float result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 double 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out double result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 decimal 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out decimal result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 DateTime 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out DateTime result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 DateTimeOffset 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out DateTimeOffset result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 Guid 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out Guid result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
    /// <summary>
    /// 尝试解析为 TimeSpan 类型
    /// </summary>
    /// <param name="span"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool TryParse(ReadOnlySpan<byte> span, out TimeSpan result)
        => Utf8Parser.TryParse(span, out result, out var consumed, _standardFormat) && consumed == span.Length;
}
