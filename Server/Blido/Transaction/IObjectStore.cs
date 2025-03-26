using Blido.Core.Transaction.Configuration;
using System.Linq.Expressions;
using System.Numerics;

namespace Blido.Core.Transaction;

public interface IObjectStore<TEntity>
    : ITransactionFilterProvider<TEntity, IObjectStore<TEntity>> where TEntity : class
{
    public ValueTask<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default);
    public ValueTask<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity[]> ToArrayAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity> FirstAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity> LastAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity> LastAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> LastOrDefaultAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity> SingleAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> FindAsync(object identifiers, CancellationToken cancellationToken = default);
    public ValueTask<int> CountAsync(CancellationToken cancellationToken = default);
    public ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<long> LongCountAsync(CancellationToken cancellationToken = default);
    public ValueTask<long> LongCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default);
    public ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<bool> AllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public ValueTask<TNumber> SumAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
    public ValueTask<TNumber> AverageAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
    public ValueTask<TNumber> MinAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
    public ValueTask<TNumber> MaxAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
}