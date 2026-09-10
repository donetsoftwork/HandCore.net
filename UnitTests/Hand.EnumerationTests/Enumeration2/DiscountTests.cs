namespace Hand.EnumerationTests.Enumeration2;

public class DiscountTests
{
    [Fact]
    public void Buy()
    {
        var user = GetUserByDb("张三");
        var cardType = CardType.Provider.Get(user.CardType);
        Assert.NotNull(cardType);
        var product = GetProductByDb("帽子");
        // 随机领取优惠券
        decimal coupon = Random.Shared.Next(0, 20);
        // 价格
        var price = product.Price;
        // 使用优惠券
        decimal discount = coupon;
        // 享受会员折扣
        var state = cardType.CheckDiscount(price, ref discount);
        if (state)
            Console.WriteLine("使用会员卡有优惠");
        // 计算应付金额
        var cost = price - discount;
        Assert.True(cost < price);
        
    }
    public User GetUserByDb(string name)
        => new(name, "Silver");
    public Product GetProductByDb(string name)
        => new(name, 100M);
    public record User(string Name, string CardType);
    public record Product(string Name, decimal Price);
}
