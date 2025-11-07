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
>* 并发控制是相当的稳

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 1 };
var factory = new ConcurrentTaskFactory(options);
Start(factory);
await Task.Delay(10000);

private void Start(TaskFactory factory)
{
    for (int i = 1; i < 10; i++)
    {
        for (int j = 1; j < 10; j++)
        {
            int a = i, b = j;
            factory.StartNew(() => Multiply(a, b));
        }
    }
}
int Multiply(int a, int b)
{
    var result = a * b;
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} {a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
    Thread.Sleep(100);
    return result;
}

// Thread11 1 x 1 = 1,08:55:05.898
// Thread11 1 x 2 = 2,08:55:06.005
// Thread11 1 x 3 = 3,08:55:06.117
// Thread11 1 x 4 = 4,08:55:06.229
// Thread11 1 x 5 = 5,08:55:06.341
// Thread11 1 x 6 = 6,08:55:06.453
// Thread11 1 x 7 = 7,08:55:06.565
// Thread11 1 x 8 = 8,08:55:06.677
// Thread11 1 x 9 = 9,08:55:06.789
// Thread11 2 x 1 = 2,08:55:06.900
// Thread11 2 x 2 = 4,08:55:07.011
// Thread11 2 x 3 = 6,08:55:07.123
// Thread11 2 x 4 = 8,08:55:07.235
// Thread11 2 x 5 = 10,08:55:07.347
// Thread11 2 x 6 = 12,08:55:07.459
// Thread11 2 x 7 = 14,08:55:07.571
// Thread11 2 x 8 = 16,08:55:07.683
// Thread11 2 x 9 = 18,08:55:07.795
// Thread11 3 x 1 = 3,08:55:07.906
// Thread11 3 x 2 = 6,08:55:08.017
// Thread11 3 x 3 = 9,08:55:08.129
// Thread11 3 x 4 = 12,08:55:08.240
// Thread11 3 x 5 = 15,08:55:08.351
// Thread11 3 x 6 = 18,08:55:08.463
// Thread11 3 x 7 = 21,08:55:08.575
// Thread11 3 x 8 = 24,08:55:08.687
// Thread11 3 x 9 = 27,08:55:08.799
// Thread11 4 x 1 = 4,08:55:08.910
// Thread11 4 x 2 = 8,08:55:09.022
// Thread11 4 x 3 = 12,08:55:09.134
// Thread11 4 x 4 = 16,08:55:09.246
// Thread11 4 x 5 = 20,08:55:09.358
// Thread11 4 x 6 = 24,08:55:09.469
// Thread11 4 x 7 = 28,08:55:09.581
// Thread11 4 x 8 = 32,08:55:09.693
// Thread11 4 x 9 = 36,08:55:09.805
// Thread11 5 x 1 = 5,08:55:09.917
// Thread11 5 x 2 = 10,08:55:10.029
// Thread11 5 x 3 = 15,08:55:10.140
// Thread11 5 x 4 = 20,08:55:10.252
// Thread11 5 x 5 = 25,08:55:10.364
// Thread11 5 x 6 = 30,08:55:10.475
// Thread11 5 x 7 = 35,08:55:10.587
// Thread11 5 x 8 = 40,08:55:10.699
// Thread11 5 x 9 = 45,08:55:10.810
// Thread11 6 x 1 = 6,08:55:10.922
// Thread11 6 x 2 = 12,08:55:11.034
// Thread11 6 x 3 = 18,08:55:11.146
// Thread11 6 x 4 = 24,08:55:11.257
// Thread11 6 x 5 = 30,08:55:11.369
// Thread11 6 x 6 = 36,08:55:11.481
// Thread11 6 x 7 = 42,08:55:11.592
// Thread11 6 x 8 = 48,08:55:11.704
// Thread11 6 x 9 = 54,08:55:11.816
// Thread11 7 x 1 = 7,08:55:11.928
// Thread11 7 x 2 = 14,08:55:12.038
// Thread11 7 x 3 = 21,08:55:12.150
// Thread11 7 x 4 = 28,08:55:12.262
// Thread11 7 x 5 = 35,08:55:12.374
// Thread11 7 x 6 = 42,08:55:12.486
// Thread11 7 x 7 = 49,08:55:12.598
// Thread11 7 x 8 = 56,08:55:12.710
// Thread11 7 x 9 = 63,08:55:12.822
// Thread11 8 x 1 = 8,08:55:12.934
// Thread11 8 x 2 = 16,08:55:13.046
// Thread11 8 x 3 = 24,08:55:13.158
// Thread11 8 x 4 = 32,08:55:13.270
// Thread11 8 x 5 = 40,08:55:13.382
// Thread11 8 x 6 = 48,08:55:13.494
// Thread11 8 x 7 = 56,08:55:13.606
// Thread11 8 x 8 = 64,08:55:13.718
// Thread11 8 x 9 = 72,08:55:13.830
// Thread11 9 x 1 = 9,08:55:13.942
// Thread11 9 x 2 = 18,08:55:14.054
// Thread11 9 x 3 = 27,08:55:14.166
// Thread11 9 x 4 = 36,08:55:14.278
// Thread11 9 x 5 = 45,08:55:14.390
// Thread11 9 x 6 = 54,08:55:14.502
// Thread11 9 x 7 = 63,08:55:14.614
// Thread11 9 x 8 = 72,08:55:14.725
// Thread11 9 x 9 = 81,08:55:14.836
~~~

