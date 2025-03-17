using System.Linq.Expressions;
using Blido.Core.Transaction.Configuration;
using Blido.Core.Transaction.JsExpression;
using Blido.Core.Transaction.Materialization;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction;

public class QueryProvider<TEntity> : ITransactionProvider<TEntity> where TEntity : class
{
    private readonly TransactionConditions<TEntity> _transactionConditions = new();
    private readonly IJSRuntime _jsRuntime;
    private readonly IExpressionBuilder _jsExpressionBuilder;
    private ObjectStore<TEntity> _objectStore = null!;

    public QueryProvider(IJSRuntime jsRuntime, IExpressionBuilder jsExpressionBuilder)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(jsExpressionBuilder);
        _jsRuntime = jsRuntime;
        _jsExpressionBuilder = jsExpressionBuilder;
    }

    void IQueryProvider.SetObjectStore(object objectStore)
    {
        _objectStore = (ObjectStore<TEntity>)objectStore;
    }

    public ITransactionProvider<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _transactionConditions.AddCondition(expression);
        return this;
    }

    public async Task<TResult> ExecuteAsync<TResult>(string methodName, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        ArgumentNullException.ThrowIfNull(selector);

        if (JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? jsMethodName))
        {
            var result = await SelectorMaterializer.ExecuteAsync<TEntity, TResult>(_jsRuntime, _objectStore,
                _jsExpressionBuilder,
                _transactionConditions, selector, jsMethodName, cancellationToken);
            _transactionConditions.Clear();
            return result;
        }

        throw new ArgumentException($"Method name '{methodName}' is not supported.");
    }

    public async Task<TResult> ExecuteAsync<TResult>(string methodName, object identifiers, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        ArgumentNullException.ThrowIfNull(identifiers);

        if (JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? jsMethodName))
        {
            var result = await IdentifierMaterializer.ExecuteAsync<TEntity, TResult>(_jsRuntime, _objectStore, _jsExpressionBuilder,
                identifiers, jsMethodName, cancellationToken);
            _transactionConditions.Clear();
            return result;
        }

        throw new ArgumentException($"Method name '{methodName}' is not supported.");
    }

    public async Task<TResult> ExecuteAsync<TResult>(string methodName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);

        if (JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? jsMethodName))
        {
            var result = await Materializer.ExecuteAsync<TEntity, TResult>(_jsRuntime, _objectStore, _jsExpressionBuilder,
                _transactionConditions, jsMethodName, cancellationToken);
            _transactionConditions.Clear();
            return result;
        }

        throw new ArgumentException($"Method name '{methodName}' is not supported.");
    }

    public async Task ExecuteAsync(string methodName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        if (JsMethodNames.MaterializerMethodNames.TryGetValue(methodName, out string? jsMethodName))
        {
            await Materializer.ExecuteAsync<TEntity, object?>(_jsRuntime, _objectStore, _jsExpressionBuilder,
                _transactionConditions, jsMethodName, cancellationToken);
            return;
        }
        throw new ArgumentException($"Method name '{methodName}' is not supported.");
    }
}