using System.Reflection;
using Blido.Core.Transaction;

namespace Blido.Core;

public abstract class IndexedDbContext
{
    private readonly IObjectStoreFactory _transactionProviderFactory;
    public IndexedDbDatabase Database { get; }
    protected IndexedDbContext(IObjectStoreFactory transactionProviderFactory)
    {
        ArgumentNullException.ThrowIfNull(transactionProviderFactory);
        _transactionProviderFactory = transactionProviderFactory;
        Database = new IndexedDbDatabase(this, _transactionProviderFactory.JsRuntime);
        InitializeProperties();
    }

    public ObjectStore<TEntity> ObjectStore<TEntity>() where TEntity : class
    {
        return (ObjectStore<TEntity>)ObjectStore(typeof(TEntity));
    }

    public object ObjectStore(Type type)
    {
        return _transactionProviderFactory.GetObjectStore(Database, type);
    }

    private void InitializeProperties()
    {
        var type = GetType();
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).AsSpan();

        foreach (var propertyInfo in properties)
        {
            var propertyType = propertyInfo.PropertyType;
            if (!propertyType.IsGenericType || !propertyInfo.CanWrite) continue;

            var genericType = propertyType.GetGenericTypeDefinition();
            if (genericType != typeof(ObjectStore<>)) continue;

            var objectStore = ObjectStore(propertyType.GetGenericArguments()[0]);
            propertyInfo.SetValue(this, objectStore);
        }
    }
}