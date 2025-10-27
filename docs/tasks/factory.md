# 《手搓》TaskFactory带你安全的起飞

## 一、TaskFactory也能手搓
>* 是的TaskFactory也能手搓
>* 而且效果是杠杠的

## 二、现在继续说程序优化的事情
### 1. 话说产品经理小马给工程师小牛提了需求批量获取产品详情
>* 小牛三下五除二就干上线了
>* 代码那是写的干净又漂亮,没有一行多余的代码
>* 只是性能有一点点瑕疵
>* 每个商品获取要0.1秒,获取10个就是1秒多
>* 小马说: 我看博客园有个博主说了,不要在循环里面直接await
>* 小牛说: 是吗?转发我看一下

~~~csharp
_output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
Stopwatch sw = Stopwatch.StartNew();
List<Product> products = new(10);
for (int i = 0; i < 10; i++)
{
    var id = i;
    var item = await GetProductAsync(id);
    products.Add(item);
}
sw.Stop();
_output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
// begin 09:10:06.086
// end 09:10:07.206, Elapsed 1118


private const int _concurrentLimit = 6;
private readonly ConcurrentControl _control = new();
private readonly ITestOutputHelper _output = output;
internal async Task<Product> GetProductAsync(int id)
{
    _control.Increment();
    await Task.Delay(100);
    var concurrent = _control.Count;
    _control.Decrement();
    if (concurrent > _concurrentLimit)
    {
        throw new Exception("Server is busy!!!");
    }
    return new(id);
}
~~~

### 2. 小牛说so easy,稍等
>* 小牛立马悟了
>* 马上把代码优化了
>* 可是一上线程序就挂了
>* 由于上游允许的最高并发是6,10个并发把上游给打挂了
>* 小牛说: 小马你坑惨我了呀
>* 小牛立马私信来骂那个博客园博主
>* 对笔者就是那个博主
>* 自己挖的坑终归是要填的,为此只好给小牛安利手搓TaskFactory

~~~csharp
_output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
Stopwatch sw = Stopwatch.StartNew();
List<Task<Product>> tasks = new(10);
for (int i = 0; i < 10; i++)
{
    var id = i;
    var task = GetProductAsync(id);
    tasks.Add(task);
}
var products = await Task.WhenAll(tasks);
sw.Stop();
_output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");

// begin 09:22:05.622
// System.Exception : Server is busy!!!
~~~

### 3. 用手搓TaskFactory来优化
>* ConcurrentTaskFactory就是手搓的TaskFactory
>* ConcurrencyLevel设置为6
>* 使用StartTask发起异步请求
>* 其他代码都不变
>* 耗时不到0.3秒,比原来快了3倍多
>* http服务是天生的多线程
>* 即使你单线程调用数据库,在大量抓取的情况下,数据库也会被打挂
>* 不知道大家有没有打挂过自家数据库的经历
>* 用上《手搓》TaskFactory,可以对上游做到并发可控,程序改动还极小
>* 小牛看得那是目瞪口呆,这是怎么实现的
>* 小牛说: 快教教我
>* 笔者说: 没问题,我写篇博文你看了自然就明白了
>* 于是这篇文章诞生了
>* 话说博客园也偶发数据库被打挂的情况
>* 不知道Dudu会不会看到这篇文章

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 6 };
var factory = new ConcurrentTaskFactory(options);
_output.WriteLine($"begin {DateTime.Now:HH:mm:ss.fff}");
Stopwatch sw = Stopwatch.StartNew();
List<Task<Product>> tasks = new(10);
for (int i = 0; i < 10; i++)
{
    var id = i;
    var task = factory.StartTask(() => GetProductAsync(id));
    tasks.Add(task);
}
var products = await Task.WhenAll(tasks);
sw.Stop();
_output.WriteLine($"end {DateTime.Now:HH:mm:ss.fff}, Elapsed {sw.ElapsedMilliseconds}");
// begin 09:33:03.070
// end 09:33:03.370, Elapsed 299
~~~

## 三、 揭秘《手搓》TaskFactory
### 1. 单并发异步操作
>* ConcurrencyLevel设置为1
>* 异步调用10次
>* 虽然执行异步的线程ID不止一个
>* 实际执行每0.1秒执行一次
>* 也就是等上一次执行完才开始下一次
>* 现在谁还敢说异步线程不是乖宝宝!!!

~~~csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var factory = new ConcurrentTaskFactory(options);
List<Task<Product>> tasks = new(10);
for (int i = 0; i < 10; i++)
{
    var id = i;
    var task = factory.StartTask(() => GetProductAsync(id));
    tasks.Add(task);
}
var products = await Task.WhenAll(tasks);
Assert.NotNull(products);
Assert.Equal(10, products.Length);

internal async Task<Product> GetProductAsync(int id)
{
    _control.Increment();
    await Task.Delay(100);
    var concurrent = _control.Count;
    _control.Decrement();
    if (concurrent > _concurrentLimit)
    {
        throw new Exception("Server is busy!!!");
    }
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} GetProductAsync({id}),{DateTime.Now:HH:mm:ss.fff}");
    return new(id);
}

// Thread10 GetProductAsync(0),10:04:37.009
// Thread11 GetProductAsync(1),10:04:37.135
// Thread32 GetProductAsync(2),10:04:37.263
// Thread11 GetProductAsync(3),10:04:37.391
// Thread32 GetProductAsync(4),10:04:37.518
// Thread11 GetProductAsync(5),10:04:37.646
// Thread11 GetProductAsync(6),10:04:37.774
// Thread11 GetProductAsync(7),10:04:37.902
// Thread11 GetProductAsync(8),10:04:38.030
// Thread11 GetProductAsync(9),10:04:38.158
~~~

### 2. 并发测试
>* ConcurrencyLevel设置为4
>* 异步调用100次
>* 总耗时3秒
>* 4个并发清晰可见

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

### 3. 《手搓》TaskFactory异步排队原理
>* 《手搓》TaskFactory控制住了异步的开始和结束
>* 当发起异步请求后,该线程并不在《手搓》TaskFactory的线程池中
>* 为了控制并发,也扣除并发配额
>* 并在注册ContinueWith中返还并发配额
>* 控制住了异步的开始和结束,就相当于异步线程也在《手搓》TaskFactory的线程池中运行
>* 配额不够,后面的异步线程就不会触发

## 四、总结
>* 《手搓》TaskFactory里面内置了线程池
>* 应该定义为静态的字段或属性
>* 也可以注册到容器中为单例对象
>* 可以对每个数据库或其他上游资源分别建一个实例,用来控制并发
>* 这样就可以放心大胆的使用并发
>* 程序性能开始起飞
>* 同时程序的安全性也得到了坚实的保障
>* 涉及IO异步操作线程池可以设大一点,只要不会打挂上游就行
>* IO异步线程和普通线程不一样,不会太影响CPU
>* 同步和异步IO的线程最好不要混用,影响配额的准确配置

另外源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！