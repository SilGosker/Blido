using BlazorIndexedOrm.Core.Transaction.JsExpression;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public readonly struct TransactionMaterializerFactory<TEntity> where TEntity : class
{
    private readonly IJSRuntime _jsRuntime;
    private readonly TransactionConditions<TEntity> _conditions;
    private readonly IndexedDbDatabase _database;
    private readonly IExpressionBuilder _jsExpressionBuilder;
    public TransactionMaterializerFactory(IJSRuntime jsRuntime, TransactionConditions<TEntity> conditions, IndexedDbDatabase database, IExpressionBuilder jsExpressionBuilder)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(conditions);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(jsExpressionBuilder);
        _jsRuntime = jsRuntime;
        _conditions = conditions;
        _database = database;
        _jsExpressionBuilder = jsExpressionBuilder;
    }

    public ITransactionMaterializer<TResult> GetMaterializer<TResult>(string methodName)
    {
        object? transactionProvider = methodName switch
        {
            nameof(ITransactionMaterializationProvider<TEntity>.FirstOrDefaultAsync) =>
               new FirstOrDefaultTransactionMaterializer<TEntity>(_jsRuntime, _jsExpressionBuilder, _conditions,  _database, NameResolver.ResolveObjectStoreName<TEntity>()),
            _ => null
        };

        if (transactionProvider is ITransactionMaterializer<TResult> aggregator)
            return aggregator;

        throw new NotImplementedException();
    }
}