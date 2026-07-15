using Hand.Cache;
using Hand.Maping;
using Hand.ParseJson.Contracts;
using Hand.ParseJson.Primitives;
using Hand.Utf8;

namespace Hand.ParseJson.Cachers;

/// <summary>
/// 基础类型缓存
/// </summary>
public class PrimitiveReaderCacher(DefaultValueBuilder defaultValue)
    : CacheFactoryBase<Type>(new DictionaryCacher<Type>())
{
    #region 配置
    private readonly DefaultValueBuilder _defaultValue = defaultValue;
    /// <summary>
    /// 默认值提供者
    /// </summary>
    public DefaultValueBuilder DefaultValue
        => _defaultValue;
    /// <summary>
    /// 解析器
    /// </summary>
    private static readonly Parser _parser = new(default);
    /// <summary>
    /// 解析器
    /// </summary>
    public static Parser Parser
        => _parser;
    #endregion

    /// <inheritdoc />
    protected override TConverter CreateNew<TConverter>(in Type key)
    {
        return Type.GetTypeCode(key) switch
        {
            TypeCode.Boolean => CheckReader<bool, TConverter>(new BoolReader(_defaultValue.Get<bool>())),
            TypeCode.Byte => CheckReader<byte, TConverter>(Greate(_defaultValue.Get<byte>())),
            TypeCode.SByte => CheckReader<sbyte, TConverter>(Greate(_defaultValue.Get<sbyte>())),
            TypeCode.Int16 => CheckReader<short, TConverter>(Greate(_defaultValue.Get<short>())),
            TypeCode.UInt16 => CheckReader<ushort, TConverter>(Greate(_defaultValue.Get<ushort>())),
            TypeCode.Int32 => CheckReader<int, TConverter>(Greate(_defaultValue.Get<int>())),
            TypeCode.UInt32 => CheckReader<uint, TConverter>(Greate(_defaultValue.Get<uint>())),
            TypeCode.Int64 => CheckReader<long, TConverter>(Greate(_defaultValue.Get<long>())),
            TypeCode.UInt64 => CheckReader<ulong, TConverter>(Greate(_defaultValue.Get<ulong>())),
            TypeCode.Single => CheckReader<float, TConverter>(Greate(_defaultValue.Get<float>())),
            TypeCode.Double => CheckReader<double, TConverter>(Greate(_defaultValue.Get<double>())),
            TypeCode.Decimal => CheckReader<decimal, TConverter>(Greate(_defaultValue.Get<decimal>())),
            TypeCode.DateTime => CheckReader<DateTime, TConverter>(Greate(_defaultValue.Get<DateTime>())),
            TypeCode.Char => CheckReader<char, TConverter>(Greate(_defaultValue.Get<char>())),
            TypeCode.String => CheckReader<string, TConverter>(new Primitives.StringReader(_defaultValue.Get<string>())),
            TypeCode.Object => CheckReader<object, TConverter>(new PrimitiveParser(_defaultValue.Get<object>())),
            _ => CreateOthers<TConverter>(key),
        };
    }
    /// <summary>
    /// 构造其他转化器
    /// </summary>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    private TConverter CreateOthers<TConverter>(Type type)
    {
        if (type == typeof(Guid))
            return CheckReader<Guid, TConverter>(Greate(_defaultValue.Get<Guid>()));
        if (type == typeof(DateTimeOffset))
            return CheckReader<DateTimeOffset, TConverter>(Greate(_defaultValue.Get<DateTimeOffset>()));
        if (type == typeof(TimeSpan))
            return CheckReader<TimeSpan, TConverter>(Greate(_defaultValue.Get<TimeSpan>()));
        return default!;
    }
    /// <summary>
    /// 获取json转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public IJsonParser<TValue> Get<TValue>()
        => Get<IJsonParser<TValue>>(typeof(TValue));
    /// <summary>
    /// 配置转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="converter"></param>
    public void Use<TValue>(ISpanConverter<byte, TValue> converter)
        => Save(typeof(TValue), converter);
    #region Greate
    /// <summary>
    /// 构造byte读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<byte> Greate(byte defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造sbyte读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<sbyte> Greate(sbyte defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造short读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<short> Greate(short defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造ushort读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<ushort> Greate(ushort defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造int读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<int> Greate(int defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造uint读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<uint> Greate(uint defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造long读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<long> Greate(long defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造ulong读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<ulong> Greate(ulong defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造float读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<float> Greate(float defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造double读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<double> Greate(double defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造decimal读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<decimal> Greate(decimal defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造char读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<char> Greate(char defaultValue)
        => new(new CharConverter(defaultValue), defaultValue);
    /// <summary>
    /// 构造DateTime读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<DateTime> Greate(DateTime defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造DateTimeOffset读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<DateTimeOffset> Greate(DateTimeOffset defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造TimeSpan读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<TimeSpan> Greate(TimeSpan defaultValue)
        => new(_parser, defaultValue);
    /// <summary>
    /// 构造Guid读取器
    /// </summary>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static PrimitiveReader<Guid> Greate(Guid defaultValue)
        => new(_parser, defaultValue);
    #endregion
    /// <summary>
    /// 检查转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="jsonGet"></param>
    /// <returns></returns>
    private static TConverter CheckReader<TValue, TConverter>(IJsonParser<TValue> jsonGet)
    {
        if (jsonGet is TConverter reader)
            return reader;
        return default!;
    }
}
