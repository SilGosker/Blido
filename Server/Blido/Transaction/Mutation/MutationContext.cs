using Blido.Core.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Blido.Core.Transaction.Mutation;

public class MutationContext: IAsyncDisposable
{
    private readonly List<Type> _pipelineTypes;
    private readonly IServiceScope _serviceScope;
    private readonly List<MutationEntityContext> _entities = new();
    private int _index = -1;
    private readonly IndexedDbContext _context;
    public MutationContext(List<Type> pipelineTypes, IServiceScope serviceScope, IndexedDbContext context)
    {
        ArgumentNullException.ThrowIfNull(pipelineTypes);
        ArgumentNullException.ThrowIfNull(serviceScope);
        ArgumentNullException.ThrowIfNull(context);
        _pipelineTypes = pipelineTypes;
        _serviceScope = serviceScope;
        _context = context;
    }

    public IReadOnlyList<MutationEntityContext> Entities => _entities.AsReadOnly();
    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _index++;
        if (_index < _pipelineTypes.Count)
        {
            var middlewareType = _pipelineTypes[_index];
            var middleware = (IBlidoMiddleware?)_serviceScope.ServiceProvider.GetService(middlewareType)
                             ?? (IBlidoMiddleware)ActivatorUtilities.CreateInstance(_serviceScope.ServiceProvider, middlewareType);

            await middleware.ExecuteAsync(this, () => SaveChangesAsync(cancellationToken), cancellationToken);
        }
    }

    public void Insert<TEntity>(TEntity entity) where TEntity : class
    {
        ThrowHelper.ThrowTypeNotInObjectStores(typeof(TEntity), _context);
        _entities.Add(MutationEntityContext.Insert(entity));
    }

    public void Update<TEntity>(TEntity entity) where TEntity : class
    {
        ThrowHelper.ThrowTypeNotInObjectStores(typeof(TEntity), _context);
        _entities.Add(MutationEntityContext.Update(entity));
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        ThrowHelper.ThrowTypeNotInObjectStores(typeof(TEntity), _context);
        _entities.Add(MutationEntityContext.Delete(entity));
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        return SaveChangesAsync();
    }
}
