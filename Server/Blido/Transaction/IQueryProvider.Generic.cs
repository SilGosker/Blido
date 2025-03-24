using Blido.Core.Transaction.Configuration;
using System.Linq.Expressions;

namespace Blido.Core.Transaction;

public interface ITransactionProvider<TEntity>
    : IQueryProvider, ITransactionFilterProvider<TEntity, ITransactionProvider<TEntity>>
    where TEntity : class
{
    public ValueTask<TResult> ExecuteAsync<TResult>(string methodName, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);
    public ValueTask<TResult> ExecuteAsync<TResult>(string methodName, object identifiers, CancellationToken cancellationToken = default);
    public ValueTask<TResult> ExecuteAsync<TResult>(string methodName, CancellationToken cancellationToken = default);
}