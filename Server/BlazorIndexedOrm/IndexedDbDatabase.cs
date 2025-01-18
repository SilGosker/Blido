using System.Reflection;
using BlazorIndexedOrm.Core.Transaction;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core;

public abstract class IndexedDbDatabase
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ValueTask<ulong> _versionTask;
    public string Name { get; }
    public ulong Version => _versionTask.Result;
    public ValueTask<ulong> GetVersionAsync() => _versionTask;

    protected IndexedDbDatabase(IJSRuntime jsRuntime)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        _jsRuntime = jsRuntime;
        Name = FindDatabaseName();
        _versionTask = _jsRuntime.InvokeAsync<ulong>(JsMethodNameConstants.GetVersion, Name);
        InitializeObjectStores();
    }

    private string FindDatabaseName()
    {
        var type = GetType();
        var databaseNameAttribute = type.GetCustomAttribute<IndexedDbDatabaseNameAttribute>();

        if (databaseNameAttribute is not null)
        {
            return databaseNameAttribute.Name;
        }

        return type.Name;
    }

    private void InitializeObjectStores()
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
            var transactionProvider = ResolveTransactionProvider(objectStoreType);
            var objectStore = Activator.CreateInstance(propertyType, transactionProvider);

            propertyInfo.SetValue(this, objectStore);
        }
    }

    private object ResolveTransactionProvider(Type genericType)
    {
        var transactionProviderType = typeof(IndexedDbTransactionProvider<>).MakeGenericType(genericType);
        return Activator.CreateInstance(transactionProviderType, this._jsRuntime, this)!;
    }
}