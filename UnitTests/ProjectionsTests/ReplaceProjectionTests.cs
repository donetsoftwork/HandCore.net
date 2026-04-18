using Hand.Maping;

namespace ProjectionsTests;

public class ReplaceProjectionTests
{
    [Theory]
    [InlineData("User", "Customer", "Id", "Id")]
    [InlineData("User", "Customer", "UserName", "CustomerName")]
    [InlineData("-", "_", "user-name", "user_name")]
    [InlineData("-", "", "user-name", "username")]
    public void Convert(string target, string replacement, string source, string expected)
    {
        //target.Contains(re
        var projection = new ReplaceProjection(target, replacement);
        Assert.Equal(target, projection.Target);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
