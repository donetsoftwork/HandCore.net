# 《手搓》线程池

## 一、什么是《手搓》线程池
>* 手搓线程池并不是用来完全代替系统线程池的
>* 你可以把手搓线程池看做系统线程池的一部分
>* 就好比在东海用集装箱搞养殖
>* 一个集装箱里养鱼
>* 另一个集装箱里养虾
>* 搞好隔离,鱼虾都不耽搁

## 二、最常用线程池的场景是什么
>* 当然是Task,是用TaskFactory.StartNew方法创建Task
>* TaskFactory也可以手搓
>* 手搓TaskFactory就需要手搓TaskScheduler
>* 前一篇文章的手搓EventBus里面包含了手搓的TaskScheduler

## 三、手搓TaskScheduler
>* ConcurrentTaskScheduler就是一个手搓的TaskScheduler

```csharp
class ConcurrentTaskScheduler(ConcurrentOptions options)
    : TaskScheduler, IProcessor
{
    // ...
}
```

### 1. 赶紧测试一下
>* 测试失败,上来一闷棍
>* 触发超时异常
>* Task就没执行啊！！！

```csharp
var options = new ConcurrentOptions { ConcurrencyLevel = 10 };
var scheduler = new ConcurrentTaskScheduler(options);
var factory = new TaskFactory(scheduler);
var task = factory.StartNew(() => Add(1, 2));
var result = await TimeoutHelper.ThrowIfTimeout(task, TimeSpan.FromSeconds(1));

public static int Add(int a, int b)
{
    var result = a + b;
    Console.WriteLine($"reslut:{result}");
    return result;
}
```

### 2. 测试用例改进一下
>* 增加scheduler.Run测试通过
>* 但是加一个任务要调用一次Run是不是太麻烦了
>* 现在就该手搓线程池出马了

```csharp
var options = new ConcurrentOptions { ConcurrencyLevel = 10 };
var scheduler = new ConcurrentTaskScheduler(options);
var factory = new TaskFactory(scheduler);
var task = factory.StartNew(() => Add(1, 2));
scheduler.Run();
var result = await TimeoutHelper.ThrowIfTimeout(task, TimeSpan.FromSeconds(1));
```

### 3. 手搓线程池测试
>* ConcurrentJobService就是那个手搓线程池
>* 计算九九乘法表,每次算术0.1秒,1秒结束
>* 其一说明线程池起作用了,不用手动调用Run了
>* 其二线程池提供了并发处理能力,平均并发为8
>* 仔细观察结果会发现,最开始的并发是1,后面越来越大,最大并发是10

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

## 四、揭秘手搓线程池
### 1. ConcurrentJobService
>* 这个名字是不是很奇怪
>* 线程池不应该叫ThreadPool吗?
>* 原来线程池里面还藏着一个线程池!!!
>* ConcurrentJobService是对线程池的封装,它比真的线程池还好用
>* 所以笔者直接叫它线程池
>* 笔者不会直接使用ThreadJobPool,而是用ConcurrentJobService

```csharp
class ConcurrentJobService : ReduceJobService
{
    ThreadJobPool Pool { get; }
    IProcessor Processor { get; }
}
```

### 2. ConcurrentJobService工作原理
>* ConcurrentJobService本身就是一个线程,可以称为主线程
>* ConcurrentJobService执行成功就从Pool里面激活一个线程
>* ConcurrentJobService执行失败就休眠一段时间,通过ReduceTime配置(默认50毫秒)
>* 主线程就像找食物的搜索狼,线程池就像狼窝,狼窝里面都是贪吃的吃货狼
>* 搜索狼找到食物就开吃,每吃一口就狼嚎
>* 听到一声狼嚎,狼窝里面就出来一个吃货狼
>* 吃货狼也是每吃一口就狼嚎
>* 这是指数数列的关系(很快就能达到最高并发)
>* 吃货狼吃完了就回狼窝睡觉
>* 搜索狼吃完了就慢腾腾的继续搜索(相当于心跳包)

## 五、不用TaskFactory能用手搓线程池吗?
>* 当然能
>* ActionThreadPool提供类系统线程池效果,传个委托就行
>* 其实就是实现IProcessor,把需要做的事情包装进去就行了
>* 自己再手搓一个也不难

