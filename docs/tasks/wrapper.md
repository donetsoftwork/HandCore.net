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
>* 可以参考以前的文章[异步"伪线程"重构《手搓》线程池,支持任务清退](https://www.cnblogs.com/xiangji/p/19192440)

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
>* 《手搓》线程池是可以高度私人订制的线程池
>* 有谁会不喜欢私人定制自己的线程池呢

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

### 2. 《手搓》线程池支持私人定制任务
#### 2.1 必需实现接口IState\<bool\>
>* 只有一个Status属性告诉线程池你这个任务当前是否有效
>* 如果Status为false线程池不会执行,直接忽略该任务
>* 如果执行中途变成false,线程池会清退该任务

#### 2.2 实现IJobItem或IAsyncJobItem
>* 同步处理实现IJobItem
>* 异步处理实现IAsyncJobItem

#### 2.3 可选实现IExceptionable
>* 实现IExceptionable可以订阅异常处理

#### 2.4 可选实现ICancelable
>* 实现ICancelable可以订阅取消处理
>* 特别要注意,有些取消往往会触发异常
>* 如果要完全处理取消,一般IExceptionable也是要实现的

#### 3. 《手搓》线程池支持私人定制处理器
>* 通过实现接口IQueueProcessor可以定制处理器
>* 定制自己的处理器逻辑可能是一个更好的选择
>* 比如为了性能专门处理异步或同步
>* 或者处理特殊的数据结构和封装自己的异常处理逻辑
>* 示例MyProcessor就是定制处理器
>* MyProcessor紧贴业务,无需委托封装也无需定义IAsyncJobItem对象,直接处理业务对象
>* 内存和性能都能做到极致

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 10 };
var processor = new MyProcessor(_output);
var pool = options.CreateJob(processor);
for (int i = 0; i < 100; i++)
{
    pool.Add("User" + i);
}
await Task.Delay(1000);

/// <summary>
/// 自定义处理器
/// </summary>
/// <param name="output"></param>
class MyProcessor(ITestOutputHelper output)
     : IQueueProcessor<string>
{
    private readonly ITestOutputHelper _output = output;
    /// <inheritdoc />
    public async void Run(IQueue<string> queue, ThreadJobService<string> service, CancellationToken token)
    {
        while (queue.TryDequeue(out var item))
        {
            if (service.Activate(item))
                await RunItemAsync(item, token);
            if (token.IsCancellationRequested)
                break;
        }
        // 用完释放(回收)
        service.Dispose();
    }
    /// <summary>
    /// 执行单个
    /// </summary>
    /// <param name="item"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task RunItemAsync(string item, CancellationToken token)
    {
        _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {item},{DateTime.Now:HH:mm:ss.fff}");
        try
        {
            await Task.Delay(10, token);
        }
        catch { }
    }
}

