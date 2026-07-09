# xml解析性能测试

## 1. UserSingleBench
>* 解析单个对象
>* Deserialize是反序列化方法
>* GetResult是普通的xml解析方法，普通的xml解析比反序列化快31%
>* GetResult1是定制建造者模式,避免反射,比反序列化快41%
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult3快一点

| Method      | Mean       | Error   | StdDev  | Median     | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |-----------:|--------:|--------:|-----------:|------:|-------:|-------:|----------:|------------:|
| Deserialize | 1,349.1 ns | 6.44 ns | 6.89 ns | 1,349.2 ns |  1.00 | 0.7240 | 0.0300 |   12.2 KB |        1.00 |
| GetResult   |   924.9 ns | 5.21 ns | 6.00 ns |   924.7 ns |  0.69 | 0.6750 | 0.0250 |  11.38 KB |        0.93 |
| GetResult1  |   816.1 ns | 7.64 ns | 8.49 ns |   822.6 ns |  0.60 | 0.6500 | 0.0240 |  10.96 KB |        0.90 |
| GetResult2  |   810.5 ns | 0.86 ns | 0.99 ns |   810.4 ns |  0.60 | 0.6500 | 0.0240 |  10.96 KB |        0.90 |
| GetResult3  |   802.2 ns | 2.22 ns | 2.38 ns |   803.5 ns |  0.59 | 0.6500 |      - |  10.95 KB |        0.90 |
| Custom      |   767.9 ns | 1.94 ns | 2.08 ns |   767.7 ns |  0.57 | 0.6500 |      - |  10.95 KB |        0.90 |

## 2. UserListBench
>* 解析对象列表
>* Deserialize是反序列化方法
>* GetResult是普通的xml解析方法，普通的xml解析比反序列化快28%
>* GetResult1是定制建造者模式,避免反射,比反序列化快40%
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult3快一点


| Method      | Mean     | Error     | StdDev    | Median   | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |---------:|----------:|----------:|---------:|------:|-------:|-------:|----------:|------------:|
| Deserialize | 2.488 us | 0.0012 us | 0.0013 us | 2.488 us |  1.00 | 0.7680 | 0.0310 |  12.94 KB |        1.00 |
| GetResult   | 1.792 us | 0.0080 us | 0.0089 us | 1.785 us |  0.72 | 0.7690 | 0.0300 |  12.97 KB |        1.00 |
| GetResult1  | 1.458 us | 0.0125 us | 0.0139 us | 1.468 us |  0.59 | 0.6830 | 0.0270 |  11.49 KB |        0.89 |
| GetResult2  | 1.438 us | 0.0023 us | 0.0026 us | 1.438 us |  0.58 | 0.6830 | 0.0270 |  11.49 KB |        0.89 |
| GetResult3  | 1.444 us | 0.0027 us | 0.0032 us | 1.443 us |  0.58 | 0.6810 | 0.0260 |  11.47 KB |        0.89 |
| Custom      | 1.378 us | 0.0015 us | 0.0016 us | 1.378 us |  0.55 | 0.6810 | 0.0260 |  11.47 KB |        0.89 |