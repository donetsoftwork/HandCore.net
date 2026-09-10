using System.ComponentModel;
using System.Runtime.Serialization;

namespace Hand.ReflectionEnumerationTests.Enum2;

/// <summary>
/// Vip卡类型
/// </summary>
public enum CardType
{
    [Description("银卡")]
    Silver = 1,
    Vip = Silver,
    [Description("金卡")]
    Gold = 2,
    Vip2 = Gold,
    [Description("白金卡")]
    Platinum = 3,
    Vip3 = Platinum,
    [Description("黑金卡")]
    [EnumMember(Value = "SVIP")]
    Black = 4,
    Vip4 = Black,
}