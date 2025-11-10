# 重构《手搓》TaskFactory带你更安全的起飞
话说上次《手搓》TaskFactory就原地起飞了
网友@舟翅桐和@0611163说飞得还不错,但你能不能拽着点,我恐高
于是有了这次的重构和优化

## 一、彻底的重构和优化
>* 本次《手搓》TaskFactory的重构是直接把依赖的《手搓》线程池一起重构了
>* 可以参看上一篇[异步"伪线程"重构《手搓》线程池,支持任务清退](https://www.cnblogs.com/xiangji/p/19168188)
>* 也就是说安全系数非常高,直接在线程里面处理
>* 大家可以更安全更放心的起飞

## 二、先看一下同步任务取消的Case
### 1. 《手搓》TaskFactory手动取消同步任务
>* 执行同步方法Hello
>* 等待100毫秒后调用Cancel
>* 等待117毫秒,触发异常
>* 之后在另一个线程Hello还是执行了
>* 如果方法已经执行很可能无法真实的取消(除非增加token参数来控制)
>* 但Task已经反馈给调用方本次取消,不会继续等待
>* 当前“线程”也回收了,避免由此可能导致的线程池堵塞
>* 有人可能说,这有什么稀奇的,系统API就支持

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1 };
var factory = new ConcurrentTaskFactory(options);
var tokenSource = new CancellationTokenSource();
var sw = Stopwatch.StartNew();
var task = factory.StartNew(() => Hello("张三", 1000), tokenSource.Token);
Assert.NotNull(task);
await Task.Delay(100);
tokenSource.Cancel();
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
await Task.Delay(1000);

void Hello(string name, int time = 10)
{
    Thread.Sleep(time);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread19 System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Tasks.ConcurrentTaskFactoryTests.ActionCancel() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\ConcurrentTaskFactoryTests.cs:line 90
// Thread19 Total Span :121.4448
// Thread11 Hello 张三,11:41:00.641
~~~

### 2. 系统TaskFactory手动取消任务
>* 系统TaskFactory的流程和前面一样
>* 也是执行同步方法Hello
>* 等待100毫秒后调用Cancel
>* 执行成功,共耗时1秒
>* 是不是很魔幻,居然没取消
>* 系统TaskFactory有Bug?

~~~csharp
var factory = Task.Factory;
var tokenSource = new CancellationTokenSource();
var sw = Stopwatch.StartNew();
var task = factory.StartNew(() => Hello("张三", 1000), tokenSource.Token);
Assert.NotNull(task);
await Task.Delay(100);
tokenSource.Cancel();
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

// Thread10 Hello 张三,11:43:06.118
// Thread19 Total Span :1006.9077
~~~

### 3. 《手搓》TaskFactory设置超时Case
>* 还是同步方法Hello
>* 这次通过CancelAfter设置1秒超时
>* 等待1秒触发异常
>* 也就是说《手搓》TaskFactory超时清退成功

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1 };
var factory = new ConcurrentTaskFactory(options);
var tokenSource = new CancellationTokenSource();
var sw = Stopwatch.StartNew();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
var task = factory.StartNew(() => Hello("张三", 2000), tokenSource.Token);
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
await Task.Delay(1000);

// Thread16 System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Tasks.ConcurrentTaskFactoryTests.ActionTimeout() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\ConcurrentTaskFactoryTests.cs:line 114
// Thread16 Total Span :1075.7213
// Thread11 Hello 张三,15:57:04.440
~~~

### 4. 系统TaskFactory设置超时Case
>* 执行同步方法Hello
>* 通过CancelAfter设置1秒超时
>* 任务执行成功,耗时2秒
>* 再次魔幻的结果,居然又没取消
>* 系统TaskFactory有Bug?

~~~csharp
var factory = Task.Factory;
var tokenSource = new CancellationTokenSource();
var sw = Stopwatch.StartNew();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
var task = factory.StartNew(() => Hello("张三", 2000), tokenSource.Token);
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
sw.Stop();
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");

// Thread10 Hello 张三,10:39:45.556
// Total Span :2017.2007
~~~

