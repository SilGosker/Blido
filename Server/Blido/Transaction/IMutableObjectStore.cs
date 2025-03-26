namespace Blido.Core.Transaction;

public interface IMutableObjectStore<in TEntity> where TEntity : class
{
    public ValueTask InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    public ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public ValueTask DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}