using Hand.Creational;

namespace ParseXmlBench.Supports;

/// <summary>
/// 建造者模式
/// </summary>
public class UserBuilder(int id, string name, int age)
    : IMemberBuilder<User>
{
    protected int _id = id;
    protected string _name = name;
    protected int _age = age;

    public int Id
    {
        get => _id;
        set => _id = value;
    }
    public string Name
    {
        get => _name;
        set => _name = value;
    }
    public int Age
    {
        get => _age;
        set => _age = value;
    }
    public UserBuilder()
        : this(0, "", 0)
    {
    }
    /// <summary>
    /// 构建User对象
    /// </summary>
    /// <returns></returns>
    public User Build()
        => new() { Id = _id, Name = _name, Age = _age };
    /// <inheritdoc />
    public void Save<TMember>(string name, TMember value)
    {
        switch (name)
        {
            case nameof(User.Id):
                if (value is int idValue)
                    _id = idValue;
                break;
            case nameof(User.Name):
                if (value is string nameValue)
                    _name = nameValue;
                break;
            case nameof(User.Age):
                if (value is int ageValue)
                    _age = ageValue;
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 工厂模式
    /// </summary>
    public static readonly ICreator<UserBuilder> Creater = new DefaultCreater<UserBuilder>();
}