### 5. 系统TaskFactory修改再测
>* 把Cancel提前到StartNew之前
>* 这次耗时106毫秒,任务取消
>* 原来系统TaskFactory是要添加任务的时候判断是否取消,而不是执行的时候
>* 意外不意外,惊喜不惊喜
>* 《手搓》TaskFactory是执行时判断是否取消
>* 你满意系统TaskFactory的Cancel逻辑还是《手搓》TaskFactory
>* 很长一段时间笔者都以为系统TaskFactory有Bug
>* 原来是设计如此,终于破案了
>* 如果说《手搓》TaskFactory是系统TaskFactory增强版不为过吧
>* 作为系统TaskFactory的平替也是个不错的选择吧

~~~csharp
var factory = Task.Factory;
var tokenSource = new CancellationTokenSource();
var sw = Stopwatch.StartNew();
tokenSource.Cancel();
var task = factory.StartNew(() => Hello("张三", 1000), tokenSource.Token);
Assert.NotNull(task);
await Task.Delay(100);
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
// System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Tasks.TaskFactoryTests.ActionCancel2() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\TaskFactoryTests.cs:line 69
// Total Span :106.1664
~~~

## 三、异步任务的Case
### 1. 《手搓》TaskFactory并发控制并清退超时任务的Case
>* ConcurrencyLevel设置为1,设置1秒超时
>* 这个CancellationToken参数传给实际执行的异步方法同时也传给线程池
>* 以便线程池及时处理并及时清理已经取消的任务
>* 还起到双保险的作用
>* 如果HelloAsync没有对CancellationToken做出正确的处理
>* 调用者也会及时触发取消的异常
>* 本次执行3个异步任务,每个500毫秒
>* 耗时1秒,两个任务执行成功,并触发取消异常
>* 第3个并未实际执行,这里要归功于token参数起到了拦截的作用
>* 系统TaskFactory没有控制异步和并发的逻辑就不对比
>* 另外需要特别强调《手搓》TaskFactory支持调用异步,但必须是使用StartTask方法
>* 如果使用StartNew方法意味着把异步方法当同步执行
>* 就不能控制并发了,只是简单触发之后在系统线程池里执行而已

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1 };
var factory = new ConcurrentTaskFactory(options);
var tokenSource = new CancellationTokenSource();
var sw = Stopwatch.StartNew();
tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
var token = tokenSource.Token;
var task = factory.StartTask((t) => HelloAsync("张三", 500, t), token);
var task2 = factory.StartTask((t) => HelloAsync("李四", 500, t), token);
var task3 = factory.StartTask((t) => HelloAsync("王二", 500, t), token);
try
{
    await Task.WhenAll(task, task2, task3);
    sw.Stop();
}
catch (Exception ex)
{
    sw.Stop();
    _output.WriteLine(($"Thread{Environment.CurrentManagedThreadId} {ex}"));
}
_output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Total Span :{sw.Elapsed.TotalMilliseconds}");
await Task.Delay(2000);

async Task HelloAsync(string name, int time = 10, CancellationToken token = default)
{
    await Task.Delay(time, token);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} HelloAsync {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread31 HelloAsync 张三,10:30:54.720
// Thread11 HelloAsync 李四,10:30:55.231
// System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Tasks.ConcurrentTaskFactoryTests.TaskCancel0() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\ConcurrentTaskFactoryTests.cs:line 126
// Total Span :1013.774
~~~

### 2. 单并发控制异步任务的Case
>* ConcurrencyLevel设置为1
>* 手搓线程池就可以让它摆出一字长蛇阵
>* 虽然线程Id不只1个,但都是间隔相同时间(误差不超过1毫秒)
>* 并发控制那是相当的稳

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1 };
var factory = new ConcurrentTaskFactory(options);
StartTask(factory);
await Task.Delay(10000);

