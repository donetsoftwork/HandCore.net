# 异步"伪线程"优化《手搓》线程池,支持任务清退
>* 网友@舟翅桐和@0611163建议Task清退机制
>* 多次尝试不能很好实现清退
>* 使用异步"伪线程"推倒重构《手搓》线程池重见光明

## 一、为什么需要Task清退
>* 大家有没有点到过这样的按钮
>* 点完之后转圈圈,页面卡死
>* 多希望尽快弹出一个是否取消的按钮
>* 如果页面的关闭按钮还能用,会毫不犹豫的去点
>* 可想而知长耗时任务如果没有取消功能是多差的用户体验

## 二、再说说Task如何清退
>* Task可以通过CancellationToken实现清退
>* 大部分IO操作都支持CancellationToken
>* 比如EFCore、Dapper、HttpClient等,都支持CancellationToken
>* 异步方法一般都建议包含CancellationToken参数
>* 如果把同步方法比作码奴心爱的玩具
>* 那异步方法就好比是能上天的高级玩具风筝
>* CancellationToken就是那根风筝线
>* 有了CancellationToken,我们的异步方法可以做到收放自如
>* 即使"开弓"也能有"回头箭"

### 1. 通过ThrowIfCancellationRequested清退的Case
>* 本Case计算1000累加,每次计算耗时为当前值的毫秒数
>* 预估耗时500秒
>* 如果用户取消通过ThrowIfCancellationRequested触发异常终止任务
>* 通过CancellationTokenSource构造CancellationToken
>* 可以通过CancelAfter设置超时时间,时间过了自动取消
>* 还可以手动调用Cancel方法取消
>* 本Case设置10秒后超时
>* 发起异步1秒后调用Cancel
>* 结果触发异常
>* 实际耗时1秒
>* 避免了500秒的等待
>* 是一个非常成功的清退

~~~csharp
int result = 0;
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(10));
var sw = Stopwatch.StartNew();
var task = CountAsynWithThrowIfCancellationRequested(1000, tokenSource.Token);
await Task.Delay(1000, CancellationToken.None);
tokenSource.Cancel();
try
{
    result = await task;
}
catch (Exception ex)
{
    _output.WriteLine(ex.ToString());
}
sw.Stop();
_output.WriteLine($"Result: {result} Elapsed:{sw.Elapsed.TotalMilliseconds}");

private static async Task<int> CountAsynWithThrowIfCancellationRequested(int num, CancellationToken token)
{
    var count = 0;
    for (int i = 0; i < num; i++)
    {
        await Task.Delay(i, CancellationToken.None);
        token.ThrowIfCancellationRequested();
        count += i;
    }
    return count;
}

// System.OperationCanceledException: The operation was canceled.
//    at System.Threading.CancellationToken.ThrowOperationCanceledException()
//    at System.Threading.CancellationToken.ThrowIfCancellationRequested()
//    at TaskTests.Tasks.CancellationTokenTests.CountAsynWithThrowIfCancellationRequested(Int32 num, CancellationToken token) in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\CancellationTokenTests.cs:line 51
//    at TaskTests.Tasks.CancellationTokenTests.ThrowIfCancellationRequested() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\CancellationTokenTests.cs:line 22
// Result: 0 Elapsed:1028.8541
~~~

### 2. 通过IsCancellationRequested清退的Case
>* 前面Case有个问题
>* 虽然我们无法忍受500秒拿到最终结果
>* 但是已经等待了1秒了,能不能把这1秒的结果先给我,也算没白等
>* 通过IsCancellationRequested可以实现
>* 还是前面那个Case
>* 这次还不用catch了
>* 实际耗时1秒,拿到中间结果703

~~~csharp
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(10));
var sw = Stopwatch.StartNew();
var task = CountAsynWithIsCancellationRequested(1000, tokenSource.Token);
await Task.Delay(1000, CancellationToken.None);
tokenSource.Cancel();
var result = await task;
sw.Stop();
_output.WriteLine($"Result: {result} Elapsed:{sw.Elapsed.TotalMilliseconds}");

private static async Task<int> CountAsynWithIsCancellationRequested(int num, CancellationToken token)
{
    var count = 0;
    for (int i = 0; i < num; i++) 
    {            
        await Task.Delay(i, CancellationToken.None);
        if (token.IsCancellationRequested)
            break;
        count += i;
    }
    return count;
}

// Result: 703 Elapsed:1056.0416
~~~

