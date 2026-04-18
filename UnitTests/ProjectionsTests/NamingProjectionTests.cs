using Hand.Converters;
using Hand.Maping;
using Hand.Rule;
using Hand.Words;

namespace ProjectionsTests;

public class NamingProjectionTests
{
    #region Rule 
    private readonly DefaultPathConverter _toPascal = new(['_', '-'], PascalWordRule.Instance);
    private readonly DefaultPathConverter _toCamel = new(['_', '-'], CamelWordRule.Instance);
    #endregion

    [Theory]
    [InlineData("Id", "Id")]
    [InlineData("user-name", "UserName")]
    [InlineData("user_name", "UserName")]
    public void Pascal(string source, string expected)
    {
        var validation = Logic.IncludeAny([.. _toPascal.Separators]);
        var projection = new NamingProjection(validation, _toPascal);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData("Id", "Id")]
    [InlineData("user-name", "userName")]
    [InlineData("user_name", "userName")]
    public void Camel(string source, string expected)
    {
        var validation = Logic.IncludeAny([.. _toCamel.Separators]);
        var projection = new NamingProjection(validation, _toCamel);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