### 1. 单并发测试
>* ConcurrencyLevel设置为1
>* 通过Add添加要执行的委托
>* 记得要调用Start方法启动线程,否则不会执行的哟
>* 所有action都在Thread31上执行

```csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var pool = new ActionThreadPool(options);
pool.Start();
pool.Add(() => Hello("张三"));
pool.Add(() => Hello("李四"));

void Hello(string name)
{
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
}

// Thread31 Hello 张三,10:41:40.145
// Thread31 Hello 李四,10:41:40.147
```

### 2. 多并发测试
>* ConcurrencyLevel设置为4
>* 一次性添加了100个action
>* 所有action都在Thread31、Thread32、Thread33和Thread34上执行

```csharp
var options = new ReduceOptions { ConcurrencyLevel = 4 };
var pool = new ActionThreadPool(options);
pool.Start();
for (int i = 0; i < 100; i++)
{
    var user = "User" + i;
    pool.Add(() => Hello(user));
}

void Hello(string name)
{
    _output.WriteLine($"Thread{Environment.CurrentManagedThreadId} Hello {name},{DateTime.Now:HH:mm:ss.fff}");
    Thread.Sleep(1);
}

// Thread31 Hello User0,10:56:12.014
// Thread31 Hello User1,10:56:12.025
// Thread32 Hello User2,10:56:12.025
// Thread32 Hello User3,10:56:12.041
// Thread33 Hello User4,10:56:12.041
// Thread31 Hello User5,10:56:12.041
// Thread34 Hello User6,10:56:12.041
// Thread33 Hello User8,10:56:12.057
// Thread32 Hello User7,10:56:12.057
// Thread31 Hello User9,10:56:12.057
// Thread34 Hello User10,10:56:12.057
// Thread32 Hello User11,10:56:12.073
// Thread33 Hello User12,10:56:12.073
// Thread31 Hello User14,10:56:12.073
// Thread34 Hello User13,10:56:12.073
// Thread33 Hello User15,10:56:12.089
// Thread34 Hello User16,10:56:12.089
// Thread32 Hello User18,10:56:12.089
// Thread31 Hello User17,10:56:12.089
// Thread33 Hello User20,10:56:12.105
// Thread34 Hello User19,10:56:12.105
// Thread32 Hello User21,10:56:12.105
// Thread31 Hello User22,10:56:12.105
// Thread31 Hello User23,10:56:12.121
// Thread32 Hello User24,10:56:12.121
// Thread33 Hello User26,10:56:12.121
// Thread34 Hello User25,10:56:12.121
// Thread31 Hello User28,10:56:12.137
// Thread33 Hello User30,10:56:12.137
// Thread34 Hello User29,10:56:12.137
// Thread32 Hello User27,10:56:12.137
// Thread32 Hello User31,10:56:12.153
// Thread31 Hello User33,10:56:12.153
// Thread34 Hello User34,10:56:12.153
// Thread33 Hello User32,10:56:12.153
// Thread34 Hello User36,10:56:12.169
// Thread32 Hello User35,10:56:12.169
// Thread33 Hello User37,10:56:12.169
// Thread31 Hello User38,10:56:12.169
// Thread32 Hello User39,10:56:12.185
// Thread31 Hello User41,10:56:12.185
// Thread34 Hello User42,10:56:12.185
// Thread33 Hello User40,10:56:12.185
// Thread34 Hello User43,10:56:12.201
// Thread33 Hello User45,10:56:12.201
// Thread31 Hello User44,10:56:12.201
// Thread32 Hello User46,10:56:12.201
// Thread31 Hello User47,10:56:12.217
// Thread32 Hello User48,10:56:12.217
// Thread33 Hello User49,10:56:12.217
// Thread34 Hello User50,10:56:12.217
// Thread33 Hello User51,10:56:12.233
// Thread31 Hello User52,10:56:12.233
// Thread32 Hello User53,10:56:12.233
// Thread34 Hello User54,10:56:12.233
// Thread34 Hello User55,10:56:12.249
// Thread32 Hello User56,10:56:12.249
// Thread33 Hello User57,10:56:12.249
// Thread31 Hello User58,10:56:12.249
// Thread31 Hello User61,10:56:12.265
// Thread32 Hello User62,10:56:12.265
// Thread34 Hello User59,10:56:12.265
// Thread33 Hello User60,10:56:12.265
// Thread34 Hello User66,10:56:12.281
// Thread31 Hello User65,10:56:12.281
// Thread33 Hello User63,10:56:12.281
// Thread32 Hello User64,10:56:12.281
// Thread34 Hello User69,10:56:12.297
// Thread32 Hello User67,10:56:12.297
// Thread31 Hello User68,10:56:12.297
// Thread33 Hello User70,10:56:12.297
// Thread34 Hello User73,10:56:12.313
// Thread33 Hello User72,10:56:12.313
// Thread31 Hello User71,10:56:12.313
// Thread32 Hello User74,10:56:12.313
// Thread32 Hello User77,10:56:12.329
// Thread33 Hello User76,10:56:12.329
// Thread34 Hello User75,10:56:12.329
// Thread31 Hello User78,10:56:12.329
// Thread34 Hello User79,10:56:12.345
// Thread33 Hello User80,10:56:12.345
// Thread32 Hello User81,10:56:12.345
// Thread31 Hello User82,10:56:12.345
// Thread33 Hello User83,10:56:12.361
// Thread34 Hello User84,10:56:12.361
// Thread32 Hello User86,10:56:12.361
// Thread31 Hello User85,10:56:12.361
// Thread31 Hello User87,10:56:12.377
// Thread33 Hello User88,10:56:12.377
// Thread32 Hello User89,10:56:12.377
// Thread34 Hello User90,10:56:12.377
// Thread33 Hello User92,10:56:12.393
// Thread34 Hello User93,10:56:12.393
// Thread32 Hello User91,10:56:12.393
// Thread31 Hello User94,10:56:12.393
// Thread31 Hello User95,10:56:12.409
// Thread32 Hello User96,10:56:12.409
// Thread33 Hello User98,10:56:12.409
// Thread34 Hello User97,10:56:12.409
// Thread31 Hello User99,10:56:12.425
```