### 3. 通过CreateLinkedTokenSource实现复杂的清退规则
>* 如下复杂业务逻辑
>* 执行A、B两个逻辑的结果再调用C逻辑
>* 总共耗时不能超过1秒
>* 其中A、B逻辑不能超过800毫秒,C逻辑不能超过600毫秒
>* 为了更好实现需求,A和B并行节约时间
>* 用CreateLinkedTokenSource实现C操作要同时满足总耗时不超过1秒,C本身不超过600毫秒
>* 综上CancellationToken作用很大,可以设置超时、可以手动触发还可以支持多组条件组合


~~~csharp
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));

var tokenSource1 = new CancellationTokenSource();
tokenSource1.CancelAfter(TimeSpan.FromSeconds(800));
var token1 = tokenSource1.Token;
var taskA = A(500, token1);
var taskB = B(400, token1);
var a = await taskA;
var b = await taskB;

var cancellationToken2 = new CancellationTokenSource();
cancellationToken2.CancelAfter(TimeSpan.FromSeconds(600));
var linked = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token, cancellationToken2.Token);
var taskC = C(400, a, b, linked.Token);
var c = await taskC;
Assert.Equal(3, c);

private static async Task<int> A(int arg, CancellationToken token)
{
    await Task.Delay(arg, token);
    return 1;
}
private static async Task<int> B(int arg, CancellationToken token)
{
    await Task.Delay(arg, token);
    return 2;
}
private static async Task<int> C(int arg, int a, int b, CancellationToken token)
{
    await Task.Delay(arg, token);
    return a + b;
}
~~~

## 三、《手搓》线程池可清退的任务
### 1. 单个异步任务清退Case
>* 通过processor.AddTask添加异步任务并启动线程池
>* 通过tokenSource.Cancel()取消
>* 任务最终并未执行
>* 异步方法最好添加CancellationToken参数以便更精细化处理
>* 特别是逻辑比较复杂的方法和循环处理,减少不必要的等待和无效的CPU计算

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
var processor = new Processor();
var pool = options.CreateJob(processor);
var tokenSource = new CancellationTokenSource();
var token = tokenSource.Token;
var state = processor.AddTask((t) => HelloAsync("张三", t), token);
pool.Start();
tokenSource.Cancel();
await Task.Delay(1000);
Assert.True(state.IsCancel);

async Task HelloAsync(string name, CancellationToken token = default)
{
    await Task.Delay(10, token);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} HelloAsync {name},{DateTime.Now:HH:mm:ss.fff}");
}
~~~

### 2. 单个同步任务清退Case
>* 通过processor.Add添加同步任务并启动线程池
>* 通过tokenSource.Cancel()取消
>* 任务最终并未执行

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
var processor = new Processor();
var pool = options.CreateJob(processor);
var tokenSource = new CancellationTokenSource();
var state = processor.Add(() => Hello("张三"), tokenSource.Token);
pool.Start();
tokenSource.Cancel();
await Task.Delay(1000);
Assert.True(state.IsCancel);

void Hello(string name, int time = 10)
{
    Thread.Sleep(time);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
}
~~~

## 四、《手搓》线程池可清退的线程
### 1. 堵塞线程池的Case
>* ConcurrencyLevel设置为1
>* 这次先添加10个正常的任务
>* 再通过Token添加bug,耗时是其他任务的100倍,并设置了该任务1秒超时
>* 后面又添加了90个任务
>* 从执行结果可以看到,执行前10个任务后确实阻塞了线程池1秒
>* 1秒后线程池恢复继续执行剩下的90个任务
>* 有一个细节,Bug那个任务也执行了,插在第48个任务之后
>* 如果方法已经执行很可能无法真实的取消(除非增加token参数来控制)
>* 但是可以把当前“线程”回收,避免由此可能导致的线程池堵塞
>* 上面的线程笔者特意加了引号,这里说的“线程”实际是一个“线程配额”,来自系统线程池
>* 回收也是一个配额,原方法一旦开始运行只能等他自行结束
>* 而手搓线程要做的是找系统线程池再要一个“配额”
>* 特别提醒不要以为不会堵塞《手搓》线程池就可以随便加超时任务
>* 最终消耗的都是系统线程池的资源
>* 当系统线程池耗完,整个程序就不好了,当然《手搓》线程池也会成为无源之水，无本之木


