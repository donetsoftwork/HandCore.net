# 枚举类组件
>* 使用枚举类而不是枚举类型

## 1、官方建议使用枚举类而不是枚举类型
>* 原生枚举缺乏行为封装
>* 类型安全性不足（可强制转换非法值）
>* 违反开闭原则（需大量 switch-case）的问题

## 2. 自定义普通枚举类
### 2.1 自定义普通枚举类CardType示例
>* CardType继承枚举类Enumeration
>* 构造函数定义为私有的,避免外部生成CardType实例
>* 内部类CardTypeProvider继承EnumerationProvider,其唯一实例是CardType的静态字段Provider
>* CardType封装了一个复杂的打折逻辑
>* 银卡9折(不与其他折扣同享)
>* 金卡最低85折
>* 铂金卡最低8折
>* 黑金卡75折上折

~~~csharp
/// <summary>
/// Vip枚举
/// </summary>
public abstract class CardType : Enumeration
{
    private CardType(string name, long original, string description)
        : base(name, original, description)
    {
    }
    #region 配置
    /// <summary>
    /// 打折
    /// </summary>
    /// <param name="price"></param>
    /// <param name="discount"></param>
    /// <returns></returns>
    public abstract bool CheckDiscount(decimal price, ref decimal discount);
    /// <summary>
    /// 银卡
    /// </summary>
    public static readonly CardType Silver = new SilverType();
    /// <summary>
    /// 金卡
    /// </summary>
    public static readonly CardType Gold = new GoldType();
    /// <summary>
    /// 铂金卡
    /// </summary>
    public static readonly CardType Platinum = new PlatinumType();
    /// <summary>
    /// 黑金卡
    /// </summary>
    public static readonly CardType Black = new BlackType();

    /// <inheritdoc cref="Silver" path="/summary"/>
    public static CardType Vip => Silver;
    /// <inheritdoc cref="Gold" path="/summary"/>
    public static CardType Vip2 => Gold;
    /// <inheritdoc cref="Platinum" path="/summary"/>
    public static CardType Vip3 => Platinum;
    /// <inheritdoc cref="Black" path="/summary"/>
    public static CardType Vip4 => Black;
    #endregion
    #region 实现类
    /// <inheritdoc cref="Silver" path="/summary"/>
    private class SilverType()
        : CardType(nameof(Silver), 1, "银卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 银卡9折(不与其他折扣同享)
            if (discount > 0M)
                return false;
            discount = price * 0.1M;
            return true;
        }
    }
    /// <inheritdoc cref="Gold" path="/summary"/>
    private class GoldType()
        : CardType(nameof(Gold), 2, "金卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 金卡最低85折
            var discount2 = price * 0.15M;
            if (discount2 > discount)
            {
                discount = discount2;
                return true;
            }
            return false;
        }
    }
    /// <inheritdoc cref="Platinum" path="/summary"/>
    private class PlatinumType()
        : CardType(nameof(Platinum), 3, "铂金卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 铂金卡最低8折
            var discount2 = price * 0.2M;
            if (discount2 > discount)
            {
                discount = discount2;
                return true;
            }
            return false;
        }
    }
    /// <inheritdoc cref="Black" path="/summary"/>
    private class BlackType()
        : CardType(nameof(Black), 4, "黑金卡")
    {
        /// <inheritdoc />
        public override bool CheckDiscount(decimal price, ref decimal discount)
        {
            // 黑金卡75折上折
            var price2 = price - discount;
            discount += price2 * 0.25M;
            return true;
        }
    }
    #endregion
    #region Provider
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    public static readonly IEnumerationProvider<CardType> Provider = new CardTypeProvider();
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    class CardTypeProvider : EnumerationProvider<CardType>
    {
        internal CardTypeProvider()
            : base(
            [Silver, Gold, Platinum, Black]
            , new Dictionary<string, CardType>(StringComparer.OrdinalIgnoreCase)
            {
                { nameof(Silver), Silver},
                { nameof(Gold), Gold},
                { nameof(Platinum), Platinum},
                { nameof(Black), Black},
                { nameof(Vip) , Silver},
                { nameof(Vip2) , Gold},
                { nameof(Vip3) , Platinum},
                { nameof(Vip4) , Black},
            }
            , new Dictionary<long, CardType>()
            {
                { Silver.Original, Silver},
                { Gold.Original, Gold},
                { Platinum.Original, Platinum},
                { Black.Original, Black}
            }
        )
        {
        }
    }
    #endregion
}
~~~

### 2.2 枚举类演示打折逻辑
>* 模拟业务场景是张三买帽子
>* GetUserByDb和GetProductByDb模拟从存储获取数据
>* 由于会员卡枚举类封装了打折逻辑
>* 使用就非常简单
>* 不需要switch-case来处理不同枚举项
>* 大家可以自行脑补使用enum如何实现同样的逻辑

~~~csharp
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

public User GetUserByDb(string name)
    => new(name, "Silver");
public Product GetProductByDb(string name)
    => new(name, 100M);
public record User(string Name, string CardType);
public record Product(string Name, decimal Price);
~~~

