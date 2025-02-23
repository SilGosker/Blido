using System;
using System.Linq.Expressions;

namespace Blido.Core.Helpers;

public class CaseInsensitivityHelper
{
    public static bool IsCaseInsensitive(StringComparison comparison)
    {
        return comparison is StringComparison.CurrentCultureIgnoreCase
            or StringComparison.InvariantCultureIgnoreCase
            or StringComparison.OrdinalIgnoreCase;
    }

    public static bool IsCaseInsensitive(Expression expression)
    {
        return TryGetStringComparison(expression, out StringComparison stringComparison) && IsCaseInsensitive(stringComparison);
    }

    public static bool TryGetStringComparison(Expression expression, out StringComparison stringComparison)
    {
        stringComparison = default;

        if (expression is ConstantExpression { Value: StringComparison comparison } && Enum.IsDefined(comparison))
        {
            stringComparison = comparison;
            return true;
        }

        return false;
    }

}