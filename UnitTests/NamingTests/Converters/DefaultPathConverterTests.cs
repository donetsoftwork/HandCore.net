using Hand.Converters;
using Hand.Words;

namespace NamingTests.Converters;

public class DefaultPathConverterTests
{
    #region Rule 
    private readonly DefaultPathConverter _toPascal = new(['_', '-'], PascalWordRule.Instance);
    private readonly DefaultPathConverter _toCamel = new(['_', '-'], CamelWordRule.Instance);
    private readonly DefaultPathConverter _toLower = new(['_', '-'], LowerWordRule.Instance);
    private readonly DefaultPathConverter _toUnder = new(['_', '-'], UnderWordRule.Instance);
    #endregion

    [Theory]
    [InlineData("abc_def", "AbcDef")]
    [InlineData("Abc_Def", "AbcDef")]
    [InlineData("abc-def", "AbcDef")]
    [InlineData("AbcDef", "AbcDef")]
    [InlineData("abcDef", "AbcDef")]
    [InlineData("abc", "Abc")]
    [InlineData("_id", "Id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToPascal(string? fullPath, string? expected)
    {
        var actual = _toPascal.Convert(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", new string[] { "Abc", "Def" })]
    [InlineData("Abc_Def", new string[] { "Abc", "Def" })]
    [InlineData("abc-def", new string[] { "Abc", "Def" })]
    [InlineData("AbcDef", new string[] {"AbcDef" })]
    [InlineData("abcDef", new string[] {"AbcDef" })]
    [InlineData("abc", new string[] { "Abc" })]
    [InlineData("_id", new string[] { "Id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToPascal(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toPascal.Split(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", "abcDef")]
    [InlineData("Abc_Def", "abcDef")]
    [InlineData("abc-def", "abcDef")]
    [InlineData("AbcDef", "abcDef")]
    [InlineData("abcDef", "abcDef")]
    [InlineData("abc", "abc")]
    [InlineData("_id", "id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToCamel(string? fullPath, string? expected)
    {
        var actual = _toCamel.Convert(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", new string[] { "abc", "Def" })]
    [InlineData("Abc_Def", new string[] { "abc", "Def" })]
    [InlineData("abc-def", new string[] { "abc", "Def" })]
    [InlineData("AbcDef", new string[] { "abcDef" })]
    [InlineData("abcDef", new string[] { "abcDef" })]
    [InlineData("abc", new string[] { "abc" })]
    [InlineData("_id", new string[] { "id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToCamel(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toCamel.Split(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", "abcdef")]
    [InlineData("Abc_Def", "abcdef")]
    [InlineData("abc-def", "abcdef")]
    [InlineData("AbcDef", "abcDef")]
    [InlineData("abcDef", "abcDef")]
    [InlineData("abc", "abc")]
    [InlineData("_id", "id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToLower(string? fullPath, string? expected)
    {
        var actual = _toLower.Convert(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", new string[] { "abc", "def" })]
    [InlineData("Abc_Def", new string[] { "abc", "def" })]
    [InlineData("abc-def", new string[] { "abc", "def" })]
    [InlineData("AbcDef", new string[] { "abcDef" })]
    [InlineData("abcDef", new string[] { "abcDef" })]
    [InlineData("abc", new string[] { "abc" })]
    [InlineData("_id", new string[] { "id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToLower(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toLower.Split(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", "_abcDef")]
    [InlineData("Abc_Def", "_abcDef")]
    [InlineData("abc-def", "_abcDef")]
    [InlineData("AbcDef", "_abcDef")]
    [InlineData("abcDef", "_abcDef")]
    [InlineData("abc", "_abc")]
    [InlineData("_id", "_id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToUnder(string? fullPath, string? expected)
    {
        var actual = _toUnder.Convert(fullPath);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("abc_def", new string[] { "_abc", "Def" })]
    [InlineData("Abc_Def", new string[] { "_abc", "Def" })]
    [InlineData("abc-def", new string[] { "_abc", "Def" })]
    [InlineData("AbcDef", new string[] { "_abcDef" })]
    [InlineData("abcDef", new string[] { "_abcDef" })]
    [InlineData("abc", new string[] { "_abc" })]
    [InlineData("_id", new string[] { "_id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToUnder(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toUnder.Split(fullPath);
        Assert.Equal(expected, actual);
    }
}
