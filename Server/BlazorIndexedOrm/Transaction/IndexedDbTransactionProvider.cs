﻿using System.Linq.Expressions;
using BlazorIndexedOrm.Core.Transaction.Materialization;
using Microsoft.JSInterop;

namespace BlazorIndexedOrm.Core.Transaction;

public class IndexedDbTransactionProvider<TEntity> : ITransactionProvider<TEntity> where TEntity : class
{
    private readonly TransactionConditions<TEntity> _transactionConditions = new();
    private readonly IJSRuntime _jsRuntime;
    private readonly IndexedDbDatabase _database;
    public IndexedDbTransactionProvider(IJSRuntime jsRuntime, IndexedDbDatabase database)
    {
        ArgumentNullException.ThrowIfNull(jsRuntime);
        ArgumentNullException.ThrowIfNull(database);
        _jsRuntime = jsRuntime;
        _database = database;
    }

    public Task<TResult> Execute<TResult>(string methodName, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(methodName);
        return Execute<TResult>(methodName, null, cancellationToken);
    }

    public Task<TResult> Execute<TResult>(string methodName, Expression<Func<TEntity, TResult>>? selector, CancellationToken cancellationToken = default)
    {
        var materializer = new TransactionMaterializerFactory<TEntity>(_jsRuntime, _transactionConditions, _database)
            .GetMaterializer<TResult>(methodName);
        
        if (selector is null)
        {
            return materializer.ExecuteAsync(cancellationToken);
        }
        throw new NotImplementedException();
    }

    public ITransactionProvider<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _transactionConditions.AddCondition(expression);
        return this;
    }
}