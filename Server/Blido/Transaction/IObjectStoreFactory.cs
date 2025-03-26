using Microsoft.JSInterop;

namespace Blido.Core.Transaction;

public interface IObjectStoreFactory
{
    internal IJSRuntime JsRuntime { get; }
    internal object GetObjectStore(IndexedDbContext context, Type entityType);
}