### 2. 多并发控制异步任务的Case
>* ConcurrencyLevel设置为10
>* 清晰可见的10个并发
>* 虽然线程数量是在线程池里是从0开始按指数关系(0,1,2,4,8)递增到ConcurrencyLevel配置
>* 但误差不超过1毫秒

~~~csharp
var options = new TaskFactoryOptions { ConcurrencyLevel = 10 };
var factory = new ConcurrentTaskFactory(options);
Start(factory);
await Task.Delay(10000);

// Thread10 1 x 2 = 2,09:05:14.810
// Thread35 1 x 7 = 7,09:05:14.810
// Thread32 1 x 4 = 4,09:05:14.810
// Thread38 2 x 1 = 2,09:05:14.810
// Thread33 1 x 5 = 5,09:05:14.810
// Thread11 1 x 1 = 1,09:05:14.810
// Thread36 1 x 8 = 8,09:05:14.810
// Thread37 1 x 9 = 9,09:05:14.810
// Thread34 1 x 6 = 6,09:05:14.810
// Thread31 1 x 3 = 3,09:05:14.810
// Thread34 2 x 4 = 8,09:05:14.914
// Thread35 3 x 1 = 3,09:05:14.914
// Thread37 2 x 8 = 16,09:05:14.914
// Thread38 2 x 7 = 14,09:05:14.914
// Thread33 2 x 5 = 10,09:05:14.914
// Thread10 2 x 3 = 6,09:05:14.914
// Thread11 2 x 6 = 12,09:05:14.914
// Thread32 3 x 2 = 6,09:05:14.914
// Thread36 2 x 9 = 18,09:05:14.914
// Thread31 2 x 2 = 4,09:05:14.914
// Thread11 3 x 3 = 9,09:05:15.026
// Thread10 3 x 5 = 15,09:05:15.026
// Thread31 3 x 4 = 12,09:05:15.026
// Thread36 4 x 1 = 4,09:05:15.026
// Thread34 3 x 9 = 27,09:05:15.026
// Thread35 4 x 3 = 12,09:05:15.026
// Thread38 3 x 8 = 24,09:05:15.026
// Thread32 4 x 2 = 8,09:05:15.026
// Thread33 3 x 7 = 21,09:05:15.026
// Thread37 3 x 6 = 18,09:05:15.026
// Thread33 4 x 5 = 20,09:05:15.138
// Thread11 4 x 9 = 36,09:05:15.138
// Thread31 5 x 2 = 10,09:05:15.138
// Thread37 4 x 4 = 16,09:05:15.138
// Thread35 5 x 4 = 20,09:05:15.138
// Thread38 4 x 8 = 32,09:05:15.138
// Thread10 5 x 3 = 15,09:05:15.138
// Thread32 4 x 7 = 28,09:05:15.138
// Thread34 4 x 6 = 24,09:05:15.138
// Thread36 5 x 1 = 5,09:05:15.138
// Thread32 6 x 1 = 6,09:05:15.250
// Thread36 5 x 6 = 30,09:05:15.250
// Thread11 5 x 8 = 40,09:05:15.250
// Thread35 5 x 9 = 45,09:05:15.250
// Thread38 6 x 4 = 24,09:05:15.250
// Thread33 6 x 5 = 30,09:05:15.250
// Thread10 5 x 5 = 25,09:05:15.250
// Thread34 6 x 3 = 18,09:05:15.250
// Thread31 6 x 2 = 12,09:05:15.250
// Thread37 5 x 7 = 35,09:05:15.250
// Thread35 7 x 1 = 7,09:05:15.362
// Thread10 7 x 4 = 28,09:05:15.362
// Thread37 7 x 3 = 21,09:05:15.362
// Thread33 6 x 6 = 36,09:05:15.362
// Thread34 7 x 5 = 35,09:05:15.362
// Thread36 7 x 2 = 14,09:05:15.362
// Thread38 7 x 6 = 42,09:05:15.362
// Thread31 6 x 8 = 48,09:05:15.362
// Thread32 6 x 9 = 54,09:05:15.362
// Thread11 6 x 7 = 42,09:05:15.362
// Thread11 7 x 9 = 63,09:05:15.474
// Thread35 8 x 2 = 16,09:05:15.474
// Thread36 8 x 1 = 8,09:05:15.474
// Thread10 7 x 7 = 49,09:05:15.474
// Thread32 8 x 3 = 24,09:05:15.474
// Thread31 8 x 6 = 48,09:05:15.474
// Thread37 8 x 5 = 40,09:05:15.474
// Thread34 8 x 7 = 56,09:05:15.474
// Thread38 7 x 8 = 56,09:05:15.474
// Thread33 8 x 4 = 32,09:05:15.474
// Thread34 8 x 9 = 72,09:05:15.586
// Thread37 9 x 3 = 27,09:05:15.586
// Thread33 9 x 2 = 18,09:05:15.586
// Thread35 9 x 8 = 72,09:05:15.586
// Thread32 9 x 1 = 9,09:05:15.586
// Thread31 9 x 5 = 45,09:05:15.586
// Thread11 8 x 8 = 64,09:05:15.586
// Thread10 9 x 6 = 54,09:05:15.586
// Thread38 9 x 7 = 63,09:05:15.586
// Thread36 9 x 4 = 36,09:05:15.586
// Thread11 9 x 9 = 81,09:05:15.698
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