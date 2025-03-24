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

    public ValueTask<TEntity> FirstAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity>(nameof(FirstAsync), cancellationToken);
    }

    public ValueTask<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity>(nameof(FirstAsync), cancellationToken);
    }

    public ValueTask<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity?>(nameof(FirstOrDefaultAsync), cancellationToken);
    }

    public ValueTask<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity?>(nameof(FirstOrDefaultAsync), cancellationToken);
    }

    public ValueTask<TEntity> LastAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity>(nameof(LastAsync), cancellationToken);
    }

    public ValueTask<TEntity> LastAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity>(nameof(LastAsync), cancellationToken);
    }

    public ValueTask<TEntity?> LastOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity?>(nameof(LastOrDefaultAsync), cancellationToken);
    }

    public ValueTask<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity?>(nameof(LastOrDefaultAsync), cancellationToken);
    }

    public ValueTask<TEntity> SingleAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity>(nameof(SingleAsync), cancellationToken);
    }

    public ValueTask<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity>(nameof(SingleAsync), cancellationToken);
    }

    public ValueTask<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity?>(nameof(SingleOrDefaultAsync), cancellationToken);
    }

    public ValueTask<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity?>(nameof(SingleOrDefaultAsync), cancellationToken);
    }

    public ValueTask<TEntity?> FindAsync(object identifiers, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(identifiers);
        return _provider.ExecuteAsync<TEntity?>(nameof(FindAsync), identifiers, cancellationToken);
    }

    public ValueTask<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<int>(nameof(CountAsync), cancellationToken);
    }

    public ValueTask<int> CountAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<int>(nameof(CountAsync), cancellationToken);
    }

    public ValueTask<long> LongCountAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<long>(nameof(LongCountAsync), cancellationToken);
    }

    public ValueTask<long> LongCountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<long>(nameof(LongCountAsync), cancellationToken);
    }

    public ValueTask<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<bool>(nameof(AnyAsync), cancellationToken);
    }

    public ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<bool>(nameof(AnyAsync), cancellationToken);
    }

    public ValueTask<bool> AllAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<bool>(nameof(AllAsync), cancellationToken);
    }

    public ValueTask<TNumber> SumAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression,
        CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(SumAsync), expression, cancellationToken);
    }

    public ValueTask<TNumber> AverageAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression,
        CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(AverageAsync), expression, cancellationToken);
    }

    public ValueTask<TNumber> MinAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(MinAsync), expression, cancellationToken);
    }

    public ValueTask<TNumber> MaxAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression, CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.ExecuteAsync<TNumber>(nameof(MaxAsync), expression, cancellationToken);
    }

    public ValueTask<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<List<TEntity>>(nameof(ToListAsync), cancellationToken);
    }

    public ValueTask<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<List<TEntity>>(nameof(ToListAsync), cancellationToken);
    }

    public ValueTask<TEntity[]> ToArrayAsync(CancellationToken cancellationToken = default)
    {
        return _provider.ExecuteAsync<TEntity[]>(nameof(ToArrayAsync), cancellationToken);
    }

    public ValueTask<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.ExecuteAsync<TEntity[]>(nameof(ToArrayAsync), cancellationToken);
    }
}