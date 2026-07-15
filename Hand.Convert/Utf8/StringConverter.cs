using Hand.Convert;
using Hand.Maping;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hand.Utf8;

/// <summary>
/// 字符串转化器
/// </summary>
public class StringConverter
    : ISpanConverter<byte, string>
    , ISpanParser<byte, string>
{
    private StringConverter() { }
    #region 配置
    private static readonly UTF8Encoding _encoding = new(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

    /// <summary>
    /// 编码
    /// </summary>
    public static UTF8Encoding Encoding
        => _encoding;
    #endregion
    /// <inheritdoc />
    public string Convert(ReadOnlySpan<byte> source)
        => GetString(source);
    /// <inheritdoc />
    public bool TryParse(ReadOnlySpan<byte> resource, out string result)
    {
        result = GetString(resource);
        return true;
    }

    /// <summary>
    /// 单例
    /// </summary>
    public static readonly StringConverter Instance = new();

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="span"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string GetString(ReadOnlySpan<byte> span)
    {
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        return _encoding.GetString(span);
#else
        return _encoding.GetString(span.ToArray());
#endif
    }
    /// <summary>
    /// 获取字节数
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetByteCount(string text)
        => _encoding.GetByteCount(text);
    /// <summary>
    /// 获取字节
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(string text)
        => _encoding.GetBytes(text);

    /// <summary>
    /// 复制字符为字节
    /// </summary>
    /// <param name="text"></param>
    /// <param name="bytes"></param>
    /// <returns></returns>
#if NET7_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    public static int GetBytes(ReadOnlySpan<char> text, Span<byte> bytes)
        => _encoding.GetBytes(text, bytes);
#else
    public static int GetBytes(string text, byte[] bytes)
        => _encoding.GetBytes(text, 0, text.Length, bytes, 0);
#endif
}