~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var processor = new Processor();
var pool = options.CreateJob(processor);
for (int i = 0; i < 10; i++)
{
    var user = "User" + i;
    processor.Add(() => Hello(user, 20));
}
var sw = Stopwatch.StartNew();
Hello("Item");
sw.Stop();
_output.WriteLine("Item Span :" + sw.Elapsed.TotalMilliseconds);
var bugToken = new CancellationTokenSource();
bugToken.CancelAfter(TimeSpan.FromMilliseconds(1000));
processor.Add(() => Hello("Bug", 2000), bugToken.Token);
for (int i = 10; i < 100; i++)
{
    var user = "User" + i;
    processor.Add(() => Hello(user, 20));
}
await Task.Delay(5000);

void Hello(string name, int time = 10)
{
    Thread.Sleep(time);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread11 Hello User0,00:50:43.376
// Thread11 Hello User1,00:50:43.408
// Thread11 Hello User2,00:50:43.440
// Thread11 Hello User3,00:50:43.472
// Thread11 Hello User4,00:50:43.504
// Thread11 Hello User5,00:50:43.536
// Thread11 Hello User6,00:50:43.568
// Thread11 Hello User7,00:50:43.600
// Thread11 Hello User8,00:50:43.632
// Thread11 Hello User9,00:50:43.664
// Thread31 Hello User10,00:50:44.447
// Thread31 Hello User11,00:50:44.479
// Thread31 Hello User12,00:50:44.511
// Thread31 Hello User13,00:50:44.543
// Thread31 Hello User14,00:50:44.575
// Thread31 Hello User15,00:50:44.607
// Thread31 Hello User16,00:50:44.639
// Thread31 Hello User17,00:50:44.671
// Thread31 Hello User18,00:50:44.703
// Thread31 Hello User19,00:50:44.735
// Thread31 Hello User20,00:50:44.767
// Thread31 Hello User21,00:50:44.799
// Thread31 Hello User22,00:50:44.831
// Thread31 Hello User23,00:50:44.863
// Thread31 Hello User24,00:50:44.895
// Thread31 Hello User25,00:50:44.927
// Thread31 Hello User26,00:50:44.959
// Thread31 Hello User27,00:50:44.990
// Thread31 Hello User28,00:50:45.022
// Thread31 Hello User29,00:50:45.053
// Thread31 Hello User30,00:50:45.084
// Thread31 Hello User31,00:50:45.116
// Thread31 Hello User32,00:50:45.148
// Thread31 Hello User33,00:50:45.180
// Thread31 Hello User34,00:50:45.212
// Thread31 Hello User35,00:50:45.244
// Thread31 Hello User36,00:50:45.276
// Thread31 Hello User37,00:50:45.308
// Thread31 Hello User38,00:50:45.340
// Thread31 Hello User39,00:50:45.372
// Thread31 Hello User40,00:50:45.404
// Thread31 Hello User41,00:50:45.436
// Thread31 Hello User42,00:50:45.468
// Thread31 Hello User43,00:50:45.500
// Thread31 Hello User44,00:50:45.532
// Thread31 Hello User45,00:50:45.564
// Thread31 Hello User46,00:50:45.596
// Thread31 Hello User47,00:50:45.628
// Thread31 Hello User48,00:50:45.660
// Thread11 Hello Bug,00:50:45.675
// Thread31 Hello User49,00:50:45.691
// Thread32 Hello User50,00:50:45.723
// Thread32 Hello User51,00:50:45.755
// Thread32 Hello User52,00:50:45.786
// Thread32 Hello User53,00:50:45.817
// Thread32 Hello User54,00:50:45.849
// Thread32 Hello User55,00:50:45.881
// Thread32 Hello User56,00:50:45.913
// Thread32 Hello User57,00:50:45.945
// Thread32 Hello User58,00:50:45.977
// Thread32 Hello User59,00:50:46.009
// Thread32 Hello User60,00:50:46.041
// Thread32 Hello User61,00:50:46.073
// Thread32 Hello User62,00:50:46.105
// Thread32 Hello User63,00:50:46.137
// Thread32 Hello User64,00:50:46.169
// Thread32 Hello User65,00:50:46.201
// Thread32 Hello User66,00:50:46.233
// Thread32 Hello User67,00:50:46.265
// Thread32 Hello User68,00:50:46.297
// Thread32 Hello User69,00:50:46.329
// Thread32 Hello User70,00:50:46.361
// Thread32 Hello User71,00:50:46.393
// Thread32 Hello User72,00:50:46.425
// Thread32 Hello User73,00:50:46.457
// Thread32 Hello User74,00:50:46.489
// Thread32 Hello User75,00:50:46.521
// Thread32 Hello User76,00:50:46.552
// Thread32 Hello User77,00:50:46.584
// Thread32 Hello User78,00:50:46.616
// Thread32 Hello User79,00:50:46.648
// Thread32 Hello User80,00:50:46.680
// Thread32 Hello User81,00:50:46.712
// Thread32 Hello User82,00:50:46.744
// Thread32 Hello User83,00:50:46.776
// Thread32 Hello User84,00:50:46.808
// Thread32 Hello User85,00:50:46.840
// Thread32 Hello User86,00:50:46.872
// Thread32 Hello User87,00:50:46.904
// Thread32 Hello User88,00:50:46.936
// Thread32 Hello User89,00:50:46.967
// Thread32 Hello User90,00:50:46.999
// Thread32 Hello User91,00:50:47.031
// Thread32 Hello User92,00:50:47.063
// Thread32 Hello User93,00:50:47.095
// Thread32 Hello User94,00:50:47.127
// Thread32 Hello User95,00:50:47.159
// Thread32 Hello User96,00:50:47.191
// Thread32 Hello User97,00:50:47.222
// Thread32 Hello User98,00:50:47.254
// Thread32 Hello User99,00:50:47.286
~~~

### 2. 没有token参数的任务堵塞线程池怎么办
>* 这次增加了参数ItemLife,设置为1秒
>* 依然是添加10个任务,插入一个Bug,再添加90个任务
>* 这次Bug没有设置token
>* 效果跟上次差不多,线程池阻塞1秒
>* Bug插入40之后
>* 也就是说ItemLife提供了全局保护
>* 再配合前面的token,可以有效提供线程池的可用性
>* 必须强调一下,为了测试博文中设置的线程池都很小
>* 这属于边界测试,实际项目建议线程池尽量设大一点,不会打挂上游就行
>* 如果线上高并发项目也像本测试这样,线程池阻塞1秒是完全无法接受的

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
var processor = new Processor();
var pool = options.CreateJob(processor);
for (int i = 0; i < 10; i++)
{
    var user = "User" + i;
    processor.Add(() => Hello(user, 20));
}
processor.Add(() => Hello("Bug", 2000));
for (int i = 10; i < 100; i++)
{
    var user = "User" + i;
    processor.Add(() => Hello(user, 20));
}
await Task.Delay(5000);

// Thread11 Hello User0,02:41:30.413
// Thread11 Hello User1,02:41:30.445
// Thread11 Hello User2,02:41:30.477
// Thread11 Hello User3,02:41:30.509
// Thread11 Hello User4,02:41:30.540
// Thread11 Hello User5,02:41:30.571
// Thread11 Hello User6,02:41:30.601
// Thread11 Hello User7,02:41:30.632
// Thread11 Hello User8,02:41:30.664
// Thread11 Hello User9,02:41:30.696
// Thread31 Hello User10,02:41:31.746
// Thread31 Hello User11,02:41:31.778
// Thread31 Hello User12,02:41:31.810
// Thread31 Hello User13,02:41:31.842
// Thread31 Hello User14,02:41:31.874
// Thread31 Hello User15,02:41:31.906
// Thread31 Hello User16,02:41:31.938
// Thread31 Hello User17,02:41:31.970
// Thread31 Hello User18,02:41:32.002
// Thread31 Hello User19,02:41:32.034
// Thread31 Hello User20,02:41:32.066
// Thread31 Hello User21,02:41:32.098
// Thread31 Hello User22,02:41:32.130
// Thread31 Hello User23,02:41:32.162
// Thread31 Hello User24,02:41:32.194
// Thread31 Hello User25,02:41:32.226
// Thread31 Hello User26,02:41:32.258
// Thread31 Hello User27,02:41:32.289
// Thread31 Hello User28,02:41:32.321
// Thread31 Hello User29,02:41:32.353
// Thread31 Hello User30,02:41:32.385
// Thread31 Hello User31,02:41:32.417
// Thread31 Hello User32,02:41:32.449
// Thread31 Hello User33,02:41:32.481
// Thread31 Hello User34,02:41:32.513
// Thread31 Hello User35,02:41:32.545
// Thread31 Hello User36,02:41:32.577
// Thread31 Hello User37,02:41:32.609
// Thread31 Hello User38,02:41:32.641
// Thread31 Hello User39,02:41:32.673
// Thread31 Hello User40,02:41:32.705
// Thread11 Hello Bug,02:41:32.705
// Thread31 Hello User41,02:41:32.737
// Thread8 Hello User42,02:41:32.769
// Thread8 Hello User43,02:41:32.801
// Thread8 Hello User44,02:41:32.833
// Thread8 Hello User45,02:41:32.865
// Thread8 Hello User46,02:41:32.897
// Thread8 Hello User47,02:41:32.929
// Thread8 Hello User48,02:41:32.961
// Thread8 Hello User49,02:41:32.993
// Thread8 Hello User50,02:41:33.025
// Thread8 Hello User51,02:41:33.057
// Thread8 Hello User52,02:41:33.089
// Thread8 Hello User53,02:41:33.121
// Thread8 Hello User54,02:41:33.153
// Thread8 Hello User55,02:41:33.185
// Thread8 Hello User56,02:41:33.217
// Thread8 Hello User57,02:41:33.249
// Thread8 Hello User58,02:41:33.281
// Thread8 Hello User59,02:41:33.313
// Thread8 Hello User60,02:41:33.345
// Thread8 Hello User61,02:41:33.377
// Thread8 Hello User62,02:41:33.409
// Thread8 Hello User63,02:41:33.441
// Thread8 Hello User64,02:41:33.473
// Thread8 Hello User65,02:41:33.505
// Thread8 Hello User66,02:41:33.537
// Thread8 Hello User67,02:41:33.568
// Thread8 Hello User68,02:41:33.600
// Thread8 Hello User69,02:41:33.632
// Thread8 Hello User70,02:41:33.664
// Thread8 Hello User71,02:41:33.696
// Thread8 Hello User72,02:41:33.728
// Thread8 Hello User73,02:41:33.759
// Thread8 Hello User74,02:41:33.791
// Thread8 Hello User75,02:41:33.823
// Thread8 Hello User76,02:41:33.855
// Thread8 Hello User77,02:41:33.887
// Thread8 Hello User78,02:41:33.919
// Thread8 Hello User79,02:41:33.951
// Thread8 Hello User80,02:41:33.983
// Thread8 Hello User81,02:41:34.015
// Thread8 Hello User82,02:41:34.047
// Thread8 Hello User83,02:41:34.079
// Thread8 Hello User84,02:41:34.111
// Thread8 Hello User85,02:41:34.143
// Thread8 Hello User86,02:41:34.174
// Thread8 Hello User87,02:41:34.206
// Thread8 Hello User88,02:41:34.238
// Thread8 Hello User89,02:41:34.270
// Thread8 Hello User90,02:41:34.302
// Thread8 Hello User91,02:41:34.333
// Thread8 Hello User92,02:41:34.365
// Thread8 Hello User93,02:41:34.397
// Thread8 Hello User94,02:41:34.428
// Thread8 Hello User95,02:41:34.460
// Thread8 Hello User96,02:41:34.491
// Thread8 Hello User97,02:41:34.523
// Thread8 Hello User98,02:41:34.555
// Thread8 Hello User99,02:41:34.587
~~~

## 五、追踪《手搓》线程池任务状态
### 1. 追踪同步任务状态的Case
>* 添加Action任务会返回一个state
>* 任务尚未执行IsSuccess为false
>* 任务执行成功IsSuccess为true
>* 另外state还有属性IsCancel,为true时表示任务已经取消
>* Exception属性表示任务执行过程总触发的异常
>* 《手搓》线程池以上特性是不是比系统线程池要方便不少
>* 另外请大家放心,任务状态信息通过回调赋值,对性能几乎没有影响

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
var processor = new Processor();
var pool = options.CreateJob(processor);
var state = processor.Add(() => Hello("张三"));
Assert.False(state.IsSuccess);
pool.Start();
await Task.Delay(1000);
Assert.True(state.IsSuccess);
~~~

~~~csharp
/// <summary>
/// 任务状态
/// </summary>
public interface IJobState
{
    /// <summary>
    /// 是否执行成功
    /// </summary>
    bool IsSuccess { get; }
    /// <summary>
    /// 是否执行失败
    /// </summary>
    bool IsFail { get; }
    /// <summary>
    /// 是否取消
    /// </summary>
    bool IsCancel { get; }
    /// <summary>
    /// 异常
    /// </summary>
    public Exception Exception { get; }
}
~~~

### 2. 只执行不追踪任务状态可以吗
>* 当然可以
>* 《手搓》线程池提供了一个简单的处理器ActionProcessor,专治性能强迫症患者
>* ActionProcessor.Instance是默认实例,只有执行逻辑,多个线程池可以共用
>* ActionProcessor和pool的Add方法是void类型
>* ActionProcessor只执行不回调任务状态
>* ActionProcessor有个缺点(也可能是优点),只支持同步任务
>* 另外ActionProcessor不支持token设置单个任务取消
>* ItemLife全局保护还是支持的

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var pool = options.CreateJob(ActionProcessor.Instance);
pool.Add(() => Hello("张三"));
pool.Add(() => Hello("李四"));

// Thread11 Hello 张三,03:09:42.222
// Thread11 Hello 李四,03:09:42.241
~~~

### 3. 追踪异步任务状态的Case
>* 添加异步任务也会返回一个state
>* 与同步任务一样

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
var processor = new Processor();
var pool = options.CreateJob(processor);
var state = processor.AddTask(() => HelloAsync("张三"));
Assert.False(state.IsSuccess);
pool.Start();
await Task.Delay(1000);
Assert.True(state.IsSuccess);
~~~

## 六、获取《手搓》线程池任务执行结果
### 1. 获取同步任务执行结果的Case
>* 添加Func任务会返回一个result
>* result类型继承前面的IJobState,并多一个Result属性

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
var processor = new Processor();
var pool = options.CreateJob(processor);
var result = processor.Add(() => Count(3));
Assert.False(result.IsSuccess);
pool.Start();
await Task.Delay(1000);
Assert.True(result.IsSuccess);
var count = result.Result;
Assert.Equal(6, count);

static int Count(int num)
{
    int result = 0;
    for (int i = 1; i <= num; i++)
        result += i;
    return result;
}

/// <summary>
/// 任务执行结果
/// </summary>
/// <typeparam name="TResult"></typeparam>
public interface IJobResult<out TResult>
    : IJobState
{
    /// <summary>
    /// 结果
    /// </summary>
    TResult Result { get; }
}
~~~

### 2. 获取异步任务执行结果的Case
>* 添加Func异步任务也会返回一个result
>* 当然这个result不能代替Task,不能通过await等到结果完成直接使用
>* 这些效果还是要靠手搓TaskFactory来实现
>* 手搓TaskFactory是基于手搓线程池实现的,这次手搓线程池大范围重构
>* 手搓TaskFactory也是重构了,抽空笔者再补一篇手搓TaskFactory重构的文章

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1, AutoStart = false };
var processor = new Processor();
var pool = options.CreateJob(processor);
var tokenSource = new CancellationTokenSource();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
var result = processor.AddTask((t) => CountAsync(3, t), tokenSource.Token);
Assert.False(result.IsSuccess);
pool.Start();
await Task.Delay(1000);
Assert.True(result.IsSuccess);
var count = result.Result;
Assert.Equal(6, count);

