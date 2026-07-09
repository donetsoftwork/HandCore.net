using Hand.Creational;
using Hand.ParseJson;

namespace ParseJsonTests.Supports;

public class UserParser : EntityParser<User>
{
    public UserParser(HandJson json)
        : base(json, new UserCreater(), default!)
    {
        WithProperty<int>(nameof(User.Id));
        WithProperty<string>(nameof(User.Name));
        WithProperty<int>(nameof(User.State));
    }
    #region IMemberBuilder
    public override IMemberBuilder<User> New()
        => new UserBuilder();
    #endregion
}
