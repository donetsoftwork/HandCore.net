namespace Hand.Models;

/// <summary>
/// 实体状态
/// </summary>
public enum EntityStatus
{
    /// <summary>
    /// 无变化
    /// </summary>
    Unchanged = 0,
    /// <summary>
    /// 新实体
    /// </summary>
    New = 1,
    /// <summary>
    /// 已添加
    /// </summary>
    Added = 2,
    /// <summary>
    /// 修改
    /// </summary>
    Modify = 3,
    /// <summary>
    /// 已修改
    /// </summary>
    Modified = 4,
    /// <summary>
    /// 删除
    /// </summary>
    Delete = 5,
    /// <summary>
    /// 已删除
    /// </summary>
    Deleted = 6
}