## 3. 自定义位标记枚举类
### 3.1 自定义位标记枚举类DaysOfWeek示例
>* DaysOfWeek继承标记枚举类FlagEnumeration
>* 构造函数定义为私有的,避免外部生成CardType实例
>* 内部类DaysOfWeekProvider继承FlagEnumerationProvider,其唯一实例是DaysOfWeek的静态字段Provider
>* Sunday、Monday到Saturday表示一周的七天
>* Unknown表示空

~~~csharp
/// <summary>
/// 星期几枚举
/// </summary>
public sealed class DaysOfWeek
    : FlagEnumeration
{
    /// <summary>
    /// 这个构造函数是关键,没有这个构造函数会报错
    /// </summary>
    /// <param name="flags"></param>
    /// <param name="original"></param>
    /// <param name="description"></param>
    private DaysOfWeek(ISet<string> flags, long original, string description = "")
        : base(flags, original, description)
    {
    }
    private DaysOfWeek(IEqualityComparer<string> comparer, string name, long original, string description = "")
         : base(comparer, name, original, description)
    {
    }
    private static readonly IEqualityComparer<string> _comparer = StringComparer.OrdinalIgnoreCase;
    #region Days
    /// <summary>
    /// 未知
    /// </summary>
    public static readonly DaysOfWeek Unknown = new(_comparer, nameof(Unknown), 0, "未知");
    /// <summary>
    /// 星期日
    /// </summary>
    public static readonly DaysOfWeek Sunday = new(_comparer, nameof(Sunday), 1, "星期日");
    /// <summary>
    /// 星期一
    /// </summary>
    public static readonly DaysOfWeek Monday = new(_comparer, nameof(Monday), 1 << 1, "星期一");
    /// <summary>
    /// 星期二
    /// </summary>
    public static readonly DaysOfWeek Tuesday = new(_comparer, nameof(Tuesday), 1 << 2, "星期二");
    /// <summary>
    /// 星期三
    /// </summary>
    public static readonly DaysOfWeek Wednesday = new(_comparer, nameof(Wednesday), 1 << 3, "星期三");
    /// <summary>
    /// 星期四
    /// </summary>
    public static readonly DaysOfWeek Thursday = new(_comparer, nameof(Thursday), 1 << 4, "星期四");
    /// <summary>
    /// 星期五
    /// </summary>
    public static readonly DaysOfWeek Friday = new(_comparer, nameof(Friday), 1 << 5, "星期五");
    /// <summary>
    /// 星期六
    /// </summary>
    public static readonly DaysOfWeek Saturday = new(_comparer, nameof(Saturday), 1 << 6, "星期六");
    #endregion
    /// <summary>
    /// 静态实例
    /// </summary>
    public static readonly IFlagEnumerationProvider<DaysOfWeek> Provider = new DaysOfWeekProvider();
    /// <summary>
    /// 星期几枚举提供者
    /// </summary>
    private sealed class DaysOfWeekProvider : FlagEnumerationProvider<DaysOfWeek>
    {
        internal DaysOfWeekProvider()
            : base(
            [Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday]
            , new Dictionary<string, DaysOfWeek>(_comparer)
            {
                { nameof(Unknown), Unknown },
                { nameof(Sunday), Sunday},
                { nameof(Monday), Monday},
                { nameof(Tuesday), Tuesday},
                { nameof(Wednesday), Wednesday},
                { nameof(Thursday),Thursday},
                { nameof(Friday), Friday},
                { nameof(Saturday), Saturday}
            }
            , new Dictionary<long, DaysOfWeek>()
            {
                { Unknown.Original, Unknown },
                { Sunday.Original, Sunday},
                { Monday.Original, Monday},
                { Tuesday.Original, Tuesday},
                { Wednesday.Original, Wednesday},
                { Thursday.Original, Thursday},
                { Friday.Original, Friday},
                { Saturday.Original, Saturday}
            }
        )
        {
        }
        /// <inheritdoc />
        public override DaysOfWeek Empty => Unknown;
        /// <inheritdoc />
        protected override DaysOfWeek CreateFlag(ISet<string> flags, long original, string description = "")
            => new(flags, original, description);
    }
}
~~~

### 3.2 位标记枚举类演示代码
>* 周一到周三员工1排班
>* 周三到周五员工2排班
>* 通过And就可以获取两个员工重叠的值班
>* HasFlag返回true说明当天两人一起值班
>* HasFlag为false说明当天只有一人值班
>* 使用体验和enum差不多,完全可以平替
>* 另外位标记枚举类还封装需要的逻辑或增加字段信息

~~~csharp
var provider = DaysOfWeek.Provider;
// 假设员工1周一到周三值班
var duty1 = provider.Or(DaysOfWeek.Monday, DaysOfWeek.Tuesday, DaysOfWeek.Wednesday);
// 假设员工2周三到周五值班
var duty2 = provider.Or(DaysOfWeek.Wednesday, DaysOfWeek.Thursday, DaysOfWeek.Friday);
/// 判断是否有两个员工共同值班
var overlay = provider.And(duty1, duty2);
Assert.True(overlay.HasFlag(DaysOfWeek.Wednesday));
Assert.False(overlay.HasFlag(DaysOfWeek.Tuesday));
~~~