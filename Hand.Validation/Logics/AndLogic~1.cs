using System.Collections.Generic;

namespace Hand.Rule.Logics;

/// <summary>
/// 与逻辑
/// </summary>
/// <typeparam name="TArgument"></typeparam>
/// <param name="items"></param>
public sealed class AndLogic<TArgument>(List<IValidation<TArgument>> items)
    : ComplexLogicBase<TArgument>(items)
{
    /// <inheritdoc />
    protected override bool Validate(List<IValidation<TArgument>> items, TArgument argument)
    {
        foreach (var item in items)
        {
            if (item.Not(argument))
                return false;
        }
        return true;
    }
}
