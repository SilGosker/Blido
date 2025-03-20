using System.Reflection;

namespace Blido.Core.Transaction.Mutation.KeyGeneration;

public class KeyGeneratorMiddleware : IBlidoMiddleware
{
    private readonly IKeyGeneratorFactory _keyGeneratorFactory;
    public KeyGeneratorMiddleware(IKeyGeneratorFactory keyGeneratorFactory)
    {
        ArgumentNullException.ThrowIfNull(keyGeneratorFactory);
        _keyGeneratorFactory = keyGeneratorFactory;
    }

    public ValueTask ExecuteAsync(MutationContext context, ProcessNextDelegate next, CancellationToken cancellationToken)
    {
        foreach (var entityContext in context.Entities)
        {
            if (entityContext.State != MutationState.Added) continue;
            foreach (PropertyInfo primaryKeyInfo in entityContext.PrimaryKeys)
            {
                if (_keyGeneratorFactory.TryGetValue(primaryKeyInfo, out var generateKey))
                {
                    generateKey(entityContext.BeforeChange, primaryKeyInfo);
                }
            }
        }

        return next();
    }
}