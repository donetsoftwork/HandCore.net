using Hand.ParseXml;
using ParseXmlTests.Supports;

namespace ParseXmlTests;

public class ComplexTests
{
    [Fact]
    public void WithItem()
    {
        var userId = 123;
        var roleId = 1;
        var text = @$"<?xml version=""1.0"" encoding=""utf-8""?>
<root>
<User>
	<Id>{userId}</Id>
	<Name>Jxj</Name>
	<Age>20</Age>
</User>
<Role>
    <Id>{roleId}</Id>
    <Name>Admin</Name>
</Role>
</root>";
        var config = HandXml.Default;
        var userParser = config.Entity<User>()
            .WithItem<int>(nameof(User.Id))
            .WithItem(nameof(User.Name))
            .WithItem<int>(nameof(User.Age));
        var roleParser = config.Entity<Role>()
            .WithItem<int>(nameof(Role.Id))
            .WithItem(nameof(Role.Name));

        var complexParser = config.Entity<Complex>()
            .WithItem(userParser, nameof(Complex.User))
            .WithItem(roleParser, nameof(Complex.Role))
            .First("root");
        var result = complexParser.Get(text);
        Assert.NotNull(result);
        var user = result.User;
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        var role = result.Role;
        Assert.NotNull(role);
        Assert.Equal(roleId, role.Id);
    }
}
