using System.Linq.Expressions;

namespace Blido.Core.Transaction;

public interface ITransactionFilterProvider<TEntity, out TSelf>
    where TSelf : ITransactionFilterProvider<TEntity, TSelf>
    where TEntity : class
{
    public TSelf Where(Expression<Func<TEntity, bool>> expression);
}