using System.Linq.Expressions;
using System.Numerics;
using BlazorIndexedOrm.Core.ObjectStore;
using BlazorIndexedOrm.Core.Transaction;
using BlazorIndexedOrm.Core.Transaction.Materialization;

namespace BlazorIndexedOrm.Core;

public class ObjectStoreSet<TEntity> :
    ITransactionFilterProvider<TEntity, ObjectStoreSet<TEntity>>,
    ITransactionMaterializationProvider<TEntity> where TEntity : class
{
    private readonly ITransactionProvider<TEntity> _provider;
    public string Name { get; }
    public ObjectStoreSet(ITransactionProvider<TEntity> provider)
    {
        ArgumentNullException.ThrowIfNull(provider);

        _provider = provider;
        Name = ObjectStoreNameResolver.Resolve<TEntity>();
    }

    public ObjectStoreSet<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        _provider.Where(expression);
        return this;
    }

    public Task<TEntity> FirstAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<TEntity>(nameof(FirstAsync), cancellationToken);
    }

    public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<TEntity>(nameof(FirstAsync), cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<TEntity?>(nameof(FirstOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<TEntity?>(nameof(FirstOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity> SingleAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<TEntity>(nameof(SingleAsync), cancellationToken);
    }

    public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<TEntity>(nameof(SingleAsync), cancellationToken);
    }

    public Task<TEntity?> SingleOrDefaultAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<TEntity?>(nameof(SingleOrDefaultAsync), cancellationToken);
    }

    public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<TEntity?>(nameof(SingleOrDefaultAsync), cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<int>(nameof(CountAsync), cancellationToken);
    }

    public Task<int> CountAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<int>(nameof(CountAsync), cancellationToken);
    }

    public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<bool>(nameof(AnyAsync), cancellationToken);
    }

    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<bool>(nameof(AnyAsync), cancellationToken);
    }

    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<bool>(nameof(AllAsync), cancellationToken);
    }

    public Task<TNumber> SumAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression,
        CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.Execute<TNumber>(nameof(SumAsync), expression, cancellationToken);
    }

    public Task<TNumber> AverageAsync<TNumber>(Expression<Func<TEntity, TNumber>> expression,
        CancellationToken cancellationToken = default) where TNumber : INumber<TNumber>
    {
        ArgumentNullException.ThrowIfNull(expression);
        return _provider.Execute<TNumber>(nameof(AverageAsync), expression, cancellationToken);
    }

    public Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<List<TEntity>>(nameof(ToListAsync), cancellationToken);
    }

    public Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<List<TEntity>>(nameof(ToListAsync), cancellationToken);
    }

    public Task<TEntity[]> ToArrayAsync(CancellationToken cancellationToken = default)
    {
        return _provider.Execute<TEntity[]>(nameof(ToArrayAsync), cancellationToken);
    }

    public Task<TEntity[]> ToArrayAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(expression);
        Where(expression);
        return _provider.Execute<TEntity[]>(nameof(ToArrayAsync), cancellationToken);
    }
}