using Hand.Models;

namespace GeneratePropertyTests;

public class ProductPriceTest
{
    [Fact]
    public void Test()
    {
        var value = 9.9M;
        var price = new ProductPrice(value);
        Assert.Equal(value, price.Value);
        var price2 = new ProductPrice(9.9M);
        Assert.Equal(price, price2);
        Assert.True(price == price2);
    }
}


public partial record struct ProductPrice : IEntityProperty<decimal>;