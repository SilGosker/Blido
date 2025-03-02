using System.Linq.Expressions;
using System.Numerics;

namespace Blido.Core.Transaction;

public interface IObjectStore<TEntity> where TEntity : class
{
    public Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default);
    public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity[]> ToArrayAsync(CancellationToken cancellationToken = default);
    public Task<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity> FirstAsync(CancellationToken cancellationToken = default);
    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default);
    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity> LastAsync(CancellationToken cancellationToken = default);
    public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity?> LastOrDefaultAsync(CancellationToken cancellationToken = default);
    public Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity> SingleAsync(CancellationToken cancellationToken = default);
    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default);
    public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TEntity?> FindAsync(object identifiers, CancellationToken cancellationToken = default);
    public Task<int> CountAsync(CancellationToken cancellationToken = default);
    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<long> LongCountAsync(CancellationToken cancellationToken = default);
    public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<bool> AnyAsync(CancellationToken cancellationToken = default);
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    public Task<TNumber> SumAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
    public Task<TNumber> AverageAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
    public Task<TNumber> MinAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
    public Task<TNumber> MaxAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>;
}