namespace Hand.Maping;

/// <summary>
/// 识别器接口
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IRecognizer<TKey>
{
    /// <summary>
    /// 识别
    /// </summary>
    /// <typeparam name="TMember"></typeparam>
    /// <param name="members"></param>
    /// <returns></returns>
    IDictionary<TKey, TMember> Recognize<TMember>(IDictionary<TKey, TMember> members);
}
