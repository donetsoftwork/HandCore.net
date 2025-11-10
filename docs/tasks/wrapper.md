# 鸡肋的TaskFactory是时候抛弃了
>* TaskFactory调用系统线程池来执行Task
>* 手搓线程池不一定要依赖TaskFactory就能直接执行Task

## 一、TaskFactory的作用
>* 通过TaskFactoryk可以生成Task
>* 并在系统线程池中执行

### 1. TaskFactory.StartNew调用同步方法的Case
>* 以下是Task经典的Case
>* 使用TaskFactory的StartNew异步执行3个耗时1秒的任务
>* 共耗时1秒

~~~csharp
 var sw = Stopwatch.StartNew();
 var task = Task.Factory.StartNew(() => Hello("张三", 1000));
 var task2 = Task.Factory.StartNew(() => Hello("李四", 1000));
 var task3 = Task.Factory.StartNew(() => Hello("王二", 1000));
 await Task.WhenAll(task, task2, task3);
 sw.Stop();
 _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

void Hello(string name, int time = 10)
{
    Thread.Sleep(time);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread10 Hello 张三,16:13:03.553
// Thread31 Hello 王二,16:13:03.553
// Thread11 Hello 李四,16:13:03.553
// Thread17 Total Span :1010.0936
~~~

## 二、Task.Run也能生成并执行Task

### 1. Task.Run调用同步方法的Case
>* Task.Run也可以生成并执行同步方法
>* TaskFactory的StartNew效果差不多
>* 区别在于Task.Run是静态方法
>* TaskFactory.StartNew是实例方法,可以手搓(自定义)
>* 参考笔者博文[重构《手搓》TaskFactory带你更安全的起飞](https://www.cnblogs.com/xiangji/p/19198381)

~~~csharp
var sw = Stopwatch.StartNew();
var task = Task.Run(() => Hello("张三", 1000));
var task2 = Task.Run(() => Hello("李四", 1000));
var task3 = Task.Run(() => Hello("王二", 1000));
await Task.WhenAll(task, task2, task3);
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

// Thread31 Hello 王二,16:22:43.545
// Thread10 Hello 张三,16:22:43.545
// Thread11 Hello 李四,16:22:43.545
// Thread15 Total Span :1014.1269
~~~

### 2. Task.Run还能调用异步方法的Case
>* Task.Run还可以胜任异步处理
>* 不知为啥TaskFactory不支持该功能
>* TaskFactory.FromAsync应该能实现类似功能,但明显用起来不方便

~~~csharp
var sw = Stopwatch.StartNew();
var task = Task.Run(() => HelloAsync("张三", 1000));
var task2 = Task.Run(() => HelloAsync("李四", 1000));
var task3 = Task.Run(() => HelloAsync("王二", 1000));
await Task.WhenAll(task, task2, task3);
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

async Task HelloAsync(string name, int time = 10, CancellationToken token = default)
{
    await Task.Delay(time, token);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} HelloAsync {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread31 HelloAsync 张三,01:35:17.541
// Thread11 HelloAsync 王二,01:35:17.541
// Thread8 HelloAsync 李四,01:35:17.541
// Thread17 Total Span :1008.7947
~~~

## 三、《手搓》TaskFactory能兼顾
### 1. 《手搓》TaskFactory调用同步方法的Case
>* 《手搓》TaskFactory可平替系统Factory调用同步方法
>* 还能实现系统Factory做不到的并发控制

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 3 };
var factory = new ConcurrentTaskFactory(options);
var sw = Stopwatch.StartNew();
var task = factory.StartNew(() => Hello("张三", 1000));
var task2 = factory.StartNew(() => Hello("李四", 1000));
var task3 = factory.StartNew(() => Hello("王二", 1000));
await Task.WhenAll(task, task2, task3);
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

// Thread11 Hello 张三,01:50:15.835
// Thread8 Hello 李四,01:50:15.835
// Thread31 Hello 王二,01:50:15.835
// Thread15 Total Span :1010.3326
~~~

### 2. 《手搓》TaskFactory调用异步方法的Case
>* 《手搓》TaskFactory也能调用异步方法
>* 《手搓》TaskFactory可以平替系统Task.Run调用异步方法

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 3 };
var factory = new ConcurrentTaskFactory(options);
var sw = Stopwatch.StartNew();
var task = factory.StartTask(() => HelloAsync("张三", 1000));
var task2 = factory.StartTask(() => HelloAsync("李四", 1000));
var task3 = factory.StartTask(() => HelloAsync("王二", 1000));
await Task.WhenAll(task, task2, task3);
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

// Thread31 HelloAsync 张三,01:57:06.110
// Thread8 HelloAsync 王二,01:57:06.110
// Thread11 HelloAsync 李四,01:57:06.110
// Thread17 Total Span :1004.2208
~~~

### 3. 《手搓》TaskFactory支持安全双保险
>* 《手搓》TaskFactory支持Token安全保护
>* 《手搓》TaskFactory还支持ItemLife安全保护
>* 系统的Factory和Task.Run只能在调用时判断Token是否取消,安全作用几乎可以忽略
>* 《手搓》TaskFactory也是Factory
>* 既然《手搓》TaskFactory那么强大,为什么还说鸡肋呢?
>* 那是因为这些都是《手搓》线程池提供的技术支持
>* 完全可以直接通过《手搓》线程池实现

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
var factory = new ConcurrentTaskFactory(options);
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
var sw = Stopwatch.StartNew();
var task = factory.StartTask(() => HelloAsync("张三", 2000, tokenSource.Token));
Assert.NotNull(task);
try
{
    await task;
    sw.Stop();
}
catch (Exception ex)
{
    sw.Stop();
    _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
}
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
await Task.Delay(2000);

// Thread17 System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Tasks.ConcurrentTaskFactoryTests.TaskItemLife() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\ConcurrentTaskFactoryTests.cs:line 72
// Thread17 Total Span :1017.4384
~~~

## 四、《手搓》线程池的Task
### 1. 《手搓》线程池直接调用同步方法的Case
>* 通过Processor可以直接生成异步

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 3 };
var processor = new Processor();
var pool = options.CreateJob(processor);
var sw = Stopwatch.StartNew();
var task = processor.StartNew(() => Hello("张三", 1000));
var task2 = processor.StartNew(() => Hello("李四", 1000));
var task3 = processor.StartNew(() => Hello("王二", 1000));
await Task.WhenAll(task, task2, task3);
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

