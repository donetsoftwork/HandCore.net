# xml解析性能测试

## 1. UserSingleBench
>* 解析单个对象
>* Deserialize是反序列化方法
>* GetResult是普通的xml解析方法，普通的xml解析比反序列化快31%
>* GetResult1是定制建造者模式,避免反射,比反序列化快41%
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult3快一点

| Method      | Mean       | Error   | StdDev  | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |-----------:|--------:|--------:|------:|-------:|-------:|----------:|------------:|
| Deserialize | 1,355.5 ns | 4.44 ns | 4.75 ns |  1.00 | 0.7240 | 0.0300 |   12.2 KB |        1.00 |
| GetResult   |   912.7 ns | 5.07 ns | 5.84 ns |  0.67 | 0.6750 | 0.0250 |  11.38 KB |        0.93 |
| GetResult1  |   808.0 ns | 4.73 ns | 5.45 ns |  0.60 | 0.6500 | 0.0240 |  10.96 KB |        0.90 |
| GetResult2  |   806.6 ns | 2.68 ns | 3.09 ns |  0.60 | 0.6500 | 0.0240 |  10.96 KB |        0.90 |
| GetResult3  |   798.3 ns | 0.40 ns | 0.42 ns |  0.59 | 0.6500 |      - |  10.95 KB |        0.90 |
| Custom      |   779.4 ns | 6.46 ns | 7.43 ns |  0.57 | 0.6500 |      - |  10.95 KB |        0.90 |

## 2. UserListBench
>* 解析对象列表
>* Deserialize是反序列化方法
>* GetResult是普通的xml解析方法，普通的xml解析比反序列化快28%
>* GetResult1是定制建造者模式,避免反射,比反序列化快40%
>* GetResult2是定制建造者模式的工厂,比GetResult1快一点
>* GetResult3是空构造函数简化的建造者模式,节省了一个对象构造,比GetResult2快一点
>* Custom继承重写EntityParser,避免字典查找,比GetResult3快一点

| Method      | Mean     | Error     | StdDev    | Ratio | Gen0   | Gen1   | Allocated | Alloc Ratio |
|------------ |---------:|----------:|----------:|------:|-------:|-------:|----------:|------------:|
| Deserialize | 2.489 us | 0.0076 us | 0.0085 us |  1.00 | 0.7660 | 0.0340 |  12.92 KB |        1.00 |
| GetResult   | 1.815 us | 0.0054 us | 0.0062 us |  0.73 | 0.7720 | 0.0280 |  13.02 KB |        1.01 |
| GetResult1  | 1.475 us | 0.0057 us | 0.0064 us |  0.59 | 0.6840 | 0.0260 |  11.54 KB |        0.89 |
| GetResult2  | 1.459 us | 0.0006 us | 0.0006 us |  0.59 | 0.6840 | 0.0260 |  11.54 KB |        0.89 |
| GetResult3  | 1.467 us | 0.0083 us | 0.0096 us |  0.59 | 0.6840 | 0.0260 |  11.52 KB |        0.89 |
| Custom      | 1.375 us | 0.0055 us | 0.0059 us |  0.55 | 0.6840 | 0.0260 |  11.52 KB |        0.89 |

