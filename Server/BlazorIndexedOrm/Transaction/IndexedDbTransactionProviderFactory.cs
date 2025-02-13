using BlazorIndexedOrm.Core.Transaction.JsExpression;
using Microsoft.JSInterop;
using System.Reflection;

namespace BlazorIndexedOrm.Core.Transaction;

public class IndexedDbTransactionProviderFactory : IIndexedDbTransactionProviderFactory
{
    private readonly IJSRuntime _jsRuntime;
    IJSRuntime IIndexedDbTransactionProviderFactory.JsRuntime => _jsRuntime;
    private IndexedDbDatabase? _database;
    private readonly IExpressionBuilder _expressionBuilder;
    public IndexedDbTransactionProviderFactory(IExpressionBuilder expressionBuilder, IJSRuntime jsRuntime)
    {
        _expressionBuilder = expressionBuilder;
        _jsRuntime = jsRuntime;
    }

    IndexedDbDatabase IIndexedDbTransactionProviderFactory.Database
    {
        set
        {
            _database = value;
        }
    }

    public object GetIndexedDbTransactionProvider(Type entityType)
    {
        var transactionProviderType = typeof(IndexedDbTransactionProvider<>).MakeGenericType(entityType);
        return Activator.CreateInstance(transactionProviderType, _jsRuntime, _database, _expressionBuilder)!;
    }

    public IndexedDbTransactionProvider<TEntity> GetIndexedDbTransactionProvider<TEntity>() where TEntity : class
    {
        return (IndexedDbTransactionProvider<TEntity>)GetIndexedDbTransactionProvider(typeof(TEntity));
    }
}