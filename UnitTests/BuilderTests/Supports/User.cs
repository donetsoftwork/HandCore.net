namespace BuilderTests.Supports;

public record User(int Id, string Name);


public class UserDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
