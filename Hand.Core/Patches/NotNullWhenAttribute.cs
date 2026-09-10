#if NETFRAMEWORK || NETSTANDARD2_0
namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// 老版本补丁
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class NotNullWhenAttribute(bool returnValue) : Attribute
    {
        /// <summary>
        /// 返回值
        /// </summary>
        public bool ReturnValue { get; } = returnValue;
    }
}
#endif