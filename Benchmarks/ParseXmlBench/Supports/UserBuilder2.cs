using Hand.Creational;

namespace ParseXmlBench.Supports;

/// <summary>
/// 建造者模式
/// </summary>
public class UserBuilder2
    : IMemberBuilder<User>
{
    private readonly User _original = new();
    /// <summary>
    /// 原始对象
    /// </summary>
    public User Original
        => _original;
    /// <inheritdoc />
    public User Build()
        => _original;
    /// <inheritdoc />
    public void Save<TMember>(string name, TMember value)
    {
        switch (name)
        {
            case nameof(User.Id):
                if (value is int idValue)
                    _original.Id = idValue;
                break;
            case nameof(User.Name):
                if (value is string nameValue)
                    _original.Name = nameValue;
                break;
            case nameof(User.Age):
                if (value is int ageValue)
                    _original.Age = ageValue;
                break;
        }
    }
    /// <summary>
    /// 工厂模式
    /// </summary>
    public static readonly ICreator<UserBuilder2> Creater = new DefaultCreater<UserBuilder2>();
}