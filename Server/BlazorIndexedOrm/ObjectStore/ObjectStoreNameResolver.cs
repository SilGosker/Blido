using System.Reflection;

namespace BlazorIndexedOrm.Core.ObjectStore;

public static class ObjectStoreNameResolver
{
    public static string Resolve<TEntity>() where TEntity : class
    {
        var type = typeof(TEntity);
        var attribute = type.GetCustomAttribute<ObjectStoreNameAttribute>();

        if (attribute is not null)
        {
            return attribute.Name;
        }

        return type.Name;
    }
}