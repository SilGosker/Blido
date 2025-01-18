using BlazorIndexedOrm.Core.ObjectStore;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public readonly struct TransactionMaterializerFactory<TEntity> where TEntity : class
{
    private readonly IJSRuntime _jsRuntime;
    private readonly TransactionConditions<TEntity> _conditions;
    private readonly IndexedDbDatabase _database;
    public TransactionMaterializerFactory(IJSRuntime jsRuntime, TransactionConditions<TEntity> conditions, IndexedDbDatabase database)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(conditions);
        ArgumentNullException.ThrowIfNull(database);
        _jsRuntime = jsRuntime;
        _conditions = conditions;
        _database = database;
    }

    public ITransactionMaterializer<TResult> GetMaterializer<TResult>(string methodName)
    {
        object? transactionProvider = methodName switch
        {
            nameof(ITransactionMaterializationProvider<TEntity>.FirstOrDefaultAsync) =>
               new FirstOrDefaultTransactionMaterializer<TEntity>(_jsRuntime, _conditions, _database, ObjectStoreNameResolver.Resolve<TEntity>()),
            _ => null
        };

        if (transactionProvider is ITransactionMaterializer<TResult> aggregator)
            return aggregator;

        throw new NotImplementedException();
    }
}