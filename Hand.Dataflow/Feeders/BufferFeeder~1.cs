using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hand.Dataflow.Feeders;

/// <summary>
/// 批量进料器
/// </summary>
/// <typeparam name="TData"></typeparam>
/// <param name="size"></param>
public class BufferFeeder<TData>(int size)
    : IFeeder<TData>
{
    private readonly int _size = size;
    private int _count = 0;
    private readonly TData[] _buffer = new TData[size];
    private TaskCompletionSource<IEnumerable<TData>> _source = new();
    /// <summary>
    /// 
    /// </summary>
    public int Count
        => _count;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool Feed(TData data)
    {
        _buffer[_count++] = data;
        if (_count >= _size)
        {
            _source.SetResult(_buffer);
            Task.WhenAny(_source.Task).ConfigureAwait(false);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 结果
    /// </summary>
    public Task<IEnumerable<TData>> Result
        => _source.Task;
}
