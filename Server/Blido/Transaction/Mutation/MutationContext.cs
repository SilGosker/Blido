using Microsoft.Extensions.DependencyInjection;

namespace Blido.Core.Transaction.Mutation;

public class MutationContext : IAsyncDisposable
{
    private readonly List<Type> _pipelineTypes;
    private readonly IServiceScope _serviceScope;
    private int _index = -1;
    public MutationContext(List<Type> pipelineTypes, IServiceScope serviceScope)
    {
        ArgumentNullException.ThrowIfNull(pipelineTypes);
        ArgumentNullException.ThrowIfNull(serviceScope);
        _pipelineTypes = pipelineTypes;
        _serviceScope = serviceScope;
    }

    public List<MutationEntityContext> Entities { get; } = new();
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

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        return SaveChangesAsync();
    }
}