namespace Blido.Core.Transaction.Mutation;

public class MutationContext<TEntity> : IAsyncDisposable
    where TEntity : class 
{
    private readonly ITransactionProvider<TEntity> _queryProvider;
    private readonly List<Type> _pipelineTypes;
    public MutationContext(ITransactionProvider<TEntity> queryProvider, List<Type> pipelineTypes)
    {
        ArgumentNullException.ThrowIfNull(queryProvider);
        ArgumentNullException.ThrowIfNull(pipelineTypes);
        _queryProvider = queryProvider;
        _pipelineTypes = pipelineTypes;
    }

    public List<MutationEntityContext> Entities { get; } = new();

    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
    {

    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        return SaveChangesAsync();
    }
}