using Hand.Maping;

namespace Hand.Text;

/// <summary>
/// 字符串转换器
/// </summary>
public interface IStringConverter<out TDest>
    : IConverter<string, TDest>
    , IConverter<char[], TDest>
    , ISpanConverter<char, TDest>
{
}
