using Hand;
using Hand.Maping;
using Hand.Maping.Recognizers;

namespace MemberValidationTests;

public class RecognizeProjectionParserTests
{
    [Theory]
    [InlineData("Prefix User", typeof(PrefixProjection))]
    [InlineData("Suffix s", typeof(SuffixProjection))]
    [InlineData("RemovePrefix User", typeof(RemovePrefixProjection))]
    [InlineData("RemoveSuffix s", typeof(RemoveSuffixProjection))]
    [InlineData("RemovePrefix User Customer", typeof(ReplacePrefixProjection))]
    [InlineData("RemoveSuffix y ies", typeof(ReplaceSuffixProjection))]
    public void ParseProjection(string? text, Type expected)
    {
        if (MemberRecognizeParser.Default.Parse(text) is not ThroughRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var projection = recognizer.Projection;
        Assert.NotNull(projection);
        Assert.IsType(expected, projection);
    }
    [Theory]
    [InlineData("Through: Prefix User", typeof(PrefixProjection))]
    [InlineData("Through: Suffix s", typeof(SuffixProjection))]
    [InlineData("Through: RemovePrefix User", typeof(RemovePrefixProjection))]
    [InlineData("Through: RemoveSuffix s", typeof(RemoveSuffixProjection))]
    [InlineData("Through: RemovePrefix User Customer", typeof(ReplacePrefixProjection))]
    [InlineData("Through: RemoveSuffix y ies", typeof(ReplaceSuffixProjection))]
    public void ParseThrough(string? text, Type expected)
    {
        if (MemberRecognizeParser.Default.Parse(text) is not ThroughRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var projection = recognizer.Projection;
        Assert.NotNull(projection);
        Assert.IsType(expected, projection);
    }
    [Theory]
    [InlineData("Cross: Prefix User", typeof(PrefixProjection))]
    [InlineData("Cross: Suffix s", typeof(SuffixProjection))]
    [InlineData("Cross: RemovePrefix User", typeof(RemovePrefixProjection))]
    [InlineData("Cross: RemoveSuffix s", typeof(RemoveSuffixProjection))]
    [InlineData("Cross: RemovePrefix User Customer", typeof(ReplacePrefixProjection))]
    [InlineData("Cross: RemoveSuffix y ies", typeof(ReplaceSuffixProjection))]
    public void ParseCross(string? text, Type expected)
    {
        if (MemberRecognizeParser.Default.Parse(text) is not CrossRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var projection = recognizer.Projection;
        Assert.NotNull(projection);
        Assert.IsType(expected, projection);
    }
    [Theory]
    [InlineData("Filter: Prefix User", typeof(PrefixProjection))]
    [InlineData("Filter: Suffix s", typeof(SuffixProjection))]
    [InlineData("Filter: RemovePrefix User", typeof(RemovePrefixProjection))]
    [InlineData("Filter: RemoveSuffix s", typeof(RemoveSuffixProjection))]
    [InlineData("Filter: RemovePrefix User Customer", typeof(ReplacePrefixProjection))]
    [InlineData("Filter: RemoveSuffix y ies", typeof(ReplaceSuffixProjection))]
    public void ParseFilter(string? text, Type expected)
    {
        if (MemberRecognizeParser.Default.Parse(text) is not FilterRecognizer<string> recognizer)
        {
            Assert.Fail();
            return;
        }
        var projection = recognizer.Projection;
        Assert.NotNull(projection);
        Assert.IsType(expected, projection);
    }
}
