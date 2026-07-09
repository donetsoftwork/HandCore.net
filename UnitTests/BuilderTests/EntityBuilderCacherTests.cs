using BuilderTests.Supports;
using Hand.Cache;
using Hand.Cachers;

namespace BuilderTests;

public class EntityBuilderCacherTests
{
    private readonly EntityBuilderCacher _entityBuilderCacher = new(new DictionaryCacher<Type>());

    [Fact]
    public void BuildUser()
    {
        var creator = _entityBuilderCacher.Get<User>();
        Assert.NotNull(creator);
        var builder = creator.Create();
        //var slot1 = builder.GetSlot<int>(nameof(User.Id));
        //Assert.NotNull(slot1);
        var id = 123;
        builder.Save(nameof(User.Id), id);
        //slot1.Save(id);
        //var slot2 = builder.GetSlot<string>(nameof(User.Name));
        //Assert.NotNull(slot2);
        var name = "JXJ";
        builder.Save(nameof(User.Name), name);
        //slot2.Save(name);
        var user = builder.Build();
        Assert.Equal(id, user.Id); 
        Assert.Equal(name, user.Name);
    }
    [Fact]
    public void BuildUserDTO()
    {
        var creator = _entityBuilderCacher.Get<UserDTO>();
        Assert.NotNull(creator);
        var builder = creator.Create();
        //var slot1 = builder.GetSlot<int>(nameof(UserDTO.Id));
        //Assert.NotNull(slot1);
        var id = 123;
        builder.Save(nameof(UserDTO.Id), id);
        //slot1.Save(id);
        //var slot2 = builder.GetSlot<string>(nameof(UserDTO.Name));
        //Assert.NotNull(slot2);
        var name = "JXJ";
        builder.Save(nameof(UserDTO.Name), name);
        //slot2.Save(name);
        var dto = builder.Build();
        Assert.Equal(id, dto.Id);
        Assert.Equal(name, dto.Name);
    }
}
