using Hand.Maping;
using Hand.Maping.Complexs;

namespace ProjectionsTests;

public class ChainProjectionTests
{
    private readonly IProjection<string>[] _toPlural;
    private readonly IProjection<string>[] _toSingular;    

    public ChainProjectionTests()
    {
        // 单数转复数
        _toPlural = [
            new DictionaryProjection<string>(new Dictionary<string,string>{
                { "foot", "feet" },
                { "mouse", "mice" }
            }),
            new ReplaceSuffixProjection("y", "ies"),
            new ReplaceSuffixProjection("s", "ses"),
            new ReplaceSuffixProjection("x", "xes"),
            new ReplaceSuffixProjection("f", "ves"),
            new ReplaceSuffixProjection("fe", "ves"),
            new ReplaceSuffixProjection("sh", "shes"),
            new ReplaceSuffixProjection("ch", "ches"),
            new ReplaceSuffixProjection("man", "men"),
            new SuffixProjection("s")
            ];
        // 复数转单数
        _toSingular = [
            new DictionaryProjection<string>(new Dictionary<string,string>{
                { "feet", "foot" },
                { "mice", "mouse" }
            }),
            new ReplaceSuffixProjection("ies", "y"),
            new RemoveSuffixProjection("es"),
            new RemoveSuffixProjection("s")
            ];
    }
    [Theory]
    [InlineData("foot", "feet")]
    [InlineData("mouse", "mice")]
    [InlineData("city", "cities")]
    [InlineData("bus", "buses")]
    [InlineData("box", "boxes")]
    [InlineData("knife", "knives")]
    [InlineData("wolf", "wolves")]
    [InlineData("wish", "wishes")]
    [InlineData("peach", "peaches")]
    [InlineData("man", "men")]
    [InlineData("woman", "women")]
    [InlineData("book", "books")]
    public void ToPlural(string source, string expected)
    {
        var projection = new ChainProjection<string>(_toPlural);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData("feet", "foot")]
    [InlineData("mice", "mouse")]
    [InlineData("cities", "city")]
    [InlineData("buses", "bus")]
    [InlineData("boxes", "box")]
    [InlineData("wishes", "wish")]
    [InlineData("peaches", "peach")]
    [InlineData("books", "book")]
    public void ToSingular(string source, string expected)
    {
        var projection = new ChainProjection<string>(_toSingular);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