static async Task<int> CountAsync(int num, CancellationToken token = default)
{
    int result = 0;
    for (int i = 1; i <= num; i++)
    {
        await Task.Delay(1, token);
        result += i;
    }    
    return result;
}
~~~

## 七、揭秘手搓线程池重构
### 1. 重构后的手搓线程池
>* 手搓线程池还是由"主线程"和真实线程池构成
>* 区别在于"主线程"的职责的发生了变化
>* 当然"线程"也发生了很大的变化,有真实线程变为"伪线程"

### 2. "主线程"的变化
>* "主线程"不再执行任务,考虑到任务可能阻塞线程
>* 如果先阻塞了主线程,继而其他线程都执行完回收后,再突发任务,会导致"饿死"线程池的不良后果
>* 就是任务堆积,线程池没满但就是没线程在执行
>* 主线程就干3件事
>* 其一就是检查有无线程被阻塞,对被阻塞的线程进行回收
>* 其二是否有任务需要执行,如果有任务就激活一个线程
>* 其三就是休眠一段时间,通过ReduceTime配置,默认50毫秒

### 3. 真实线程变为"伪线程"
>* 由于需要支持异步,如果用真实线程await异步操作,那就是浪费一个线程
>* 所以重构为从系统线程池"申请"线程"配额",await的时候线程还给系统,系统可以另行安排
>* await完成线程再次激活,当然不见得还是前面那个线程,所以变成了"伪线程",也可以说是一个线程"配额"

### 4. 线程增加状态
>* 增加了LastTime属性,用于监控线程是否被堵塞
>* 增加了LastItem属性,用于监控当前执行任务状态是否正常(是否被取消)

好了,就介绍到这里,更多信息请查看源码库
源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！