private void StartTask(ConcurrentTaskFactory factory)
{
    for (int i = 1; i < 100; i++)
    {
        var user = "User" + i;
        factory.StartTask(() => HelloAsync(user));
    }
}
async Task HelloAsync(string name, int time = 10, CancellationToken token = default)
{
    await Task.Delay(time, token);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} HelloAsync {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread11 HelloAsync User1,09:41:51.473
// Thread11 HelloAsync User2,09:41:51.489
// Thread11 HelloAsync User3,09:41:51.505
// Thread10 HelloAsync User4,09:41:51.521
// Thread10 HelloAsync User5,09:41:51.537
// Thread10 HelloAsync User6,09:41:51.553
// Thread10 HelloAsync User7,09:41:51.569
// Thread11 HelloAsync User8,09:41:51.585
// Thread11 HelloAsync User9,09:41:51.600
// Thread11 HelloAsync User10,09:41:51.616
// Thread11 HelloAsync User11,09:41:51.632
// Thread10 HelloAsync User12,09:41:51.648
// Thread10 HelloAsync User13,09:41:51.664
// Thread10 HelloAsync User14,09:41:51.680
// Thread10 HelloAsync User15,09:41:51.696
// Thread11 HelloAsync User16,09:41:51.712
// Thread11 HelloAsync User17,09:41:51.728
// Thread11 HelloAsync User18,09:41:51.744
// Thread11 HelloAsync User19,09:41:51.760
// Thread11 HelloAsync User20,09:41:51.776
// Thread11 HelloAsync User21,09:41:51.792
// Thread11 HelloAsync User22,09:41:51.808
// Thread11 HelloAsync User23,09:41:51.824
// Thread10 HelloAsync User24,09:41:51.840
// Thread10 HelloAsync User25,09:41:51.856
// Thread10 HelloAsync User26,09:41:51.872
// Thread10 HelloAsync User27,09:41:51.888
// Thread10 HelloAsync User28,09:41:51.904
// Thread10 HelloAsync User29,09:41:51.920
// Thread10 HelloAsync User30,09:41:51.936
// Thread10 HelloAsync User31,09:41:51.952
// Thread11 HelloAsync User32,09:41:51.968
// Thread11 HelloAsync User33,09:41:51.984
// Thread11 HelloAsync User34,09:41:52.000
// Thread11 HelloAsync User35,09:41:52.016
// Thread10 HelloAsync User36,09:41:52.032
// Thread10 HelloAsync User37,09:41:52.048
// Thread10 HelloAsync User38,09:41:52.064
// Thread10 HelloAsync User39,09:41:52.080
// Thread10 HelloAsync User40,09:41:52.096
// Thread10 HelloAsync User41,09:41:52.112
// Thread10 HelloAsync User42,09:41:52.128
// Thread10 HelloAsync User43,09:41:52.144
// Thread11 HelloAsync User44,09:41:52.160
// Thread11 HelloAsync User45,09:41:52.176
// Thread11 HelloAsync User46,09:41:52.192
// Thread11 HelloAsync User47,09:41:52.208
// Thread10 HelloAsync User48,09:41:52.224
// Thread10 HelloAsync User49,09:41:52.240
// Thread10 HelloAsync User50,09:41:52.256
// Thread10 HelloAsync User51,09:41:52.272
// Thread10 HelloAsync User52,09:41:52.288
// Thread10 HelloAsync User53,09:41:52.303
// Thread10 HelloAsync User54,09:41:52.319
// Thread10 HelloAsync User55,09:41:52.335
// Thread11 HelloAsync User56,09:41:52.351
// Thread11 HelloAsync User57,09:41:52.367
// Thread11 HelloAsync User58,09:41:52.383
// Thread10 HelloAsync User59,09:41:52.399
// Thread10 HelloAsync User60,09:41:52.415
// Thread10 HelloAsync User61,09:41:52.431
// Thread10 HelloAsync User62,09:41:52.447
// Thread10 HelloAsync User63,09:41:52.463
// Thread11 HelloAsync User64,09:41:52.479
// Thread11 HelloAsync User65,09:41:52.495
// Thread11 HelloAsync User66,09:41:52.511
// Thread11 HelloAsync User67,09:41:52.527
// Thread11 HelloAsync User68,09:41:52.543
// Thread11 HelloAsync User69,09:41:52.559
// Thread11 HelloAsync User70,09:41:52.575
// Thread11 HelloAsync User71,09:41:52.591
// Thread10 HelloAsync User72,09:41:52.607
// Thread10 HelloAsync User73,09:41:52.622
// Thread10 HelloAsync User74,09:41:52.638
// Thread10 HelloAsync User75,09:41:52.654
// Thread11 HelloAsync User76,09:41:52.670
// Thread11 HelloAsync User77,09:41:52.686
// Thread11 HelloAsync User78,09:41:52.702
// Thread11 HelloAsync User79,09:41:52.718
// Thread10 HelloAsync User80,09:41:52.734
// Thread10 HelloAsync User81,09:41:52.750
// Thread10 HelloAsync User82,09:41:52.766
// Thread10 HelloAsync User83,09:41:52.782
// Thread10 HelloAsync User84,09:41:52.798
// Thread10 HelloAsync User85,09:41:52.814
// Thread10 HelloAsync User86,09:41:52.830
// Thread10 HelloAsync User87,09:41:52.846
// Thread11 HelloAsync User88,09:41:52.862
// Thread11 HelloAsync User89,09:41:52.878
// Thread10 HelloAsync User90,09:41:52.894
// Thread10 HelloAsync User91,09:41:52.910
// Thread11 HelloAsync User92,09:41:52.926
// Thread11 HelloAsync User93,09:41:52.942
// Thread11 HelloAsync User94,09:41:52.958
// Thread10 HelloAsync User95,09:41:52.974
// Thread10 HelloAsync User96,09:41:52.990
// Thread10 HelloAsync User97,09:41:53.006
// Thread10 HelloAsync User98,09:41:53.022
// Thread10 HelloAsync User99,09:41:53.038
~~~

### 3. 多并发控制异步任务的Case
>* ConcurrencyLevel设置为10
>* 清晰可见的10个并发
>* 虽然线程数量是在线程池里是从0开始按指数关系(0,1,2,4,8)递增到ConcurrencyLevel配置
>* 但误差不超过1毫秒

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 10 };
var factory = new ConcurrentTaskFactory(options);
StartTask(factory);
await Task.Delay(10000);

