# 《手搓》线程池优化的追求
《手搓》线程池实现了完美的指数递进关系
异步并发测试一发入魂,开局即是高潮带来了困惑
沉着思考后连夜优化

## 一、先回顾一下以前《手搓》线程池Case
>* ConcurrencyLevel设置为10
>* 并发实现了完美的指数递进关系
>* 当时内心还是得到了很大的满足
>* 第一批01:58:55.832是1个并发
>* 第二批01:58:55.944是2个并发
>* 第三批01:58:56.054是4个并发
>* 第四批01:58:56.165是8个并发
>* 01:58:56.276及以后一段时间达到并发上限10个
>* 参考笔者博文[《手搓》线程池](https://www.cnblogs.com/xiangji/p/19165106)

```csharp
var options = new ReduceOptions { ConcurrencyLevel = 10 };
var scheduler = new ConcurrentTaskScheduler(options);
var factory = new TaskFactory(scheduler);
var jobService = new ConcurrentJobService(scheduler, options);
jobService.Start();
Start(factory);

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
public int Multiply(int a, int b)
{
    var result = a * b;
    _output.WriteLine($"{a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
    Thread.Sleep(100);
    return result;
}

// 1 x 1 = 1,01:58:55.832
// 1 x 2 = 2,01:58:55.944
// 1 x 3 = 3,01:58:55.944
// 1 x 4 = 4,01:58:56.054
// 1 x 5 = 5,01:58:56.054
// 1 x 6 = 6,01:58:56.054
// 1 x 7 = 7,01:58:56.054
// 1 x 8 = 8,01:58:56.165
// 1 x 9 = 9,01:58:56.165
// 2 x 1 = 2,01:58:56.165
// 2 x 3 = 6,01:58:56.165
// 2 x 2 = 4,01:58:56.165
// 2 x 4 = 8,01:58:56.165
// 2 x 5 = 10,01:58:56.165
// 2 x 6 = 12,01:58:56.165
// 2 x 8 = 16,01:58:56.276
// 2 x 9 = 18,01:58:56.276
// 3 x 2 = 6,01:58:56.276
// 2 x 7 = 14,01:58:56.276
// 3 x 3 = 9,01:58:56.276
// 3 x 1 = 3,01:58:56.276
// 3 x 4 = 12,01:58:56.276
// 3 x 5 = 15,01:58:56.276
// 3 x 6 = 18,01:58:56.277
// 3 x 7 = 21,01:58:56.277
// 4 x 1 = 4,01:58:56.388
// 3 x 8 = 24,01:58:56.388
// 3 x 9 = 27,01:58:56.388
// 4 x 2 = 8,01:58:56.388
// 4 x 6 = 24,01:58:56.388
// 4 x 3 = 12,01:58:56.388
// 4 x 5 = 20,01:58:56.388
// 4 x 8 = 32,01:58:56.388
// 4 x 4 = 16,01:58:56.388
// 4 x 7 = 28,01:58:56.388
// 5 x 8 = 40,01:58:56.500
// 5 x 4 = 20,01:58:56.500
// 5 x 9 = 45,01:58:56.500
// 5 x 2 = 10,01:58:56.500
// 5 x 7 = 35,01:58:56.500
// 5 x 6 = 30,01:58:56.500
// 4 x 9 = 36,01:58:56.500
// 5 x 5 = 25,01:58:56.500
// 5 x 1 = 5,01:58:56.500
// 5 x 3 = 15,01:58:56.500
// 6 x 5 = 30,01:58:56.612
// 6 x 2 = 12,01:58:56.612
// 6 x 1 = 6,01:58:56.612
// 6 x 3 = 18,01:58:56.612
// 6 x 6 = 36,01:58:56.612
// 6 x 7 = 42,01:58:56.612
// 6 x 4 = 24,01:58:56.612
// 6 x 8 = 48,01:58:56.612
// 6 x 9 = 54,01:58:56.612
// 7 x 1 = 7,01:58:56.612
// 7 x 2 = 14,01:58:56.724
// 8 x 1 = 8,01:58:56.724
// 7 x 4 = 28,01:58:56.724
// 7 x 7 = 49,01:58:56.724
// 7 x 5 = 35,01:58:56.724
// 8 x 2 = 16,01:58:56.724
// 7 x 8 = 56,01:58:56.724
// 7 x 6 = 42,01:58:56.724
// 7 x 9 = 63,01:58:56.724
// 7 x 3 = 21,01:58:56.724
// 8 x 5 = 40,01:58:56.836
// 8 x 8 = 64,01:58:56.836
// 9 x 1 = 9,01:58:56.836
// 9 x 2 = 18,01:58:56.836
// 8 x 7 = 56,01:58:56.836
// 8 x 9 = 72,01:58:56.836
// 8 x 3 = 24,01:58:56.836
// 8 x 6 = 48,01:58:56.836
// 9 x 3 = 27,01:58:56.836
// 8 x 4 = 32,01:58:56.836
// 9 x 5 = 45,01:58:56.948
// 9 x 7 = 63,01:58:56.948
// 9 x 4 = 36,01:58:56.948
// 9 x 6 = 54,01:58:56.948
// 9 x 9 = 81,01:58:56.948
// 9 x 8 = 72,01:58:56.948
```

## 二、短暂的快乐被异步并发测试给打破了
>* ConcurrencyLevel设置为4
>* 一开始就是4个清晰可见的并发
>* 一发入魂,开局即是高潮
>* 第一感觉是bug
>* 第二感觉是偶发事件
>* 笔者多次测试,并反复Review代码,都没发现其中的蹊跷
>* 参考笔者博文[《手搓》TaskFactory带你安全的起飞](https://www.cnblogs.com/xiangji/p/19168188)

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 4 };
var factory = new ConcurrentTaskFactory(options);
_output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
Stopwatch sw = Stopwatch.StartNew();
List<Task<Product>> tasks = new(100);
for (int i = 0; i < 100; i++)
{
    var id = i;
    var task = factory.StartTask(() => GetProductAsync(id));
    tasks.Add(task);
}
var products = await Task.WhenAll(tasks);
sw.Stop();
_output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
Assert.NotNull(products);
Assert.Equal(100, products.Length);

// begin 10:20:45.317
// Thread36 GetProductAsync(0),10:20:45.487
// Thread11 GetProductAsync(2),10:20:45.487
// Thread8 GetProductAsync(3),10:20:45.487
// Thread35 GetProductAsync(1),10:20:45.487
// Thread11 GetProductAsync(6),10:20:45.614
// Thread35 GetProductAsync(4),10:20:45.614
// Thread8 GetProductAsync(7),10:20:45.614
// Thread36 GetProductAsync(5),10:20:45.614
// Thread35 GetProductAsync(9),10:20:45.742
// Thread8 GetProductAsync(8),10:20:45.742
// Thread41 GetProductAsync(11),10:20:45.742
// Thread37 GetProductAsync(10),10:20:45.742
// Thread37 GetProductAsync(12),10:20:45.869
// Thread8 GetProductAsync(14),10:20:45.869
// Thread11 GetProductAsync(13),10:20:45.869
// Thread41 GetProductAsync(15),10:20:45.869
// Thread8 GetProductAsync(18),10:20:45.997
// Thread35 GetProductAsync(17),10:20:45.997
// Thread41 GetProductAsync(19),10:20:45.997
// Thread11 GetProductAsync(16),10:20:45.997
// Thread11 GetProductAsync(20),10:20:46.125
// Thread8 GetProductAsync(22),10:20:46.125
// Thread35 GetProductAsync(21),10:20:46.125
// Thread41 GetProductAsync(23),10:20:46.125
// Thread37 GetProductAsync(27),10:20:46.253
// Thread41 GetProductAsync(26),10:20:46.253
// Thread35 GetProductAsync(25),10:20:46.253
// Thread11 GetProductAsync(24),10:20:46.253
// Thread35 GetProductAsync(29),10:20:46.381
// Thread41 GetProductAsync(28),10:20:46.381
// Thread11 GetProductAsync(31),10:20:46.381
// Thread37 GetProductAsync(30),10:20:46.381
// Thread8 GetProductAsync(32),10:20:46.507
// Thread37 GetProductAsync(34),10:20:46.507
// Thread41 GetProductAsync(35),10:20:46.507
// Thread11 GetProductAsync(33),10:20:46.507
// Thread37 GetProductAsync(39),10:20:46.635
// Thread11 GetProductAsync(37),10:20:46.635
// Thread41 GetProductAsync(36),10:20:46.635
// Thread35 GetProductAsync(38),10:20:46.635
// Thread41 GetProductAsync(40),10:20:46.763
// Thread37 GetProductAsync(41),10:20:46.763
// Thread35 GetProductAsync(42),10:20:46.763
// Thread11 GetProductAsync(43),10:20:46.763
// Thread41 GetProductAsync(47),10:20:46.891
// Thread8 GetProductAsync(44),10:20:46.891
// Thread11 GetProductAsync(46),10:20:46.891
// Thread37 GetProductAsync(45),10:20:46.891
// Thread37 GetProductAsync(51),10:20:47.018
// Thread8 GetProductAsync(49),10:20:47.018
// Thread41 GetProductAsync(48),10:20:47.018
// Thread11 GetProductAsync(50),10:20:47.018
// Thread41 GetProductAsync(55),10:20:47.146
// Thread11 GetProductAsync(54),10:20:47.146
// Thread8 GetProductAsync(52),10:20:47.146
// Thread37 GetProductAsync(53),10:20:47.146
// Thread11 GetProductAsync(59),10:20:47.274
// Thread8 GetProductAsync(58),10:20:47.274
// Thread37 GetProductAsync(56),10:20:47.274
// Thread41 GetProductAsync(57),10:20:47.274
// Thread41 GetProductAsync(62),10:20:47.402
// Thread11 GetProductAsync(63),10:20:47.402
// Thread37 GetProductAsync(60),10:20:47.402
// Thread8 GetProductAsync(61),10:20:47.402
// Thread41 GetProductAsync(66),10:20:47.530
// Thread88 GetProductAsync(64),10:20:47.530
// Thread35 GetProductAsync(65),10:20:47.530
// Thread11 GetProductAsync(67),10:20:47.530
// Thread11 GetProductAsync(68),10:20:47.658
// Thread41 GetProductAsync(70),10:20:47.658
// Thread8 GetProductAsync(69),10:20:47.658
// Thread35 GetProductAsync(71),10:20:47.658
// Thread41 GetProductAsync(74),10:20:47.786
// Thread95 GetProductAsync(75),10:20:47.786
// Thread35 GetProductAsync(73),10:20:47.786
// Thread88 GetProductAsync(72),10:20:47.786
// Thread95 GetProductAsync(78),10:20:47.914
// Thread41 GetProductAsync(77),10:20:47.914
// Thread88 GetProductAsync(79),10:20:47.914
// Thread8 GetProductAsync(76),10:20:47.914
// Thread95 GetProductAsync(80),10:20:48.042
// Thread41 GetProductAsync(83),10:20:48.042
// Thread8 GetProductAsync(82),10:20:48.042
// Thread35 GetProductAsync(81),10:20:48.042
// Thread95 GetProductAsync(84),10:20:48.170
// Thread35 GetProductAsync(86),10:20:48.170
// Thread41 GetProductAsync(85),10:20:48.170
// Thread8 GetProductAsync(87),10:20:48.170
// Thread11 GetProductAsync(90),10:20:48.297
// Thread88 GetProductAsync(88),10:20:48.297
// Thread8 GetProductAsync(89),10:20:48.297
// Thread35 GetProductAsync(91),10:20:48.297
// Thread11 GetProductAsync(95),10:20:48.425
// Thread8 GetProductAsync(94),10:20:48.425
// Thread41 GetProductAsync(93),10:20:48.425
// Thread35 GetProductAsync(92),10:20:48.425
// Thread41 GetProductAsync(98),10:20:48.553
// Thread35 GetProductAsync(99),10:20:48.553
// Thread8 GetProductAsync(97),10:20:48.553
// Thread88 GetProductAsync(96),10:20:48.553
// end 10:20:48.553, Elapsed 3235
~~~

## 三、进一步优化的空间
>* 傍晚在小区边的小湖散步就反复思考异步并发的问题
>* 如果把异步线程等同同步线程,在异步线程未执行完就激活了新的线程
>* 由于激活前这个过程足够快导致线程叠加
>* 造成开局即是高潮的"假象"

### 1. 《手搓》线程池核心代码
>* 异步时_processor.Run的耗时可以忽略不计
>* 如何能实现同步和异步同样的效果

~~~csharp
while (true)
{
    if (_processor.Run())
    {
        _pool.Increment();
    }
    else
    {
        await Task.Delay(_reduceTime, CancellationToken.None)
            .ConfigureAwait(false);
    }
    if (token.IsCancellationRequested)
        break;
}
~~~


### 2. 以上想明白后就好优化了
>* 优化后的代码
>* 把_processor.Run()拆分processor.TryTake(out var item)和processor.Run(ref item)
>* 先发现任务,激活线程再执行
>* 有人可能会说,你这代码逻辑有问题啊
>* 发现任务也只是证明本线程有事可干,激活新线程明显浪费资源啊
>* 这点确实应该说一下
>* 现实经验告诉我们,如果发现1只蟑螂,很可能你家已经成为了蟑螂窝
>* 就算激活一个线程没事可干消耗也不大,TryTake返回false就直接走线程回收逻辑了

~~~csharp
while (true)
{
    if (processor.TryTake(out var item))
    {
        _pool.Increment();
        processor.Run(ref item);
    }
    else
    {
        await Task.Delay(_reduceTime, CancellationToken.None)
            .ConfigureAwait(false);
    }
    if (token.IsCancellationRequested)
        break;
}
~~~

### 3. 优化后再把第一个同步Case重跑一遍
>* ConcurrencyLevel设置为10
>* 同步方法也实现了一发入魂,开局即是高潮
>* 必需承认完美的指数递进关系很有数学美
>* 但笔者做为一个码奴,追求程序性能才是终极目标

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 10 };
var scheduler = new ConcurrentTaskScheduler(options);
var factory = new TaskFactory(scheduler);
var jobService = new ConcurrentJobService(scheduler, options);
jobService.Start();
Start(factory);

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
public int Multiply(int a, int b)
{
    var result = a * b;
    _output.WriteLine($"{a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
    Thread.Sleep(100);
    return result;
}
// 1 x 6 = 6,09:15:35.332
// 1 x 3 = 3,09:15:35.332
// 1 x 2 = 2,09:15:35.332
// 1 x 4 = 4,09:15:35.332
// 1 x 5 = 5,09:15:35.332
// 1 x 9 = 9,09:15:35.333
// 1 x 7 = 7,09:15:35.333
// 1 x 8 = 8,09:15:35.333
// 1 x 1 = 1,09:15:35.332
// 2 x 1 = 2,09:15:35.333
// 2 x 6 = 12,09:15:35.443
// 2 x 3 = 6,09:15:35.443
// 2 x 7 = 14,09:15:35.443
// 2 x 4 = 8,09:15:35.443
// 2 x 8 = 16,09:15:35.443
// 2 x 5 = 10,09:15:35.443
// 2 x 2 = 4,09:15:35.443
// 2 x 9 = 18,09:15:35.443
// 3 x 1 = 3,09:15:35.443
// 3 x 2 = 6,09:15:35.444
// 4 x 3 = 12,09:15:35.554
// 3 x 4 = 12,09:15:35.554
// 3 x 5 = 15,09:15:35.554
// 3 x 3 = 9,09:15:35.554
// 3 x 7 = 21,09:15:35.554
// 3 x 9 = 27,09:15:35.554
// 4 x 1 = 4,09:15:35.554
// 3 x 8 = 24,09:15:35.554
// 3 x 6 = 18,09:15:35.554
// 4 x 2 = 8,09:15:35.554
// 4 x 6 = 24,09:15:35.666
// 5 x 4 = 20,09:15:35.666
// 4 x 8 = 32,09:15:35.666
// 4 x 9 = 36,09:15:35.666
// 4 x 7 = 28,09:15:35.666
// 5 x 3 = 15,09:15:35.666
// 4 x 4 = 16,09:15:35.666
// 5 x 2 = 10,09:15:35.666
// 5 x 1 = 5,09:15:35.666
// 4 x 5 = 20,09:15:35.666
// 5 x 6 = 30,09:15:35.777
// 6 x 1 = 6,09:15:35.777
// 6 x 5 = 30,09:15:35.777
// 6 x 4 = 24,09:15:35.777
// 5 x 9 = 45,09:15:35.777
// 5 x 5 = 25,09:15:35.777
// 6 x 2 = 12,09:15:35.777
// 5 x 8 = 40,09:15:35.777
// 5 x 7 = 35,09:15:35.777
// 6 x 3 = 18,09:15:35.777
// 6 x 9 = 54,09:15:35.888
// 6 x 8 = 48,09:15:35.888
// 7 x 1 = 7,09:15:35.888
// 6 x 7 = 42,09:15:35.888
// 6 x 6 = 36,09:15:35.888
// 7 x 6 = 42,09:15:35.888
// 7 x 4 = 28,09:15:35.888
// 7 x 3 = 21,09:15:35.888
// 7 x 5 = 35,09:15:35.888
// 7 x 2 = 14,09:15:35.888
// 7 x 8 = 56,09:15:36.000
// 7 x 7 = 49,09:15:36.000
// 7 x 9 = 63,09:15:36.000
// 8 x 1 = 8,09:15:36.000
// 8 x 7 = 56,09:15:36.000
// 8 x 2 = 16,09:15:36.000
// 8 x 3 = 24,09:15:36.000
// 8 x 5 = 40,09:15:36.000
// 8 x 6 = 48,09:15:36.000
// 8 x 4 = 32,09:15:36.000
// 8 x 8 = 64,09:15:36.112
// 8 x 9 = 72,09:15:36.112
// 9 x 2 = 18,09:15:36.112
// 9 x 1 = 9,09:15:36.112
// 9 x 3 = 27,09:15:36.112
// 9 x 4 = 36,09:15:36.112
// 9 x 5 = 45,09:15:36.112
// 9 x 7 = 63,09:15:36.112
// 9 x 6 = 54,09:15:36.112
// 9 x 8 = 72,09:15:36.112
// 9 x 9 = 81,09:15:36.223
~~~

### 4. 线程池执行同步方法开局即是高潮重要吗
>* 当然重要,这就是冷启动的速度问题
>* 如果用TaskFactory对同步方法做并发操作就更显得重要了
>* GetProduct同步方法执行1次耗时0.1秒
>* ConcurrencyLevel设置为7
>* 现在优化后获取7条只要0.1秒
>* 如果冷启动按指数递进关系要至少要0.3秒多
>* 这就快了3倍
>* 如果ConcurrencyLevel越大,效果就越明显

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 7 };
var factory = new ConcurrentTaskFactory(options);
_output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
Stopwatch sw = Stopwatch.StartNew();
List<Task<Product>> tasks = new(7);
for (int i = 0; i < 7; i++)
{
    var id = i;
    var task = factory.StartNew(() => GetProduct(id));
    tasks.Add(task);
}
var products = await Task.WhenAll(tasks);
sw.Stop();
_output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");

internal Product GetProduct(int id)
{
    Thread.Sleep(100);
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} GetProductAsync({id}),{DateTime.Now:HH:mm:ss.fff}");
    return new(id);
}

// begin 09:52:02.916
// Thread36 GetProductAsync(5),09:52:03.079
// Thread32 GetProductAsync(1),09:52:03.079
// Thread33 GetProductAsync(2),09:52:03.079
// Thread37 GetProductAsync(6),09:52:03.079
// Thread34 GetProductAsync(3),09:52:03.079
// Thread35 GetProductAsync(4),09:52:03.079
// Thread8 GetProductAsync(0),09:52:03.079
// end 09:52:03.079, Elapsed 162
~~~

好了,就介绍到这里,更多信息请查看源码库
源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！