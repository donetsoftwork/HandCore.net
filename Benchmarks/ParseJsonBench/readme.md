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

| Method      | Mean     | Error   | StdDev  | Median   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------ |---------:|--------:|--------:|---------:|------:|--------:|-------:|----------:|------------:|
| Deserialize | 130.5 ns | 1.55 ns | 1.78 ns | 130.5 ns |  1.00 |    0.02 | 0.0036 |      64 B |        1.00 |
| Generated   | 123.9 ns | 1.63 ns | 1.74 ns | 125.3 ns |  0.95 |    0.02 | 0.0036 |      64 B |        1.00 |
| GetResult   | 253.6 ns | 0.12 ns | 0.13 ns | 253.6 ns |  1.94 |    0.03 | 0.0402 |     696 B |       10.88 |
| GetResult1  | 145.6 ns | 0.42 ns | 0.45 ns | 145.6 ns |  1.12 |    0.02 | 0.0110 |     192 B |        3.00 |
| GetResult2  | 138.4 ns | 0.24 ns | 0.27 ns | 138.4 ns |  1.06 |    0.01 | 0.0110 |     192 B |        3.00 |
| GetResult3  | 138.9 ns | 0.19 ns | 0.20 ns | 138.9 ns |  1.06 |    0.01 | 0.0106 |     184 B |        2.88 |
| Custom      | 114.7 ns | 1.83 ns | 2.03 ns | 116.2 ns |  0.88 |    0.02 | 0.0106 |     184 B |        2.88 |

## 2. UserListBench
>* 解析对象列表
>* Deserialize是反序列化方法
>* Generated是使用JsonSourceGeneration生成技术‌,比Deserialize快一点
>* GetResult是使用反射解析json方法,比反序列化慢1倍左右,说明Deserialize里面应该有JIT优化
>* GetResult1是定制建造者模式,避免反射,与反序列化慢12%,已经比较接近了
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典哈希,比GetResult3不少,比JsonSourceGeneration还快

| Method      | Mean     | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------ |---------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| Deserialize | 490.0 ns |  0.93 ns |  1.04 ns |  1.00 |    0.00 | 0.0433 |     752 B |        1.00 |
| Generated   | 490.4 ns |  0.89 ns |  1.02 ns |  1.00 |    0.00 | 0.0433 |     752 B |        1.00 |
| GetResult   | 853.9 ns |  7.05 ns |  8.11 ns |  1.74 |    0.02 | 0.1260 |    2176 B |        2.89 |
| GetResult1  | 523.7 ns |  4.65 ns |  5.35 ns |  1.07 |    0.01 | 0.0383 |     664 B |        0.88 |
| GetResult2  | 485.8 ns | 10.77 ns | 12.40 ns |  0.99 |    0.02 | 0.0383 |     664 B |        0.88 |
| GetResult3  | 509.1 ns |  2.69 ns |  3.10 ns |  1.04 |    0.01 | 0.0370 |     640 B |        0.85 |
| Custom      | 406.0 ns |  8.74 ns | 10.06 ns |  0.83 |    0.02 | 0.0370 |     640 B |        0.85 |