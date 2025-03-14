using System.Linq.Expressions;
using System.Numerics;
using Blido.Core.Transaction;
using Blido.Core.Transaction.Configuration;

namespace Blido.Core;

public class ObjectStore<TEntity> :
    ITransactionFilterProvider<TEntity, ObjectStore<TEntity>>,
    IObjectStore<TEntity> where TEntity : class
{
    private readonly ITransactionProvider<TEntity> _provider;
    public string Name { get; }
    public IndexedDbDatabase Database { get; }

    public ObjectStore(IndexedDbDatabase database, ITransactionProvider<TEntity> provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(database);
        Database = database;
        _provider = provider;
        Name = NameResolver.ResolveObjectStoreName<TEntity>();
    }

    public ObjectStore<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _provider.Where(expression);
        return this;
    }

    public Task<TEntity> FirstAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity>(nameof(FirstAsync), cancellationToken);
    }

    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity>(nameof(FirstAsync), cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity?>(nameof(FirstOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity?>(nameof(FirstOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity> LastAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity>(nameof(LastAsync), cancellationToken);
    }

    public Task<TEntity> LastAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity>(nameof(LastAsync), cancellationToken);
    }

    public Task<TEntity?> LastOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity?>(nameof(LastOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity?>(nameof(LastOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity> SingleAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity>(nameof(SingleAsync), cancellationToken);
    }

    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity>(nameof(SingleAsync), cancellationToken);
    }

    public Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity?>(nameof(SingleOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity?>(nameof(SingleOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity?> FindAsync(object identifiers, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifiers);
        return _provider.ExecuteAsync<TEntity?>(nameof(FindAsync), identifiers, cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<int>(nameof(CountAsync), cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<int>(nameof(CountAsync), cancellationToken);
    }

    public Task<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<long>(nameof(LongCountAsync), cancellationToken);
    }

    public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<long>(nameof(LongCountAsync), cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<bool>(nameof(AnyAsync), cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<bool>(nameof(AnyAsync), cancellationToken);
    }

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<bool>(nameof(AllAsync), cancellationToken);
    }

    public Task<TNumber> SumAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression,
        CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(SumAsync), expression, cancellationToken);
    }

    public Task<TNumber> AverageAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression,
        CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(AverageAsync), expression, cancellationToken);
    }

    public Task<TNumber> MinAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(MinAsync), expression, cancellationToken);
    }

    public Task<TNumber> MaxAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(MaxAsync), expression, cancellationToken);
    }

    public Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<List<TEntity>>(nameof(ToListAsync), cancellationToken);
    }

    public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<List<TEntity>>(nameof(ToListAsync), cancellationToken);
    }

    public Task<TEntity[]> ToArrayAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity[]>(nameof(ToArrayAsync), cancellationToken);
    }

    public Task<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity[]>(nameof(ToArrayAsync), cancellationToken);
    }
}