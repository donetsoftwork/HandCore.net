using Hand.Enumerations;

namespace Hand.EnumerationTests.Partial1;

/// <summary>
/// 源生成器样板示例
/// </summary>
partial class CardType
{
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    public static readonly IEnumerationProvider<CardType> Provider = new CardTypeProvider();
    /// <summary>
    /// Vip枚举提供者
    /// </summary>
    private class CardTypeProvider : EnumerationProvider<CardType>
    {
        internal CardTypeProvider()
            : base(
                  new Dictionary<string, CardType>
                  {
                      { nameof(Silver), Silver},
                      { nameof(Gold), Gold},
                      { nameof(Platinum), Platinum},
                      { nameof(Black), Black},
                      { nameof(Vip) , Vip},
                      { nameof(Vip2) , Vip2},
                      { nameof(Vip3) , Vip3},
                      { nameof(Vip4) , Vip4},
                  }
        )
        {
        }
    }
}
