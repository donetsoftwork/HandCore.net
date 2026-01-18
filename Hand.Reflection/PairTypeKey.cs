namespace Hand.Reflection;

/// <summary>
/// 类型关联键
/// </summary>
/// <param name="leftType"></param>
/// <param name="rightType"></param>
public readonly struct PairTypeKey(Type leftType, Type rightType)
     : IEquatable<PairTypeKey>
{
    #region 配置
    private readonly Type _leftType = leftType;
    private readonly Type _rightType = rightType;
    /// <summary>
    /// 映射源类型
    /// </summary>
    public Type LeftType
        => _leftType;
    /// <summary>
    /// 映射目标类型
    /// </summary>
    public Type RightType
        => _rightType;
    #endregion
    /// <summary>
    /// HashCode
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
#if NET7_0_OR_GREATER
        => HashCode.Combine(_leftType, _rightType); 
#else
        => _leftType.GetHashCode() ^ _rightType.GetHashCode();
#endif
    #region IEquatable
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(PairTypeKey other)
        => _leftType.Equals(other._leftType) && _rightType.Equals(other._rightType);
    /// <summary>
    /// 判同
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals(object other)
        => other is PairTypeKey key && Equals(key);
    #endregion
    #region CheckNullable
    /// <summary>
    /// 判断可空类型
    /// </summary>
    /// <param name="leftType"></param>
    /// <param name="rightType"></param>
    /// <returns></returns>
    public static bool CheckNullable(ref Type leftType, ref Type rightType)
    {
        bool isNullable = false;
        if (ReflectionType.IsNullable(leftType))
        {
            leftType = Nullable.GetUnderlyingType(leftType);
            isNullable = true;
        }
        if (ReflectionType.IsNullable(rightType))
        {
            rightType = Nullable.GetUnderlyingType(rightType);
            return true;
        }
        return isNullable;
    }
    #endregion
    #region CheckNullCondition
    /// <summary>
    /// 判断是否需要检查null
    /// </summary>
    /// <param name="declareType"></param>
    /// <returns></returns>
    public static bool CheckNullCondition(Type declareType)
    {
        return declareType.IsGenericType || !declareType.IsValueType;
    }
    /// <summary>
    /// 判断是否需要检查null(值类型转非值类型需要检查null)
    /// </summary>
    /// <param name="sourceType"></param>
    /// <param name="destType"></param>
    /// <returns></returns>
    public static bool CheckNullCondition(Type sourceType, Type destType)
    {
        if (destType.IsGenericType)
            return false;
        return destType.IsValueType && CheckNullCondition(sourceType);
    }
    #endregion
    #region CheckType
    /// <summary>
    /// 判断是否兼容值类型
    /// </summary>
    /// <param name="leftType"></param>
    /// <param name="rightType"></param>
    /// <returns></returns>
    public static bool CheckValueType(Type leftType, Type rightType)
    {
        if (leftType == rightType)
            return true;
        if (rightType.IsValueType)
        {
            if (leftType.IsValueType && !rightType.IsGenericType)
                return rightType.IsAssignableFrom(leftType);
            return false;
        }
        if (leftType.IsValueType)
            return false;
        return rightType.IsAssignableFrom(leftType);
    }
    #endregion
}