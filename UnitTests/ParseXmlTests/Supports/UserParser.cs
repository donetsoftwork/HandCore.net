using Hand.Creational;
using Hand.ParseXml;

namespace ParseXmlTests.Supports;

public class UserParser : EntityParser<User>
{
    public UserParser(HandXml xml)
        : base(xml, new UserCreater(), null)
    {
        WithItem<int>(nameof(User.Id));
        WithItem(nameof(User.Name));
        WithItem<int>(nameof(User.Age));
    }
    #region IMemberBuilder
    public override IMemberBuilder<User> New()
        => new UserBuilder();
    #endregion
}
