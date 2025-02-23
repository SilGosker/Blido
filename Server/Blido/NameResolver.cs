using Blido.Core.Attributes;
using System.Reflection;

namespace Blido.Core;

public static class NameResolver
{
    public static string ResolveObjectStoreName<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        var attribute = type.GetCustomAttribute<ObjectStoreNameAttribute>();

        if (attribute is not null)
        {
            return attribute.Name;
        }

        return type.Name;
    }

    public static string ResolveObjectFieldName(MemberInfo memberInfo)
    {
        var attribute = memberInfo.GetCustomAttribute<FieldNameAttribute>();
        if (attribute is null)
        {
            return memberInfo.Name;
        }

        return attribute.Name;
    }

    public static string ResolveIndexedDbStoreName(Type type)
    {
        var attribute = type.GetCustomAttribute<IndexedDbDatabaseNameAttribute>();
        if (attribute is not null)
        {
            return attribute.Name;
        }

        return type.Name;
    }
}