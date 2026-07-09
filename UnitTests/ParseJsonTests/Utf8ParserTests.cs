using System.Buffers.Text;
using System.Text;

namespace ParseJsonTests;

public class Utf8ParserTests
{
    [Fact]
    public void ParseInt32()
    {
        ReadOnlySpan<byte> span = Encoding.UTF8.GetBytes("12345");
        bool success = Utf8Parser.TryParse(span, out int value, out int bytesConsumed);
        Assert.True(success);
        Assert.Equal(12345, value);
        Assert.Equal(span.Length, bytesConsumed);
    }
    [Fact]
    public void ParseDouble()
    {
        ReadOnlySpan<byte> span = Encoding.UTF8.GetBytes("123.45");
        bool success = Utf8Parser.TryParse(span, out double value, out int bytesConsumed);
        Assert.True(success);
        Assert.Equal(123.45, value);
        Assert.Equal(span.Length, bytesConsumed);
    }
}
