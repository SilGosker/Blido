using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction;

public interface IIndexedDbTransactionProviderFactory
{
    internal IJSRuntime JsRuntime { get; }
    internal IndexedDbDatabase Database { set; }
    public object GetIndexedDbTransactionProvider(Type entityType);
    public IndexedDbTransactionProvider<TEntity> GetIndexedDbTransactionProvider<TEntity>()
        where TEntity : class;
}