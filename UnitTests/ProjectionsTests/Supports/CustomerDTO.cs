namespace ProjectionsTests.Supports;

public class CustomerDTO
{
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int CustomerLevel { get; set; }
}

public record Customer(int Id, string Name, int Level);