// Thread32 HelloAsync User1,09:45:23.848
// Thread34 HelloAsync User5,09:45:23.848
// Thread11 HelloAsync User6,09:45:23.848
// Thread35 HelloAsync User10,09:45:23.848
// Thread33 HelloAsync User2,09:45:23.848
// Thread10 HelloAsync User7,09:45:23.848
// Thread31 HelloAsync User8,09:45:23.848
// Thread36 HelloAsync User3,09:45:23.848
// Thread38 HelloAsync User4,09:45:23.848
// Thread37 HelloAsync User9,09:45:23.848
// Thread38 HelloAsync User19,09:45:23.864
// Thread36 HelloAsync User18,09:45:23.864
// Thread33 HelloAsync User17,09:45:23.864
// Thread34 HelloAsync User16,09:45:23.864
// Thread31 HelloAsync User14,09:45:23.864
// Thread36 HelloAsync User12,09:45:23.864
// Thread38 HelloAsync User11,09:45:23.864
// Thread33 HelloAsync User15,09:45:23.864
// Thread34 HelloAsync User13,09:45:23.864
// Thread35 HelloAsync User20,09:45:23.864
// Thread35 HelloAsync User30,09:45:23.880
// Thread34 HelloAsync User25,09:45:23.880
// Thread34 HelloAsync User29,09:45:23.880
// Thread35 HelloAsync User26,09:45:23.880
// Thread34 HelloAsync User24,09:45:23.880
// Thread34 HelloAsync User23,09:45:23.880
// Thread33 HelloAsync User28,09:45:23.880
// Thread35 HelloAsync User22,09:45:23.880
// Thread36 HelloAsync User27,09:45:23.880
// Thread38 HelloAsync User21,09:45:23.880
// Thread34 HelloAsync User40,09:45:23.896
// Thread38 HelloAsync User39,09:45:23.896
// Thread36 HelloAsync User37,09:45:23.896
// Thread34 HelloAsync User36,09:45:23.896
// Thread34 HelloAsync User32,09:45:23.896
// Thread35 HelloAsync User34,09:45:23.896
// Thread38 HelloAsync User31,09:45:23.896
// Thread36 HelloAsync User35,09:45:23.896
// Thread31 HelloAsync User33,09:45:23.896
// Thread33 HelloAsync User38,09:45:23.896
// Thread33 HelloAsync User50,09:45:23.912
// Thread31 HelloAsync User49,09:45:23.912
// Thread36 HelloAsync User46,09:45:23.912
// Thread38 HelloAsync User48,09:45:23.912
// Thread31 HelloAsync User44,09:45:23.912
// Thread33 HelloAsync User47,09:45:23.912
// Thread31 HelloAsync User42,09:45:23.912
// Thread35 HelloAsync User45,09:45:23.912
// Thread33 HelloAsync User41,09:45:23.912
// Thread34 HelloAsync User43,09:45:23.912
// Thread35 HelloAsync User59,09:45:23.928
// Thread34 HelloAsync User60,09:45:23.928
// Thread31 HelloAsync User54,09:45:23.928
// Thread33 HelloAsync User58,09:45:23.928
// Thread31 HelloAsync User55,09:45:23.928
// Thread35 HelloAsync User56,09:45:23.928
// Thread36 HelloAsync User57,09:45:23.928
// Thread34 HelloAsync User51,09:45:23.928
// Thread33 HelloAsync User52,09:45:23.928
// Thread38 HelloAsync User53,09:45:23.928
// Thread38 HelloAsync User70,09:45:23.944
// Thread36 HelloAsync User69,09:45:23.944
// Thread34 HelloAsync User66,09:45:23.944
// Thread35 HelloAsync User65,09:45:23.944
// Thread38 HelloAsync User67,09:45:23.944
// Thread35 HelloAsync User63,09:45:23.944
// Thread36 HelloAsync User64,09:45:23.944
// Thread34 HelloAsync User61,09:45:23.944
// Thread33 HelloAsync User68,09:45:23.944
// Thread31 HelloAsync User62,09:45:23.944
// Thread35 HelloAsync User80,09:45:23.960
// Thread33 HelloAsync User79,09:45:23.960
// Thread31 HelloAsync User75,09:45:23.960
// Thread33 HelloAsync User78,09:45:23.960
// Thread38 HelloAsync User76,09:45:23.960
// Thread31 HelloAsync User72,09:45:23.960
// Thread31 HelloAsync User71,09:45:23.960
// Thread33 HelloAsync User73,09:45:23.960
// Thread35 HelloAsync User77,09:45:23.960
// Thread34 HelloAsync User74,09:45:23.960
// Thread34 HelloAsync User90,09:45:23.976
// Thread35 HelloAsync User85,09:45:23.976
// Thread33 HelloAsync User87,09:45:23.976
// Thread34 HelloAsync User88,09:45:23.976
// Thread34 HelloAsync User83,09:45:23.976
// Thread34 HelloAsync User81,09:45:23.976
// Thread35 HelloAsync User82,09:45:23.976
// Thread38 HelloAsync User86,09:45:23.976
// Thread33 HelloAsync User84,09:45:23.976
// Thread31 HelloAsync User89,09:45:23.976
// Thread31 HelloAsync User99,09:45:23.992
// Thread38 HelloAsync User98,09:45:23.992
// Thread33 HelloAsync User96,09:45:23.992
// Thread35 HelloAsync User97,09:45:23.992
// Thread34 HelloAsync User94,09:45:23.992
// Thread33 HelloAsync User91,09:45:23.992
// Thread36 HelloAsync User92,09:45:23.992
// Thread31 HelloAsync User93,09:45:23.992
// Thread32 HelloAsync User95,09:45:23.992
~~~

