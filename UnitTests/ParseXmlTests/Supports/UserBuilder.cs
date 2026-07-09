using Hand.Creational;

namespace ParseXmlTests.Supports;

public class UserCreater(int id = 0, string name = "", int age = 0)
    : ICreator<UserBuilder>
{
    protected int _id = id;
    protected string _name = name;
    protected int _age = age;

    public UserBuilder Create()
        => new(_id, _name, _age);
}
public class UserBuilder(int id = 0, string name = "", int age = 0)
    : UserCreater(id, name, age)
    ,IMemberBuilder<User>
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public User Build()
        => new(_id, _name, _age);

    //public ISlotStore<TMember>? GetSlot<TMember>(string memberName)
    //{
    //    return memberName switch
    //    {
    //        nameof(User.Id) => new IdStore(this) as ISlotStore<TMember>,
    //        nameof(User.Name) => this as ISlotStore<TMember>,
    //        nameof(User.Age) => new AgeStore(this) as ISlotStore<TMember>,
    //        _ => null,
    //    };
    //}

    public void Save<TMember>(string name, TMember value)
    {
        if (name == nameof(User.Id))
        {
            if (value is int idValue)
                _id = idValue;
        }
        else if (name == nameof(User.Name))
        {
            if (value is string nameValue)
                _name = nameValue;
        }
        else if (name == nameof(User.Age))
        {
            if (value is int ageValue)
                _age = ageValue;
        }
    }

    //class NameStore(UserBuilder original) : ISlotStore<string>
    //{
    //    private readonly UserBuilder _original = original;
    //    public void Save(string value)
    //        => _original._name = value;
    //    void ISlotStore.Save(object value)
    //        => Save((string)value);
    //}
    //class IdStore(UserBuilder original) : ISlotStore<int>
    //{
    //    private readonly UserBuilder _original = original;
    //    public void Save(int value)
    //        => _original._id = value;
    //    void ISlotStore.Save(object value)
    //        => Save((int)value);
    //}
    //class AgeStore(UserBuilder original) : ISlotStore<int>
    //{
    //    private readonly UserBuilder _original = original;
    //    public void Save(int value)
    //        => _original._age = value;
    //    void ISlotStore.Save(object value)
    //        => Save((int)value);
    //}
}