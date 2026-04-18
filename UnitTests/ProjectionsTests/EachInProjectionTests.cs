using Hand.Maping;
using Hand.Maping.Complexs;

namespace ProjectionsTests;

public class EachInProjectionTests
{
    private EachInProjection<string> _projection;
    private EachInProjection<string> _failContinue;
    public EachInProjectionTests()
    {
        var user = Projection.RemovePrefix("User", StringComparison.OrdinalIgnoreCase);
        var u = Projection.RemovePrefix("U");
        _projection = new EachInProjection<string>(false, user, u);
        _failContinue = new EachInProjection<string>(true, user, u);
    }

    [Theory]
    [InlineData("UserId", "Id")]
    [InlineData("UId", "UId")]
    [InlineData("userId", "Id")]
    [InlineData("uId", "uId")]
    [InlineData("UUserName", "UUserName")]
    [InlineData("UserUName", "Name")]
    public void TryConvert(string source, string expected)
    {
        _projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData("UserId", "Id")]
    [InlineData("UId", "Id")]
    [InlineData("userId", "Id")]
    [InlineData("uId", "uId")]
    [InlineData("UUserName", "UserName")]
    [InlineData("UserUName", "Name")]
    public void FailContinue(string source, string expected)
    {
        _failContinue.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
