# xml解析性能测试

## 1. UserSingleBench
>* 解析单个对象
>* Deserialize是反序列化方法
>* GetResult是普通的xml解析方法，普通的xml解析比反序列化快32%
>* GetResult1是定制建造者模式,避免反射,比反序列化快41%
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult3快一点

| Method      | Mean       | Error   | StdDev   | Median     | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |-----------:|--------:|---------:|-----------:|------:|-------:|-------:|----------:|------------:|
| Deserialize | 1,353.9 ns | 9.03 ns | 10.04 ns | 1,361.2 ns |  1.00 | 0.7240 | 0.0300 |   12.2 KB |        1.00 |
| GetResult   |   917.2 ns | 1.46 ns |  1.68 ns |   916.8 ns |  0.68 | 0.6750 | 0.0250 |  11.38 KB |        0.93 |
| GetResult1  |   804.0 ns | 3.16 ns |  3.64 ns |   804.3 ns |  0.59 | 0.6500 | 0.0240 |  10.96 KB |        0.90 |
| GetResult2  |   800.4 ns | 3.81 ns |  4.23 ns |   797.5 ns |  0.59 | 0.6500 | 0.0240 |  10.96 KB |        0.90 |
| GetResult3  |   799.5 ns | 6.24 ns |  6.68 ns |   799.4 ns |  0.59 | 0.6500 |      - |  10.95 KB |        0.90 |
| Custom      |   765.9 ns | 1.41 ns |  1.57 ns |   766.2 ns |  0.57 | 0.6500 |      - |  10.95 KB |        0.90 |

## 2. UserListBench
>* 解析对象列表
>* Deserialize是反序列化方法
>* GetResult是普通的xml解析方法，普通的xml解析比反序列化快29%
>* GetResult1是定制建造者模式,避免反射,比反序列化快41%
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult3快一点

| Method      | Mean     | Error     | StdDev    | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |---------:|----------:|----------:|------:|--------:|-------:|-------:|----------:|------------:|
| Deserialize | 4.291 us | 0.0531 us | 0.0569 us |  1.00 |    0.02 | 0.7660 | 0.0340 |  12.92 KB |        1.00 |
| GetResult   | 3.056 us | 0.0169 us | 0.0195 us |  0.71 |    0.01 | 0.7760 | 0.0320 |  13.09 KB |        1.01 |
| GetResult1  | 2.525 us | 0.0162 us | 0.0180 us |  0.59 |    0.01 | 0.6880 | 0.0260 |  11.61 KB |        0.90 |
| GetResult2  | 2.473 us | 0.0095 us | 0.0105 us |  0.58 |    0.01 | 0.6880 | 0.0260 |  11.61 KB |        0.90 |
| GetResult3  | 2.507 us | 0.0087 us | 0.0093 us |  0.58 |    0.01 | 0.6880 | 0.0260 |  11.59 KB |        0.90 |
| Custom      | 2.318 us | 0.0917 us | 0.1020 us |  0.54 |    0.02 | 0.6880 | 0.0260 |  11.59 KB |        0.90 |