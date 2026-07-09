namespace Hand.Creational;

/// <summary>
/// 构造器接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICreator<out T>
{
    /// <summary>
    /// 创建
    /// </summary>
    T Create();
}
