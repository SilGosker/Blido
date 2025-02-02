using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public readonly struct FirstOrDefaultTransactionMaterializer<TEntity> : ITransactionMaterializer<TEntity>
    where TEntity : class
{
    private readonly IJSRuntime _jsRuntime;
    private readonly TransactionConditions<TEntity> _conditions;
    private readonly IndexedDbDatabase _database;
    private readonly string _objectStore;
    public FirstOrDefaultTransactionMaterializer(IJSRuntime jsRuntime, TransactionConditions<TEntity> conditions, IndexedDbDatabase database, string objectStore)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(conditions);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(objectStore);
        _jsRuntime = jsRuntime;
        _conditions = conditions;
        _database = database;
        _objectStore = objectStore;
    }

    public async Task<TEntity> ExecuteAsync(CancellationToken cancellationToken = new())
    {
        var version = await _database.GetVersionAsync();

        DotNetObjectReference<TransactionConditions<TEntity>>? transactionReference = null;

        if (_conditions.HasConditions)
        {
            transactionReference = DotNetObjectReference.Create(_conditions);
        }

        var result = await _jsRuntime.InvokeAsync<TEntity>(JsMethodNameConstants.FirstOrDefault, cancellationToken, _database.Name, _objectStore, version, transactionReference);
        return result;
    }
}