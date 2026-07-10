using Hand.ParseJson;
using ParseJsonTests.Supports;
using System.Text;
using System.Text.Json;

namespace ParseJsonTests;

public class ComplexTests
{
    [Fact]
    public void WithItem()
    {
        var userId = 123;
        var roleId = 1;
        string json = @$"{{""User"": {{ ""Id"": {userId}, ""Name"": ""Jxj"",  ""State"": true}}, ""Role"": {{ ""Id"": {roleId}, ""Name"": ""Admin""}}}}";
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
        var config = HandJson.Default;
        var userParser = config.Entity<User>()
            .WithProperty<int>(nameof(User.Id))
            .WithProperty<string>(nameof(User.Name))
            .WithProperty<bool>(nameof(User.State));
        var roleParser = config.Entity<Role>()
            .WithProperty<int>(nameof(Role.Id))
            .WithProperty<string>(nameof(Role.Name));

        var complexParser = config.Entity<Complex>()
            .WithProperty(userParser, nameof(Complex.User))
            .WithProperty(roleParser, nameof(Complex.Role));
        var result = complexParser.Parse(reader);
        Assert.NotNull(result);
        var user = result.User;
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        var role = result.Role;
        Assert.NotNull(role);
        Assert.Equal(roleId, role.Id);
    }
}
