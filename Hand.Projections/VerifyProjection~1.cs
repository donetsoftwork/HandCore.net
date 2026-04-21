using Hand.Rule;

namespace Hand.Maping;

/// <summary>
/// 校验投影
/// </summary>
/// <typeparam name="TArgument"></typeparam>
/// <param name="validation"></param>
public class VerifyProjection<TArgument>(IValidation<TArgument> validation)
    : ProjectionBase<TArgument>, IProjection<TArgument>
{
    #region 配置
    private readonly IValidation<TArgument> _validation = validation;
    #endregion

    #region IProjection<TEntity, TEntity>
    /// <inheritdoc />
    public override bool TryConvert(TArgument source, out TArgument value)
    {
        value = source;
        return _validation.Validate(source);
    }
    #endregion
    #region IValidation<TEntity>
    /// <inheritdoc />
    public override bool Validate(TArgument source)
        => _validation.Validate(source);
    #endregion
    #region IConverter<T, T>   
    /// <inheritdoc />
    public override TArgument Convert(TArgument source)
        => source;
    #endregion
}
