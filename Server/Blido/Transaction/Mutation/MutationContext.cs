﻿using Blido.Core.Helpers;
using Blido.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blido.Core.Transaction.Mutation;

public class MutationContext: IAsyncDisposable
{
    private readonly MutationConfiguration _mutationConfiguration;
    private readonly IServiceProvider _serviceProvider;
    private readonly List<MutationEntityContext> _entities = new();
    private int _index = -1;
    private readonly IndexedDbContext _context;
    public IndexedDbDatabase Database { get; }
    public MutationContext(IOptions<MutationConfiguration> mutationConfiguration, IServiceProvider serviceProvider, IndexedDbContext context)
    {
        ArgumentNullException.ThrowIfNull(mutationConfiguration);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(context);
        _mutationConfiguration = mutationConfiguration.Value;
        _serviceProvider = serviceProvider;
        _context = context;
        Database = context.Database;
    }

    public IReadOnlyList<MutationEntityContext> Entities => _entities.AsReadOnly();
    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (Entities.Count == 0)
        {
            return;
        }

        _index++;
        if (_index < _mutationConfiguration.MiddlewareTypes.Count)
        {
            var middlewareType = _mutationConfiguration.MiddlewareTypes[_index];
            await middlewareType.InvokeAsync(_serviceProvider, this, () => SaveChangesAsync(cancellationToken),
                cancellationToken);
        }
    }

    public void Insert(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ThrowHelper.ThrowTypeNotInObjectStores(entity.GetType(), _context);
        _entities.Add(MutationEntityContext.Insert(entity));
    }

    public void Update(object entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ThrowHelper.ThrowTypeNotInObjectStores(entity.GetType(), _context);
        _entities.Add(MutationEntityContext.Update(entity));
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(entity);
        ThrowHelper.ThrowTypeNotInObjectStores(entity.GetType(), _context);
        _entities.Add(MutationEntityContext.Delete(entity));
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        return SaveChangesAsync();
    }
}
