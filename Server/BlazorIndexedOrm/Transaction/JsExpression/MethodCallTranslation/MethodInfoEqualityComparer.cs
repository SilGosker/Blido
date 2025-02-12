using System.Reflection;
using System.Runtime.CompilerServices;

namespace BlazorIndexedOrm.Core.Transaction.JsExpression.MethodCallTranslation;

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

    public int GetHashCode(MethodInfo obj)
    {
        HashCode hashCode = new();
        hashCode.Add(obj.CustomAttributes);
        hashCode.Add(obj.DeclaringType);
        hashCode.Add(obj.IsCollectible);
        hashCode.Add(obj.MetadataToken);
        hashCode.Add(obj.Module);
        hashCode.Add(obj.Name);
        hashCode.Add(obj.ReflectedType);
        hashCode.Add((int)obj.Attributes);
        hashCode.Add((int)obj.CallingConvention);
        hashCode.Add(obj.ContainsGenericParameters);
        hashCode.Add(obj.IsAbstract);
        hashCode.Add(obj.IsAssembly);
        hashCode.Add(obj.IsConstructedGenericMethod);
        hashCode.Add(obj.IsConstructor);
        hashCode.Add(obj.IsFamily);
        hashCode.Add(obj.IsFamilyAndAssembly);
        hashCode.Add(obj.IsFamilyOrAssembly);
        hashCode.Add(obj.IsFinal);
        hashCode.Add(obj.IsGenericMethod);
        hashCode.Add(obj.IsGenericMethodDefinition);
        hashCode.Add(obj.IsHideBySig);
        hashCode.Add(obj.IsPrivate);
        hashCode.Add(obj.IsPublic);
        hashCode.Add(obj.IsSecurityCritical);
        hashCode.Add(obj.IsSecuritySafeCritical);
        hashCode.Add(obj.IsSecurityTransparent);
        hashCode.Add(obj.IsSpecialName);
        hashCode.Add(obj.IsStatic);
        hashCode.Add(obj.IsVirtual);
        hashCode.Add(obj.MethodHandle);
        hashCode.Add((int)obj.MethodImplementationFlags);
        hashCode.Add((int)obj.MemberType);
        hashCode.Add(obj.ReturnParameter);
        hashCode.Add(obj.ReturnType);
        hashCode.Add(obj.ReturnTypeCustomAttributes);
        return hashCode.ToHashCode();
    }
}