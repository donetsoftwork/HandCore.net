using Hand.Cache;
using Hand.Maping;
using System.Xml;

namespace Hand.ParseXml.Cachers;

/// <summary>
/// xml转化器缓存
/// </summary>
internal class XmlConvertCacher()
    : CacheFactoryBase<Type>(new DictionaryCacher<Type>())
{
    /// <inheritdoc />
    protected override TConverter CreateNew<TConverter>(in Type key)
    {
        return Type.GetTypeCode(key) switch
        {
            TypeCode.Boolean => CheckConverter<bool, TConverter>(XmlConvert.ToBoolean),
            TypeCode.Byte => CheckConverter<byte, TConverter>(XmlConvert.ToByte),
            TypeCode.Int16 => CheckConverter<short, TConverter>(XmlConvert.ToInt16),
            TypeCode.Char => CheckConverter<char, TConverter>(XmlConvert.ToChar),
            TypeCode.SByte => CheckConverter<sbyte, TConverter>(XmlConvert.ToSByte),
            TypeCode.UInt16 => CheckConverter<ushort, TConverter>(XmlConvert.ToUInt16),
            TypeCode.Int32 => CheckConverter<int, TConverter>(XmlConvert.ToInt32),
            TypeCode.UInt32 => CheckConverter<uint, TConverter>(XmlConvert.ToUInt32),
            TypeCode.Int64 => CheckConverter<long, TConverter>(XmlConvert.ToInt64),
            TypeCode.UInt64 => CheckConverter<ulong, TConverter>(XmlConvert.ToUInt64),
            TypeCode.Single => CheckConverter<float, TConverter>(XmlConvert.ToSingle),
            TypeCode.Double => CheckConverter<double, TConverter>(XmlConvert.ToDouble),
            TypeCode.Decimal => CheckConverter<decimal, TConverter>(XmlConvert.ToDecimal),
            TypeCode.DateTime => CheckConverter<DateTime, TConverter>(static str => XmlConvert.ToDateTime(str, XmlDateTimeSerializationMode.Utc)),
            TypeCode.String => CheckConverter<string, TConverter>(static str => str),
            _ => CreateOthers<TConverter>(key),
        };
    }
    /// <summary>
    /// 获取xml转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public IConverter<string, TValue> Get<TValue>()
        => Get<IConverter<string, TValue>>(typeof(TValue));
    /// <summary>
    /// 配置转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="converter"></param>
    public void Use<TValue, TConverter>(TConverter converter)
        where TConverter : IConverter<string, TValue>
        => Save(typeof(TValue), converter);
    /// <summary>
    /// 配置转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="converter"></param>
    public void Use<TValue>(Converter<string, TValue> converter)
        => Save(typeof(TValue), new DelegateConverter<string, TValue>(converter));
    /// <summary>
    /// 构造其他转化器
    /// </summary>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    private static TConverter CreateOthers<TConverter>(Type type)
    {
        if (type == typeof(Guid))
            return CheckConverter<Guid, TConverter>(XmlConvert.ToGuid);
        if (type == typeof(DateTimeOffset))
            return CheckConverter<DateTimeOffset, TConverter>(XmlConvert.ToDateTimeOffset);
        if (type == typeof(TimeSpan))
            return CheckConverter<TimeSpan, TConverter>(XmlConvert.ToTimeSpan);
        return default!;
    }
    /// <summary>
    /// 检查转化器
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TConverter"></typeparam>
    /// <param name="delegate"></param>
    /// <returns></returns>
    private static TConverter CheckConverter<TValue, TConverter>(Converter<string, TValue> @delegate)
    {
        var value = new DelegateConverter<string, TValue>(@delegate);
        if (value is TConverter converter)
            return converter;
        return default!;
    }
}
