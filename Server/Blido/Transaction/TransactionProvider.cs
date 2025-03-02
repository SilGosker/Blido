using System.Linq.Expressions;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.Materialization;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction;

public class TransactionProvider<TEntity> : ITransactionProvider<TEntity> where TEntity : class
{
    private readonly TransactionConditions<TEntity> _transactionConditions = new();
    private readonly IJSRuntime _jsRuntime;
    private readonly IndexedDbDatabase _database;
    private readonly IExpressionBuilder _jsExpressionBuilder;
    private ObjectStore<TEntity> _objectStore = null!;

    public TransactionProvider(IJSRuntime jsRuntime,
        IndexedDbDatabase database,
        IExpressionBuilder jsExpressionBuilder)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(database);
        ArgumentNullException.ThrowIfNull(jsExpressionBuilder);
        _jsRuntime = jsRuntime;
        _database = database;
        _jsExpressionBuilder = jsExpressionBuilder;
    }

    void ITransactionProvider.SetObjectStore(object objectStore)
    {
        _objectStore = (ObjectStore<TEntity>)objectStore;
    }

    public ITransactionProvider<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        _transactionConditions.AddCondition(expression);
        return this;
    }

    public Task<TResult> ExecuteAsync<TResult>(string methodName, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<TResult> ExecuteAsync<TResult>(string methodName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        if (JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? jsMethodName))
        {
            return await Materializer.ExecuteAsync<TEntity, TResult>(_jsRuntime, _objectStore, _jsExpressionBuilder,
                _transactionConditions, jsMethodName, cancellationToken);
        }

        throw new ArgumentException($"Method name '{methodName}' is not supported.");
    }

    public async Task<TEntity> ExecuteAsync(string methodName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        if (JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? jsMethodName))
        {
            return await Materializer.ExecuteAsync<TEntity, TEntity>(_jsRuntime, _objectStore, _jsExpressionBuilder,
                _transactionConditions, jsMethodName, cancellationToken);
        }

        throw new ArgumentException($"Method name '{methodName}' is not supported.");
    }
}