// Thread8 Hello User1,19:49:05.835
// Thread34 Hello User5,19:49:05.836
// Thread36 Hello User7,19:49:05.836
// Thread35 Hello User6,19:49:05.836
// Thread33 Hello User4,19:49:05.835
// Thread11 Hello User0,19:49:05.835
// Thread31 Hello User2,19:49:05.835
// Thread37 Hello User8,19:49:05.836
// Thread32 Hello User3,19:49:05.835
// Thread38 Hello User9,19:49:05.836
// Thread31 Hello User10,19:49:05.853
// Thread36 Hello User11,19:49:05.853
// Thread37 Hello User12,19:49:05.853
// Thread38 Hello User13,19:49:05.853
// Thread34 Hello User14,19:49:05.853
// Thread8 Hello User15,19:49:05.853
// Thread35 Hello User16,19:49:05.853
// Thread33 Hello User17,19:49:05.853
// Thread11 Hello User19,19:49:05.853
// Thread32 Hello User18,19:49:05.853
// Thread33 Hello User20,19:49:05.868
// Thread35 Hello User21,19:49:05.868
// Thread34 Hello User22,19:49:05.868
// Thread8 Hello User23,19:49:05.868
// Thread34 Hello User24,19:49:05.868
// Thread35 Hello User26,19:49:05.868
// Thread37 Hello User25,19:49:05.868
// Thread8 Hello User28,19:49:05.868
// Thread33 Hello User27,19:49:05.868
// Thread38 Hello User29,19:49:05.868
// Thread38 Hello User30,19:49:05.884
// Thread37 Hello User31,19:49:05.884
// Thread38 Hello User32,19:49:05.884
// Thread8 Hello User34,19:49:05.884
// Thread35 Hello User35,19:49:05.884
// Thread33 Hello User33,19:49:05.884
// Thread38 Hello User36,19:49:05.884
// Thread38 Hello User37,19:49:05.884
// Thread8 Hello User39,19:49:05.884
// Thread34 Hello User38,19:49:05.884
// Thread37 Hello User40,19:49:05.900
// Thread34 Hello User41,19:49:05.900
// Thread37 Hello User43,19:49:05.900
// Thread34 Hello User44,19:49:05.900
// Thread8 Hello User42,19:49:05.900
// Thread34 Hello User45,19:49:05.900
// Thread34 Hello User46,19:49:05.900
// Thread34 Hello User48,19:49:05.900
// Thread37 Hello User47,19:49:05.900
// Thread38 Hello User49,19:49:05.900
// Thread38 Hello User50,19:49:05.916
// Thread37 Hello User51,19:49:05.916
// Thread34 Hello User52,19:49:05.916
// Thread8 Hello User53,19:49:05.916
// Thread35 Hello User54,19:49:05.916
// Thread38 Hello User56,19:49:05.916
// Thread8 Hello User57,19:49:05.916
// Thread34 Hello User58,19:49:05.916
// Thread33 Hello User55,19:49:05.916
// Thread36 Hello User59,19:49:05.916
// Thread36 Hello User60,19:49:05.932
// Thread35 Hello User62,19:49:05.932
// Thread33 Hello User61,19:49:05.932
// Thread34 Hello User63,19:49:05.932
// Thread38 Hello User64,19:49:05.932
// Thread35 Hello User65,19:49:05.932
// Thread38 Hello User67,19:49:05.932
// Thread34 Hello User68,19:49:05.932
// Thread36 Hello User66,19:49:05.932
// Thread8 Hello User69,19:49:05.932
// Thread8 Hello User70,19:49:05.948
// Thread36 Hello User71,19:49:05.948
// Thread8 Hello User75,19:49:05.948
// Thread38 Hello User74,19:49:05.948
// Thread34 Hello User73,19:49:05.948
// Thread36 Hello User72,19:49:05.948
// Thread38 Hello User77,19:49:05.948
// Thread35 Hello User76,19:49:05.948
// Thread36 Hello User78,19:49:05.948
// Thread33 Hello User79,19:49:05.948
// Thread8 Hello User80,19:49:05.964
// Thread36 Hello User82,19:49:05.964
// Thread33 Hello User81,19:49:05.964
// Thread35 Hello User83,19:49:05.964
// Thread8 Hello User86,19:49:05.964
// Thread36 Hello User85,19:49:05.964
// Thread33 Hello User87,19:49:05.964
// Thread8 Hello User88,19:49:05.964
// Thread34 Hello User84,19:49:05.964
// Thread38 Hello User89,19:49:05.964
// Thread38 Hello User90,19:49:05.980
// Thread34 Hello User91,19:49:05.980
// Thread34 Hello User94,19:49:05.980
// Thread8 Hello User93,19:49:05.980
// Thread34 Hello User95,19:49:05.980
// Thread38 Hello User92,19:49:05.980
// Thread38 Hello User97,19:49:05.980
// Thread34 Hello User98,19:49:05.980
// Thread35 Hello User96,19:49:05.980
// Thread33 Hello User99,19:49:05.980
~~~

### 4. ActionProcessor
>* ActionProcessor非常简单,只支持执行Action
>* 正因为它逻辑简单,能获得更好的性能,专治性能强迫症患者
>* 虽然简单,线程池的任务清退和并发控制却一点也不打折
>* 把它是比喻为“小李飞刀”一点都不为过

~~~csharp
 var options = new ReduceOptions { ConcurrencyLevel = 1 };
 var pool = options.CreateJob(ActionProcessor.Instance);
 pool.Add(() => Hello("张三"));
 pool.Add(() => Hello("李四"));
 await Task.Delay(1000);

void Hello(string name)
{
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
    Thread.Sleep(1);
}

// Thread11 Hello 张三,09:57:57.195
// Thread11 Hello 李四,09:57:57.202
~~~


好了,就介绍到这里,更多信息请查看源码库
源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！