### 3. 用ActionThreadPool,能不能知道它啥时候执行完呢
>* 这不就是TaskFactory做的事情吗?
>* 现在强制ActionThreadPool能做到吗?
>* 山人自有妙计,完全可以实现
>* 用TaskWrapper把action包装成Task再让ActionThreadPool执行就OK了
>* TaskWrapper是用TaskCompletionSource简单封装而来
>* 如果action执行有异常,await也是会抛异常的哟

```csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var pool = new ActionThreadPool(options);
pool.Start();
var wrapper1 = TaskWrapper.Wrap(() => Hello("张三"));
var wrapper2 = TaskWrapper.Wrap(() => Hello("李四"));
pool.Add(wrapper1.Run);
pool.Add(wrapper2.Run);
await Task.WhenAll(wrapper1.Original, wrapper2.Original);

// Thread31 Hello 张三,11:58:47.600
// Thread31 Hello 李四,11:58:47.617
```

### 4. 用ActionThreadPool能执行Func并拿到结果吗
>* 当然也能
>* 还是要用山人妙计
>* 用TaskWrapper包装后再让ActionThreadPool执行

```csharp
var options = new ReduceOptions { ConcurrencyLevel = 1 };
var pool = new ActionThreadPool(options);
pool.Start();
var wrapper = TaskWrapper.Wrap(() => Multiply(9, 9));
pool.Add(wrapper.Run);
var result = await wrapper.Original;
Assert.Equal(81, result);

int Multiply(int a, int b)
{
    var result = a * b;
    _output.WriteLine($"{a} x {b} = {result},{DateTime.Now:HH:mm:ss.fff}");
    return result;
}

// 9 x 9 = 81,11:27:54.537
```

好了,就介绍到这里,更多信息请查看源码库
源码托管地址: https://github.com/donetsoftwork/HandCore.net ，欢迎大家直接查看源码。
gitee同步更新:https://gitee.com/donetsoftwork/HandCore.net

如果大家喜欢请动动您发财的小手手帮忙点一下Star,谢谢！！！