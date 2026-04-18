using Hand.Maping;
using ProjectionsTests.Supports;

namespace ProjectionsTests;

public class ReplacePrefixProjectionTests
{
    private readonly Dictionary<string, Func<CustomerDTO, object?>> _sourceMembers = new()
    {
        [nameof(CustomerDTO.CustomerId)] = obj => obj.CustomerId,
        [nameof(CustomerDTO.CustomerName)] = obj => obj.CustomerName,
        [nameof(CustomerDTO.CustomerLevel)] = obj => obj.CustomerLevel
    };
    private readonly IProjection<string> _projection = Projection.ReplacePrefix("Customer", "User");

    [Theory]
    [InlineData("User", "Customer", "Id", "Id")]
    [InlineData("User", "Customer", "UserName", "CustomerName")]
    public void Convert(string prefix, string replacement, string source, string expected)
    {
        var projection = new ReplacePrefixProjection(prefix, replacement);
        Assert.Equal(prefix, projection.Prefix);
        if (projection.Validate(source))
            Assert.Equal(expected, projection.Convert(source));
        else
            Assert.Equal(expected, source);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void Cross()
    {
        var result = _projection.Cross(_sourceMembers);
        Assert.Equal(6, result.Count);
        Assert.True(result.ContainsKey(nameof(CustomerDTO.CustomerId)));
        Assert.True(result.ContainsKey(nameof(CustomerDTO.CustomerName)));
        Assert.True(result.ContainsKey(nameof(CustomerDTO.CustomerLevel)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserId)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserName)));
        Assert.True(result.ContainsKey("UserLevel"));
    }
    [Fact]
    public void Through()
    {
        var result = _projection.Through(_sourceMembers);
        Assert.Equal(3, result.Count);
        Assert.True(result.ContainsKey(nameof(UserDTO.UserId)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserName)));
    }
    [Fact]
    public void Filter()
    {
        var result = _projection.Filter(_sourceMembers);
        Assert.Equal(3, result.Count);
        Assert.True(result.ContainsKey(nameof(UserDTO.UserId)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserName)));
        Assert.True(result.ContainsKey("UserLevel"));
    }
}
