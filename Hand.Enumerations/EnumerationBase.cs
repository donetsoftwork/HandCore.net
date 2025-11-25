namespace Hand.Enumerations;

/// <summary>
/// 枚举基类
/// </summary>
/// <param name="code"></param>
/// <param name="value"></param>
/// <param name="display"></param>
public abstract class EnumerationBase(string code, int value, string display)
{
    /// <summary>
    /// 枚举基类
    /// </summary>
    /// <param name="value"></param>
    /// <param name="code"></param>
    public EnumerationBase(int value, string code)
        : this(code, value, code)
    {
    }
    private readonly string _code = code;
    private readonly int _value = value;
    private readonly string _display = display;

    /// <summary>
    /// 编码
    /// </summary>
    public string Code
        => _code;
    /// <summary>
    /// 值
    /// </summary>
    public int Value
        => _value;
    /// <summary>
    /// 展示
    /// </summary>
    public string Display
        => _display;
    /// <summary>
    /// 展示
    /// </summary>
    /// <returns></returns>
    public override string ToString()
        => _display;
    /// <summary>
    /// 比较
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is not EnumerationBase otherValue)
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (!GetType().Equals(obj.GetType()))
        {
            return false;
        }
        return _value.Equals(otherValue.Value);
    }
    /// <summary>
    /// 获取哈希值
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
    /// <summary>
    /// 比较
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(object other)
    {
        return _value.CompareTo(((EnumerationBase)other).Value);
    }
    /// <summary>
    /// 隐式转换为字符串
    /// </summary>
    /// <param name="enumeration"></param>
    public static implicit operator int(EnumerationBase enumeration)
    {
        return enumeration._value;
    }
}
