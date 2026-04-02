using Hand.Converters;
using Hand.Words;

namespace NamingTests.Converters;

public class PascalPathConverterTests
{
    #region Rule 
    private readonly PascalPathConverter _toPascal = new(PascalWordRule.Instance);
    private readonly PascalPathConverter _toCamel = new(CamelWordRule.Instance);
    private readonly PascalPathConverter _toLower = new(LowerWordRule.Instance);
    private readonly PascalPathConverter _toUnder = new(UnderWordRule.Instance);
    #endregion

    [Theory]
    [InlineData("abc_def", "Abc_def")]
    [InlineData("Abc_Def", "Abc_Def")]
    [InlineData("abc-def", "Abc-def")]
    [InlineData("AbcDef", "AbcDef")]
    [InlineData("abcDef", "AbcDef")]
    [InlineData("abc", "Abc")]
    [InlineData("_id", "_id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToPascal(string? fullPath, string? expected)
    {
        var actual = _toPascal.Convert(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toPascal.Convert(fullPath.AsSpan()));
    }
    [Theory]
    [InlineData("abc_def", new string[] { "Abc_def" })]
    [InlineData("Abc_Def", new string[] { "Abc_", "Def" })]
    [InlineData("abc-def", new string[] { "Abc-def" })]
    [InlineData("AbcDef", new string[] { "Abc", "Def" })]
    [InlineData("abcDef", new string[] { "Abc", "Def" })]
    [InlineData("abc", new string[] { "Abc" })]
    [InlineData("_id", new string[] { "_id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToPascal(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toPascal.Split(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toPascal.Split(fullPath.AsSpan()));
    }
    [Fact]
    public void FieldToPascal()
    {
        var fieldName = "_id";
        var propertyName = _toPascal.Convert(fieldName.AsSpan(1));
        Assert.Equal("Id", propertyName);
        Assert.Equal("Id", _toPascal.Convert(fieldName, 1));
    }
    [Theory]
    [InlineData("abc_def", "abc_def")]
    [InlineData("Abc_Def", "abc_Def")]
    [InlineData("abc-def", "abc-def")]
    [InlineData("AbcDef", "abcDef")]
    [InlineData("abcDef", "abcDef")]
    [InlineData("abc", "abc")]
    [InlineData("_id", "_id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToCamel(string? fullPath, string? expected)
    {
        var actual = _toCamel.Convert(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toCamel.Convert(fullPath.AsSpan()));
    }
    [Theory]
    [InlineData("abc_def", new string[] { "abc_def" })]
    [InlineData("Abc_Def", new string[] { "abc_", "Def" })]
    [InlineData("abc-def", new string[] { "abc-def" })]
    [InlineData("AbcDef", new string[] { "abc", "Def" })]
    [InlineData("abcDef", new string[] { "abc", "Def" })]
    [InlineData("abc", new string[] { "abc" })]
    [InlineData("_id", new string[] { "_id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToCamel(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toCamel.Split(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toCamel.Split(fullPath.AsSpan()));
    }
    [Fact]
    public void FieldToCamel()
    {
        var fieldName = "_id";        
        var parameterName = _toCamel.Convert(fieldName.AsSpan(1));
        Assert.Equal("id", parameterName);
        Assert.Equal("id", _toCamel.Convert(fieldName, 1));
    }
    [Theory]
    [InlineData("abc_def", "abc_def")]
    [InlineData("Abc_Def", "abc_def")]
    [InlineData("abc-def", "abc-def")]
    [InlineData("AbcDef", "abcdef")]
    [InlineData("abcDef", "abcdef")]
    [InlineData("abc", "abc")]
    [InlineData("_id", "_id")]
    [InlineData(null, "")]
    [InlineData("", "")]
    public void ConvertToLower(string? fullPath, string? expected)
    {
        var actual = _toLower.Convert(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toLower.Convert(fullPath.AsSpan()));
    }
    [Theory]
    [InlineData("abc_def", new string[] { "abc_def" })]
    [InlineData("Abc_Def", new string[] { "abc_", "def" })]
    [InlineData("abc-def", new string[] { "abc-def" })]
    [InlineData("AbcDef", new string[] { "abc", "def" })]
    [InlineData("abcDef", new string[] { "abc", "def" })]
    [InlineData("abc", new string[] { "abc" })]
    [InlineData("_id", new string[] { "_id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToLower(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toLower.Split(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toLower.Split(fullPath.AsSpan()));
    }
    [Fact]
    public void FieldToLower()
    {
        var fieldName = "_id";
        var parameterName = _toLower.Convert(fieldName.AsSpan(1));
        Assert.Equal("id", parameterName);
        Assert.Equal("id", _toLower.Convert(fieldName, 1));
    }
    [Theory]
    [InlineData("abc_def", "_abc_def")]
    [InlineData("Abc_Def", "_abc_Def")]
    [InlineData("abc-def", "_abc-def")]
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
        Assert.Equal(expected, _toUnder.Convert(fullPath.AsSpan()));
    }
    [Theory]
    [InlineData("abc_def", new string[] { "_abc_def" })]
    [InlineData("Abc_Def", new string[] { "_abc_", "Def" })]
    [InlineData("abc-def", new string[] { "_abc-def" })]
    [InlineData("AbcDef", new string[] { "_abc", "Def" })]
    [InlineData("abcDef", new string[] { "_abc", "Def" })]
    [InlineData("abc", new string[] { "_abc" })]
    [InlineData("_id", new string[] { "_id" })]
    [InlineData(null, new string[0])]
    [InlineData("", new string[0])]
    public void SplitToUnder(string? fullPath, IEnumerable<string> expected)
    {
        var actual = _toUnder.Split(fullPath);
        Assert.Equal(expected, actual);
        Assert.Equal(expected, _toUnder.Split(fullPath.AsSpan()));
    }
}