### 4. 网友@舟翅桐的Case也再跑一次
>* 这个Case是他在[上一篇博文](https://www.cnblogs.com/xiangji/p/19168188)的评论里提出的
>* 是个非常有意思的Case
>* 同时这里特别感谢@舟翅桐提出任务清退的概念
>* 3个并发分别执行3个不同异步任务
>* 每个方法有开始和结束日志
>* 可以清晰的看到每3个开始再3个结束
>* 每个任务0.1秒,执行30个任务,3个并发
>* 总耗时1秒,Nice!!!

~~~csharp
var sw = Stopwatch.StartNew();
var factory = new ConcurrentTaskFactory(new TaskFactoryOptions { ConcurrencyLevel = 3 });
List<Task<int>> tasks = new(30);
for (int i = 0; i < 10; i++)
{
    var index = i;
    var t1 = factory.StartTask(() => One(index));
    var t2 = factory.StartTask(() => Two(index));
    var t3 = factory.StartTask(() => Three(index));

    tasks.Add(t1);
    tasks.Add(t2);
    tasks.Add(t3);
}

await Task.WhenAll(tasks);
sw.Stop();
_output.WriteLine("Total Span :" + sw.Elapsed.TotalMilliseconds);

public async Task<int> One(int i, CancellationToken token = default)
{
    token.ThrowIfCancellationRequested();
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} One Start {i}:{DateTime.Now:HH:mm:ss.fff}");
    await Task.Delay(100, token);
    token.ThrowIfCancellationRequested();
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} One End {i}:{DateTime.Now:HH:mm:ss.fff}");
    return 1;
}
public async Task<int> Two(int i, CancellationToken token = default)
{
    token.ThrowIfCancellationRequested();
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Two Start {i}:{DateTime.Now:HH:mm:ss.fff}");
    await Task.Delay(100, token);
    token.ThrowIfCancellationRequested();
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Two End {i}:{DateTime.Now:HH:mm:ss.fff}");
    return 2;
}
public async Task<int> Three(int i, CancellationToken token = default)
{
    token.ThrowIfCancellationRequested();
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Three Start {i}:{DateTime.Now:HH:mm:ss.fff}");
    await Task.Delay(100, token);
    token.ThrowIfCancellationRequested();
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Three End {i}:{DateTime.Now:HH:mm:ss.fff}");
    return 3;
}

