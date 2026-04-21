using Hand.Maping;

namespace ProjectionsTests;

public class FirstReturnProjectionTests
{
    private IProjection<string> _projection;
    public FirstReturnProjectionTests()
    {
        var user = Projection.RemovePrefix("User", StringComparison.OrdinalIgnoreCase);
        var u = Projection.RemovePrefix("U");
        //_projection = new FirstReturnProjection<string>(user, u);
        _projection = Projection.FirstReturn(user, u);
    }

    [Theory]
    [InlineData("UserId", "Id")]
    [InlineData("UId", "Id")]
    [InlineData("userId", "Id")]
    [InlineData("uId", "uId")]
    [InlineData("UUserName", "UserName")]
    [InlineData("UserUName", "UName")]
    public void TryConvert(string source, string expected)
    {
        _projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
}
