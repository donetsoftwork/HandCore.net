using System.Collections.Generic;

namespace Hand.Rule.Logics;

/// <summary>
/// OR逻辑
/// </summary>
/// <typeparam name="TArgument"></typeparam>
/// <param name="items"></param>
public sealed class OrLogic<TArgument>(List<IValidation<TArgument>> items)
    : ComplexLogicBase<TArgument>(items)
{
    /// <inheritdoc />
    protected override bool Validate(List<IValidation<TArgument>> items, TArgument argument)
    {
        foreach (var item in items)
        {
            if (item.Validate(argument))
                return true;
        }
        return false;
    }
}