// Thread31 Three Start 0:09:50:48.945
// Thread11 One Start 0:09:50:48.945
// Thread8 Two Start 0:09:50:48.945
// Thread31 Two End 0:09:50:49.050
// Thread8 Three End 0:09:50:49.050
// Thread11 One End 0:09:50:49.050
// Thread8 One Start 1:09:50:49.050
// Thread11 Two Start 1:09:50:49.050
// Thread31 Three Start 1:09:50:49.050
// Thread31 Three End 1:09:50:49.162
// Thread8 One End 1:09:50:49.162
// Thread11 Two End 1:09:50:49.162
// Thread11 Two Start 2:09:50:49.162
// Thread8 Three Start 2:09:50:49.162
// Thread31 One Start 2:09:50:49.162
// Thread11 Two End 2:09:50:49.273
// Thread31 One End 2:09:50:49.273
// Thread8 Three End 2:09:50:49.273
// Thread8 Three Start 3:09:50:49.273
// Thread31 Two Start 3:09:50:49.273
// Thread11 One Start 3:09:50:49.273
// Thread11 Two End 3:09:50:49.385
// Thread8 Three End 3:09:50:49.385
// Thread8 One Start 4:09:50:49.385
// Thread11 Two Start 4:09:50:49.385
// Thread32 One End 3:09:50:49.385
// Thread32 Three Start 4:09:50:49.385
// Thread32 One End 4:09:50:49.497
// Thread11 Two End 4:09:50:49.497
// Thread33 Three End 4:09:50:49.497
// Thread11 Two Start 5:09:50:49.497
// Thread32 One Start 5:09:50:49.497
// Thread33 Three Start 5:09:50:49.497
// Thread32 Two End 5:09:50:49.609
// Thread33 Three End 5:09:50:49.609
// Thread11 One End 5:09:50:49.609
// Thread11 One Start 6:09:50:49.609
// Thread32 Three Start 6:09:50:49.609
// Thread33 Two Start 6:09:50:49.609
// Thread11 Two End 6:09:50:49.721
// Thread32 Three End 6:09:50:49.721
// Thread33 One End 6:09:50:49.721
// Thread11 One Start 7:09:50:49.721
// Thread32 Two Start 7:09:50:49.721
// Thread33 Three Start 7:09:50:49.721
// Thread33 Two End 7:09:50:49.833
// Thread32 One End 7:09:50:49.833
// Thread33 One Start 8:09:50:49.833
// Thread32 Two Start 8:09:50:49.833
// Thread8 Three End 7:09:50:49.833
// Thread8 Three Start 8:09:50:49.833
// Thread8 Two End 8:09:50:49.945
// Thread32 Three End 8:09:50:49.945
// Thread32 Two Start 9:09:50:49.945
// Thread8 One Start 9:09:50:49.945
// Thread34 One End 8:09:50:49.945
// Thread34 Three Start 9:09:50:49.945
// Thread34 One End 9:09:50:50.057
// Thread32 Two End 9:09:50:50.057
// Thread8 Three End 9:09:50:50.057
// Total Span :1116.4711
~~~

## 四、全局设置超时的Case
### 1. 同步任务无CancellationToken的Case
>* ItemLife配置为1秒
>* Hello实际需要2秒
>* 调用等待1秒后触发取消异常,与ItemLife配置一致
>* 同步任务清退成功

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
var factory = new ConcurrentTaskFactory(options);
var sw = Stopwatch.StartNew();
var task = factory.StartNew(() => Hello("张三", 2000));
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
await Task.Delay(1000);

