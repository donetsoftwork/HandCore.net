using Hand.Maping;

namespace ProjectionsTests;

public class ReplaceProjectionTests
{
    [Theory]
    [InlineData("User", "Customer", "Id", "Id")]
    [InlineData("User", "Customer", "UserName", "CustomerName")]
    [InlineData("-", "_", "user-name", "user_name")]
    //[InlineData("-", "", "user-name", "username")]
    public void Convert(string target, string replacement, string source, string expected)
    {
        IProjection<string> projection = new ReplaceProjection(target, replacement);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
        var reversed = projection.Reverse();
        reversed.TryConvert(expected, out var source2);
        Assert.Equal(source, source2);
    }
}
