using System.Reflection;
using BlazorIndexedOrm.Core.Transaction;

namespace BlazorIndexedOrm.Core;

public abstract class IndexedDbDatabase
{
    private readonly IIndexedDbTransactionProviderFactory _transactionProviderFactory;
    private readonly ValueTask<ulong> _versionTask;
    public string Name { get; }
    public ulong Version => _versionTask.Result;
    public ValueTask<ulong> GetVersionAsync() => _versionTask;

    protected IndexedDbDatabase(IIndexedDbTransactionProviderFactory transactionProviderFactory)
    {
        ArgumentNullException.ThrowIfNull(transactionProviderFactory);
        Name = NameResolver.ResolveIndexedDbStoreName(GetType());
        _transactionProviderFactory = transactionProviderFactory;
        _versionTask = _transactionProviderFactory.JsRuntime.InvokeAsync<ulong>(JsMethodNameConstants.GetVersion, new[] { this.Name });
        transactionProviderFactory.Database = this;
        InitializeProperties();
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
            if (genericType != typeof(ObjectStoreSet<>)) continue;

            var objectStoreType = propertyType.GenericTypeArguments[0];
            var transactionProvider = _transactionProviderFactory.GetIndexedDbTransactionProvider(objectStoreType);
            var objectStore = Activator.CreateInstance(propertyType, transactionProvider);

            propertyInfo.SetValue(this, objectStore);
        }
    }
}