// Thread31 Hello 王二,02:15:07.714
// Thread10 Hello 李四,02:15:07.714
// Thread11 Hello 张三,02:15:07.714
// Thread16 Total Span :1011.4239
~~~

### 2. 《手搓》线程池直接调用异步方法的Case
>* 通过Processor可以直接调用异步方法

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 3 };
var processor = new Processor();
var pool = options.CreateJob(processor);
var sw = Stopwatch.StartNew();
var task = processor.StartTask(() => HelloAsync("张三", 1000, CancellationToken.None));
var task2 = processor.StartTask(() => HelloAsync("李四", 1000, CancellationToken.None));
var task3 = processor.StartTask(() => HelloAsync("王二", 1000, CancellationToken.None));
await Task.WhenAll(task, task2, task3);
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

// Thread8 HelloAsync 李四,02:25:06.458
// Thread11 HelloAsync 张三,02:25:06.458
// Thread31 HelloAsync 王二,02:25:06.458
// Thread17 Total Span :1005.3909
~~~

### 3. 《手搓》线程池双保险执行Task的Case
>* 《手搓》线程池支持双保险执行Task
>* 实际上QueueTaskScheduler(《手搓》TaskFactory核心组件)相当于一个特殊的Processor
>* ConcurrentTaskFactory内部就包含了一个手搓线程池
>* 从某种意义来说,ConcurrentTaskFactory(《手搓》TaskFactory)有点多余
>* 《手搓》线程池几乎能实现《手搓》TaskFactory的所有功能
>* 还能提供其他更多功能
>* 也可以说《手搓》TaskFactory是阉割版的线程池

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var processor = new Processor();
var pool = options.CreateJob(processor);
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
var sw = Stopwatch.StartNew();
var task = processor.StartTask(() => HelloAsync("张三", 2000, tokenSource.Token));
Assert.NotNull(task);
try
{
    await task;
    sw.Stop();
}
catch (Exception ex)
{
    sw.Stop();
    _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
}
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
await Task.Delay(2000);

