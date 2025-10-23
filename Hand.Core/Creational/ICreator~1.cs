namespace Hand.Creational;

/// <summary>
/// 工厂接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ICreator<out T>
{
    /// <summary>
    /// 创建
    /// </summary>
    T Create();
}
