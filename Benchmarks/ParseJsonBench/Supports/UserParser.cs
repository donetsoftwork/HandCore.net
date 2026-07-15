using Hand.Creational;
using Hand.ParseJson;
using Hand.ParseJson.Contracts;
using System.Text.Json;

namespace ParseJsonBench.Supports;

public class UserParser(HandJson json)
    : EntityParser<User>(json, UserBuilder2.Creater, default!)
{
    #region 配置
    private readonly IJsonParser<int> _id = json.Value<int>();
    private readonly IJsonParser<string> _name = json.Value<string>();
    private readonly IJsonParser<int> _age = json.Value<int>();
    #endregion
    public override void ReadProperty(IMemberStore entity, ref Utf8JsonReader reader, string name)
    {
        if (entity is UserBuilder2 builder)
            ReadProperty(builder.Original, ref reader, name);
        else
            base.ReadProperty(entity, ref reader, name);
    }
    public void ReadProperty(User entity, ref Utf8JsonReader reader, string name)
    {
        switch (name)
        {
            case nameof(User.Id):
                if (_id.TryParse(ref reader, out var idResult))
                    entity.Id = idResult;
                break;
            case nameof(User.Name):
                if (_name.TryParse(ref reader, out var nameResult))
                    entity.Name = nameResult;
                break;
            case nameof(User.Age):
                if (_age.TryParse(ref reader, out var ageResult))
                    entity.Age = ageResult;
                break;
        }
    }
}
