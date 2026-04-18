namespace ProjectionsTests.Supports;

public class UserDTO
{
    public int UserId { get; set; }
    public string? UserName { get; set; }
}

public record User(int Id, string UserName);
