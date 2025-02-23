using System.Reflection;
using System.Runtime.CompilerServices;

namespace Blido.Core.Transaction.JsExpression.MethodCallTranslation;

/// <summary>
/// Equality comparer for MethodInfo to ignore generic type parameters.
/// </summary>
public sealed class MethodInfoEqualityComparer : IEqualityComparer<MethodInfo>
{
    public bool Equals(MethodInfo? x, MethodInfo? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;

        // Compare declaring type
        if (x.DeclaringType != y.DeclaringType) return false;

        // Compare method name
        if (!string.Equals(x.Name, y.Name, StringComparison.Ordinal)) return false;

        // Compare number of generic parameters
        if (x.IsGenericMethod != y.IsGenericMethod) return false;

        if (x.IsGenericMethod && x.GetGenericArguments().Length != y.GetGenericArguments().Length)
        {
            return false;
        }

        // Compare method parameters, ignoring generic type parameters
        var xParams = x.GetParameters().AsSpan();
        var yParams = y.GetParameters().AsSpan();
        int xLength = xParams.Length;

        if (xLength != yParams.Length) return false;

        for (int i = 0; i < xLength; i++)
        {
            if (!AreParametersEquivalent(xParams[i].ParameterType, yParams[i].ParameterType))
                return false;
        }

        return true;
    }

    private static bool AreParametersEquivalent(Type x, Type y)
    {
        // Treat generic type arguments as equal
        if (x.IsGenericParameter && y.IsGenericParameter) return true;

        // Compare the generic type definition if present
        if (x.IsGenericType && y.IsGenericType)
            return x.GetGenericTypeDefinition() == y.GetGenericTypeDefinition();

        return x == y;
    }

    public int GetHashCode(MethodInfo method)
    {
        if (method.IsGenericMethod)
        {
            return method.GetGenericMethodDefinition().GetHashCode();
        }
        return method.GetHashCode();
    }
}