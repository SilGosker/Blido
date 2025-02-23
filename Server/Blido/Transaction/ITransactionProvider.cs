using System.Linq.Expressions;

namespace Blido.Core.Transaction;

public interface ITransactionProvider<TEntity> : ITransactionFilterProvider<TEntity, ITransactionProvider<TEntity>>
    where TEntity : class
{
    public Task<TResult> Execute<TResult>(string methodName, CancellationToken cancellationToken = default);
    public Task<TResult> Execute<TResult>(string methodName, Expression<Func<TEntity, TResult>>? selector, CancellationToken cancellationToken = default);
}