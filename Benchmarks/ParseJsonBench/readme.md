# json解析性能测试

## 1. UserSingleBench
>* 解析单个对象
>* Deserialize是反序列化方法
>* Generated是使用JsonSourceGeneration生成技术‌,比Deserialize快一点
>* GetResult是使用反射解析json方法,比反序列化慢1倍左右,说明Deserialize里面应该有JIT优化
>* GetResult1是定制建造者模式,避免反射,与反序列化慢12%,已经比较接近了
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典哈希,比GetResult3不少,比JsonSourceGeneration还快

| Method      | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------ |---------:|--------:|--------:|------:|--------:|-------:|----------:|------------:|
| Deserialize | 130.8 ns | 2.74 ns | 3.16 ns |  1.00 |    0.03 | 0.0036 |      64 B |        1.00 |
| Generated   | 124.0 ns | 0.62 ns | 0.67 ns |  0.95 |    0.02 | 0.0036 |      64 B |        1.00 |
| GetResult   | 261.3 ns | 1.34 ns | 1.55 ns |  2.00 |    0.05 | 0.0402 |     696 B |       10.88 |
| GetResult1  | 150.6 ns | 0.32 ns | 0.35 ns |  1.15 |    0.03 | 0.0110 |     192 B |        3.00 |
| GetResult2  | 150.5 ns | 1.61 ns | 1.85 ns |  1.15 |    0.03 | 0.0110 |     192 B |        3.00 |
| GetResult3  | 144.0 ns | 0.28 ns | 0.33 ns |  1.10 |    0.03 | 0.0106 |     184 B |        2.88 |
| Custom      | 114.9 ns | 0.49 ns | 0.56 ns |  0.88 |    0.02 | 0.0106 |     184 B |        2.88 |

## 2. UserListBench
>* 解析对象列表
>* Deserialize是反序列化方法
>* Generated是使用JsonSourceGeneration生成技术‌,比Deserialize快一点
>* GetResult是使用反射解析json方法,比反序列化慢1倍左右,说明Deserialize里面应该有JIT优化
>* GetResult1是定制建造者模式,避免反射,与反序列化慢12%,已经比较接近了
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典哈希,比GetResult3不少,比JsonSourceGeneration还快

| Method      | Mean     | Error   | StdDev   | Median   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------ |---------:|--------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Deserialize | 512.7 ns | 5.70 ns |  6.56 ns | 512.4 ns |  1.00 |    0.02 | 0.0433 |     752 B |        1.00 |
| Generated   | 482.6 ns | 2.52 ns |  2.90 ns | 482.5 ns |  0.94 |    0.01 | 0.0433 |     752 B |        1.00 |
| GetResult   | 882.4 ns | 9.30 ns | 10.71 ns | 882.5 ns |  1.72 |    0.03 | 0.1260 |    2176 B |        2.89 |
| GetResult1  | 534.3 ns | 6.74 ns |  7.49 ns | 540.4 ns |  1.04 |    0.02 | 0.0383 |     664 B |        0.88 |
| GetResult2  | 511.8 ns | 7.14 ns |  8.23 ns | 512.1 ns |  1.00 |    0.02 | 0.0383 |     664 B |        0.88 |
| GetResult3  | 517.3 ns | 2.81 ns |  3.24 ns | 517.2 ns |  1.01 |    0.01 | 0.0370 |     640 B |        0.85 |
| Custom      | 414.0 ns | 2.56 ns |  2.95 ns | 413.6 ns |  0.81 |    0.01 | 0.0370 |     640 B |        0.85 |
