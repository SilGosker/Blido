using Blido.Core.Transaction.JsExpression;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction;

public class ObjectStoreFactory : IObjectStoreFactory
{
    private readonly IJSRuntime _jsRuntime;
    IJSRuntime IObjectStoreFactory.JsRuntime => _jsRuntime;
    private readonly IExpressionBuilder _expressionBuilder;
    public ObjectStoreFactory(IExpressionBuilder expressionBuilder, IJSRuntime jsRuntime)
    {
        ArgumentNullException.ThrowIfNull(expressionBuilder);
        ArgumentNullException.ThrowIfNull(jsRuntime);
        _expressionBuilder = expressionBuilder;
        _jsRuntime = jsRuntime;
    }

    object IObjectStoreFactory.GetObjectStore(IndexedDbDatabase database, Type entityType)
    {
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(entityType);

        var transactionProviderType = typeof(QueryProvider<>).MakeGenericType(entityType);
        var transactionProvider = (IQueryProvider)Activator.CreateInstance(transactionProviderType, _jsRuntime, _expressionBuilder)!;
        
        var objectStoreType = typeof(ObjectStore<>).MakeGenericType(entityType);
        var objectStore = Activator.CreateInstance(objectStoreType, database, transactionProvider)!;
        
        transactionProvider.SetObjectStore(objectStore);
        
        return objectStore;
    }
}