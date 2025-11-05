namespace Hand.Job;

/// <summary>
/// 作业服务接口
/// </summary>
public interface IJobService
{
    /// <summary>
    /// 启动服务
    /// </summary>
    /// <returns></returns>
    bool Start();
    /// <summary>
    /// 停止服务
    /// </summary>
    /// <returns></returns>
    bool Stop();
}
