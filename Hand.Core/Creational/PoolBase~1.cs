using System.Collections.Concurrent;

namespace Hand.Creational;

/// <summary>
/// 池基类
/// </summary>
/// <typeparam name="TResource"></typeparam>
public abstract class PoolBase<TResource>
    : IPool<TResource>
    where TResource : class
{
    /// <summary>
    /// 池基类
    /// </summary>
    public PoolBase()
        : this(0, Environment.ProcessorCount)
    {
    }
    /// <summary>
    /// 池基类
    /// </summary>
    /// <param name="initialSize"></param>
    /// <param name="maxSize"></param>
    public PoolBase(int initialSize, int maxSize)
    {
        _maxSize = maxSize;
        Initialize(initialSize);
    }
    #region 配置
    /// <summary>
    /// 最大数量
    /// </summary>
    protected readonly int _maxSize;
    /// <summary>
    /// 待用资源池
    /// </summary>
    protected readonly ConcurrentBag<TResource> _pool = [];
    /// <summary>
    /// 已激活资源(正在使用)
    /// </summary>
    protected readonly LinkedList<TResource> _actives = new();
    /// <summary>
    /// 资源池大小
    /// </summary>
    public int PoolCount
        => _pool.Count;
    /// <summary>
    /// 已激活数量
    /// </summary>
    public int ActiveCount 
        => _actives.Count;
    /// <summary>
    /// 最大容量
    /// </summary>
    public int MaxSize 
        => _maxSize;
    #endregion
    /// <inheritdoc />
    public virtual TResource Get()
    {
        if (_pool.TryTake(out var resource))
        {
            lock (_actives)
            {
                _actives.AddLast(resource);
            }
            return resource;
        }
        if (CheckActiveCount())
            return default;
        lock (_actives)
        {
            if (CheckActiveCount())
                return default;
            resource = CreateNew();
            _actives.AddLast(resource);
        }
        return resource;
    }
    /// <inheritdoc />
    public virtual void Return(TResource resource)
    {
        try
        {
            if (Clean(ref resource))
                _pool.Add(resource);
        }
        catch (Exception ex)
        {
#if NET7_0_OR_GREATER
            Console.WriteLine(ex.ToString());
#endif
        }
    }
    /// <summary>
    /// 构造新对象
    /// </summary>
    /// <returns></returns>
    protected abstract TResource CreateNew();
    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="initialSize"></param>
    protected virtual void Initialize(int initialSize) 
    {
        for (var i = 0; i < initialSize; i++)
            _pool.Add(CreateNew());
    }
    /// <summary>
    /// 检查已激活数量
    /// </summary>
    /// <returns></returns>
    private bool CheckActiveCount()
        => _actives.Count >= _maxSize;
    /// <summary>
    /// 检查是否超过最大容量
    /// </summary>
    /// <returns></returns>
    private bool CheckPoolSize()
        => _pool.Count <= _maxSize;
    /// <summary>
    /// 清理对象(及判断对象是否能重入)
    /// </summary>
    /// <param name="resource"></param>
    protected virtual bool Clean(ref TResource resource)
    {
        lock (_actives)
            _actives.Remove(resource);
        return CheckPoolSize();
    }
}