// Thread17 System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Job.ProcessorTests.HelloAsync(String name, Int32 time, CancellationToken token) in D:\projects\HandCore.net\UnitTests\TaskTests\Job\ProcessorTests.cs:line 207
//    at Hand.Tasks.Internal.TaskFuncState.RunAsync(CancellationToken token) in D:\projects\HandCore.net\Hand.Tasks\Internal\TaskFuncState.cs:line 32
//    at Hand.Job.Processor.Hand.Job.IQueueProcessor<Hand.States.IState<System.Boolean>>.Run(IQueue`1 queue, ThreadJobService`1 service, CancellationToken token) in D:\projects\HandCore.net\Hand.Job\Processor.cs:line 155
//    at TaskTests.Job.ProcessorTests.TaskItemLife() in D:\projects\HandCore.net\UnitTests\TaskTests\Job\ProcessorTests.cs:line 189
// Thread17 Total Span :1008.4003
~~~

## 五、《手搓》线程池与系统线程池有区别吗?
>* 当然有区别,而且区别很大
>* 系统线程池是面对过程的,也就是委托方法,可以带一个object参数
>* 《手搓》线程池是面向对象的

### 1. 《手搓》线程池的组成
>* 其一要处理的对象类型
>* 其二对象集合(队列)
>* 其三是处理器(处理逻辑)
>* 可以支持高度定制

~~~csharp
class ReduceJobService<TItem>
{
    /// <summary>
    /// 队列
    /// </summary>
    public IQueue<TItem> Queue { get; }
    /// <summary>
    /// 处理器
    /// </summary>
    public IQueueProcessor<TItem> Processor { get; }
}
~~~

### 1. 《手搓》线程池默认处理器
>* 这是一个有状态任务处理器
>* 线程池保护可以基于这个状态
>* 其中的状态为bool类型,true为有效,false就表示线程池应该清退它
>* Processor支持同步(IJobItem)和异步(IAsyncJobItem)两种任务
>* 通过实现IExceptionable订阅异常处理
>* 通过实现ICancelable订阅取消处理
>* 为了兼容系统线程池使用习惯,默认提供了基于委托(方法)的处理逻辑
>* 实际《手搓》线程池是面向对象的
>* 通过实现IJobItem或IAsyncJobItem并实现IState<bool>,Processor就能支持了
>* 当然你要再实现IExceptionable和ICancelable也能得到相应的回调
>* 或许定义IQueueProcessor接口,定义自己的处理器逻辑是个更好的选择
>* 专门处理异步或同步
>* 或者处理特殊的数据结构和封装自己的异常处理逻辑
>* 能私人订制的线程池,又有谁不喜欢呢

~~~csharp
class Processor(IQueue<IState<bool>> queue)
    : IQueueProcessor<IState<bool>>;
/// <summary>
/// 状态
/// </summary>
/// <typeparam name="TStatus"></typeparam>
interface IState<TStatus>
{
    /// <summary>
    /// 状态
    /// </summary>
    TStatus Status { get; }
}
interface IExceptionable
{
    /// <summary>
    /// 触发异常回调
    /// </summary>
    /// <param name="exception"></param>
    void OnException(Exception exception);
}
interface ICancelable
{
    /// <summary>
    /// 取消
    /// </summary>
    void OnCancel();
}
~~~

### 2. ActionProcessor
>* 顾名思义,该处理器是执行Action的
>* 不处理状态只同步执行Action
>* 有次特殊需求的可以获得更好的性能

好了,就介绍到这里,更多信息请查看源码库
源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！