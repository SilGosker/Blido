using System.Reflection;
using System.Runtime.CompilerServices;

namespace Blido.Core.Transaction.JsExpression.MemberTranslation;

public class MemberInfoEqualityComparer : IEqualityComparer<MemberInfo>
{
    public bool Equals(MemberInfo? x, MemberInfo? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null) return false;
        if (y is null) return false;
        if ((x is not PropertyInfo and not FieldInfo)
            || (y is not PropertyInfo and not FieldInfo))
        {
            return false;
        }

        var xType = x.DeclaringType;
        var yType = y.DeclaringType;

        if (xType == null && yType == null)
        {
            return x.Equals(y);
        }

        if (xType == null || yType == null)
        {
            return false;
        }

        return  IsAnonymousType(xType) == IsAnonymousType(yType);
    }

    public int GetHashCode(MemberInfo obj)
    {
        var type = obj.DeclaringType;

        if (type == null)
        {
            return obj.GetHashCode();
        }

        var isAnonymous = IsAnonymousType(type);
        if (isAnonymous)
        {
            return 1;
        }

        return obj.GetHashCode();
    }

    internal bool IsAnonymousType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
            && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
            && type.Attributes.HasFlag(TypeAttributes.NotPublic);
    }
}