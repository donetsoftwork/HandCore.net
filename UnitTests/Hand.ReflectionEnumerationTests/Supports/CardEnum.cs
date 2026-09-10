using System.Runtime.Serialization;

namespace Hand.ReflectionEnumerationTests.Supports;

/// <summary>
/// Vip卡类型
/// </summary>
public enum CardEnum
{
    [EnumMember(Value = "银卡")]
    Silver = 1,
    Vip = Silver,
    [EnumMember(Value = "黄金卡")]
    Gold = 2,
    Vip2 = Gold,
    [EnumMember(Value = "金卡")]
    Platinum = 3,
    Vip3 = Platinum,
    [EnumMember(Value = "黑金卡")]
    Black = 4,
    Vip4 = Black,
}