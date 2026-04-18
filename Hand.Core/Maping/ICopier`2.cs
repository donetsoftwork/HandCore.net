namespace Hand.Maping;

/// <summary>
/// 复制器
/// </summary>
/// <typeparam name="TFrom"></typeparam>
/// <typeparam name="TTo"></typeparam>
public interface ICopier<in TFrom, in TTo>
{
    /// <summary>
    /// 复制
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    void Copy(TFrom from, TTo to);
}