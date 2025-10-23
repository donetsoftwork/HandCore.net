namespace Hand.Models;

/// <summary>
/// 空对象
/// </summary>
public readonly struct Empty
{
    private static readonly Empty _value = new();
    /// <summary>
    /// 空值
    /// </summary>
    public static ref readonly Empty Value
        => ref _value;
}
