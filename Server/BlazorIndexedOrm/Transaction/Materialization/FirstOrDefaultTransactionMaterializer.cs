using BlazorIndexedOrm.Core.Transaction.JsExpression;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction.Materialization;

public readonly struct FirstOrDefaultTransactionMaterializer<TEntity> : ITransactionMaterializer<TEntity>
    where TEntity : class
{
    private readonly IJSRuntime _jsRuntime;
    private readonly TransactionConditions<TEntity> _conditions;
    private readonly IndexedDbDatabase _database;
    private readonly string _objectStore;
    private readonly IExpressionBuilder _expressionBuilder;
    public FirstOrDefaultTransactionMaterializer(IJSRuntime jsRuntime,
        IExpressionBuilder expressionBuilder,
        TransactionConditions<TEntity> conditions,
        IndexedDbDatabase database,
        string objectStore)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(conditions);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(objectStore);
        ArgumentNullException.ThrowIfNull(expressionBuilder);
        _jsRuntime = jsRuntime;
        _conditions = conditions;
        _database = database;
        _objectStore = objectStore;
        _expressionBuilder = expressionBuilder;
    }

    public async Task<TEntity> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var version = await _database.GetVersionAsync();
        string[] jsFunctions = new string[_conditions.Count];

        if (_conditions.HasConditions)
        {
            for (var i = 0; i < _conditions.Count; i++)
            {
                var expression = _conditions[i]!;
                
                jsFunctions[i] = _expressionBuilder.ProcessExpression(expression);
            }
        }

        var result = await _jsRuntime.InvokeAsync<TEntity>(JsMethodNameConstants.FirstOrDefault, cancellationToken, _database.Name, _objectStore, version, jsFunctions);
        return result;
    }
}