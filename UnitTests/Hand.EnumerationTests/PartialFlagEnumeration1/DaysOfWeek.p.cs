using Hand.Enumerations;

namespace Hand.EnumerationTests.PartialFlagEnumeration1;

partial class DaysOfWeek
{
    /// <summary>
    /// 静态实例
    /// </summary>
    public static readonly IFlagEnumerationProvider<DaysOfWeek> Provider = new DaysOfWeekProvider();
    /// <summary>
    /// 星期几枚举提供者
    /// </summary>
    private sealed class DaysOfWeekProvider : FlagEnumerationProvider<DaysOfWeek>
    {
        internal DaysOfWeekProvider()
            : base(
                  new Dictionary<string, DaysOfWeek>
                  {
                      { nameof(Unknown), Unknown },
                      { nameof(Sunday), Sunday},
                      { nameof(Monday), Monday},
                      { nameof(Tuesday), Tuesday},
                      { nameof(Wednesday), Wednesday},
                      { nameof(Thursday),Thursday},
                      { nameof(Friday), Friday},
                      { nameof(Saturday), Saturday}
                  }
        )
        {
        }
        /// <inheritdoc />
        public override DaysOfWeek Empty => Unknown;
        /// <inheritdoc />
        protected override DaysOfWeek CreateFlag(ISet<string> flags, long original, string description = "")
            => new(flags, original, description);
    }
}
