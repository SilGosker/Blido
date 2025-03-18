using Microsoft.Extensions.DependencyInjection;

namespace Blido.Core.Transaction.Mutation;

public class MutationContext : IAsyncDisposable
{
    private readonly List<Type> _pipelineTypes;
    private readonly IServiceScope _serviceScope;
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

    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        return SaveChangesAsync();
    }
}