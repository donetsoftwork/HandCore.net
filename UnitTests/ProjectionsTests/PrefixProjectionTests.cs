using Hand.Maping;
using ProjectionsTests.Supports;

namespace ProjectionsTests;

public class PrefixProjectionTests
{
    private readonly Dictionary<string, Func<User, object>> _sourceMembers = new()
    {
        [nameof(User.Id)] = obj => obj.Id,
        [nameof(User.UserName)] = obj => obj.UserName
    };
    private readonly Dictionary<string, Action<UserDTO, object?>> _destMembers = new()
    {
        [nameof(UserDTO.UserId)] = (obj,value) => obj.UserId = (int)value!,
        [nameof(UserDTO.UserName)] = (obj, value) => obj.UserName = (string?)value,
    };
    private readonly IProjection<string> _projection = Projection.Prefix("User");

    [Theory]
    [InlineData("User", "Id", "UserId")]
    [InlineData("User", "Name", "UserName")]
    public void Convert(string prefix, string source, string expected)
    {
        IProjection<string> projection = new PrefixProjection(prefix);
        projection.TryConvert(source, out var result);
        Assert.Equal(expected, result);
        var reversed = projection.Reverse();
        reversed.TryConvert(expected, out var source2);
        Assert.Equal(source, source2);
    }
    [Fact]
    public void Cross()
    {
        var result = _projection.Cross(_sourceMembers);
        Assert.Equal(3, result.Count);
        Assert.True(result.ContainsKey(nameof(User.Id)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserId)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserName)));
    }
    [Fact]
    public void Through()
    {
        var result = _projection.Through(_sourceMembers);
        Assert.Equal(_sourceMembers.Count, result.Count);
        Assert.True(result.ContainsKey(nameof(UserDTO.UserId)));
        Assert.True(result.ContainsKey(nameof(UserDTO.UserName)));
    }
    [Fact]
    public void Filter()
    {
        var result = _projection.Filter(_sourceMembers);
        Assert.Single(result);
        Assert.True(result.ContainsKey(nameof(UserDTO.UserId)));
    }
    [Fact]
    public void Mapper()
    {
        var mapper = CreateMapper();
        var user = new User(1, "Jxj");
        var dto = new UserDTO();
        mapper(user, dto);
        Assert.Equal(user.Id, dto.UserId);
        Assert.Equal(user.UserName, dto.UserName);
    }
    private Action<User, UserDTO> CreateMapper()
    {
        var sourceMembers = _projection.Cross(_sourceMembers);
        Action<User, UserDTO>? copyAction = default;
        foreach (var memberName in _destMembers.Keys)
        {
            if (sourceMembers.TryGetValue(memberName, out var sourceMember))
            {
                var destMember = _destMembers[memberName];
                copyAction += (user, dto) => destMember(dto, sourceMember(user));
            }
        }
        if (copyAction is null)
            return (user, dto) => { };
        return copyAction;
    }
}