// Thread16 System.Threading.Tasks.TaskCanceledException: A task was canceled.
//    at TaskTests.Tasks.ConcurrentTaskFactoryTests.ActionItemLife() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\ConcurrentTaskFactoryTests.cs:line 35
// Thread16 Total Span :1016.3529
// Thread11 Hello 张三,16:09:49.847
~~~

### 2. 异步任务无CancellationToken的Case
>* ItemLife配置为1秒
>* HelloAsync实际需要2秒
>* 调用等待1秒后触发取消异常,与ItemLife配置一致
>* 异步任务也清退成功
>* 由于本次没有给HelloAsync传token,虽然清退了,HelloAsync还是稍后在其他线程执行了
>* 这个Case再次证明token参数的重要性,可以避免不必要的操作

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1, ItemLife = TimeSpan.FromSeconds(1) };
var factory = new ConcurrentTaskFactory(options);
var sw = Stopwatch.StartNew();
var task = factory.StartTask(() => HelloAsync("张三", 2000));
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
//    at TaskTests.Tasks.ConcurrentTaskFactoryTests.TaskItemLife() in D:\projects\HandCore.net\UnitTests\TaskTests\Tasks\ConcurrentTaskFactoryTests.cs:line 65
// Thread17 Total Span :1020.1329
// Thread8 HelloAsync 张三,16:14:04.831
~~~

## 五、揭秘重构《手搓》TaskFactory
### 1. 前1版本《手搓》TaskFactory是基于并发配额控制
>* 启动异步扣除并发配额
>* 注册ContinueWith返还并发配额
>* 参考以前的博文[《手搓》TaskFactory带你安全的起飞](https://www.cnblogs.com/xiangji/p/19168188)

### 2. 重构《手搓》TaskFactory去掉了并发配额控制
>* 去掉并发配额控制
>* 异步线程直接在线程池里面await
>* 线程池原生支持性能更好
>* 线程池大小就是最好的并发控制,原生支持后并发配额完全不需要

## 六、总结
### 1. 《手搓》TaskFactory的作用
>* 《手搓》TaskFactory的主要作用是并发控制
>* .net实现异步多并发很容易,但是并发太大很容易失控
>* 比如数据库连接过高、上游服务被打挂
>* 为此很多人都把.net程序写成“单线程”模式
>* 即使写成“单线程”模式依然避免不了上游资源被打挂的事故
>* 通过《手搓》TaskFactory可以设置为全局模式,来管控某些某些资源的并发控制
>* 同时程序改动极小
>* 无论是同步操作还是异步都可以托管给《手搓》TaskFactory
>* 也就是实现程序的异步多并发的起飞
>* 加上CancellationToken和ItemLife可以保证线程池的不会被堵塞
>* 安全高效的线程池助力.net程序安全的起飞

### 2. 《手搓》线程池的作用
>* 其一支持《手搓》TaskFactory的执行处理
>* 其二支持事件总线的事件派发
>* 拿来做一个毫秒级的任务调度系统应该也没什么问题

### 3. CancellationToken
>* 及时清退任务的关键之一是CancellationToken
>* [前文](https://www.cnblogs.com/xiangji/p/19192440)已经说明,CancellationToken是放飞异步的风筝线
>* 用好CancellationToken,是安全使用异步的关键之一
>* 特别是含await的异步方法
>* 当await成功后很可能已经发生了线程切换,耗时更是不确定的因素
>* 这时很可能要校验CancellationToken判断是否还有必要继续执行
>* 多步骤逻辑耗时,每一步骤前也很可能需要判断是否还有必要继续执行后面的步骤
>* 耗时循环,每次循环也很可能需要判断是否还有必要继续执行
>* 在某些特殊情况,同步方法也可能有必要增加CancellationToken
>* 给实际执行的方法传CancellationToken的同时传给《手搓》TaskFactory或者《手搓》线程池,可以实现双保险

### 4. ItemLife
>* ItemLife是线程池兜底的安全机制
>* 可以有效避免线程池被堵塞
>* 特别是那些还没来得及加CancellationToken的祖传代码


好了,就介绍到这里,更多信息请查看源码库
源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！