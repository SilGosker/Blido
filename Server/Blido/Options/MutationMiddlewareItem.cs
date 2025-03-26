using Blido.Core.Transaction.Mutation;
using Microsoft.Extensions.DependencyInjection;

namespace Blido.Core.Options;

public class MutationMiddlewareItem
{
    public MutationMiddlewareItem(Type type, object[] arguments) : this(type, arguments, null)
    {
    }

    public MutationMiddlewareItem(Func<IServiceProvider, IBlidoMiddleware> createInstance) : this(null, null, createInstance)
    {
    }

    public MutationMiddlewareItem(Type? type = null, object[]? arguments = null, Func<IServiceProvider, IBlidoMiddleware>? createInstance = null)
    {
        if (type is null && createInstance is null)
        {
            throw new ArgumentException("Type or CreateInstance must be set.");
        }
        Type = type;
        Arguments = arguments ?? Array.Empty<object>();
        CreateInstance = createInstance;
    }

    public Type? Type { get; }
    public object[] Arguments { get; }
    public Func<IServiceProvider, IBlidoMiddleware>? CreateInstance { get; }

    public async ValueTask InvokeAsync(IServiceProvider serviceProvider,
        MutationContext context,
        ProcessNextDelegate processNext,
        CancellationToken cancellationToken)
    {
        if (Type != null)
        {
            var middleware = (IBlidoMiddleware?)serviceProvider.GetService(Type)
                             ?? (IBlidoMiddleware)ActivatorUtilities.CreateInstance(serviceProvider, Type, Arguments);
            await middleware.ExecuteAsync(context, processNext, cancellationToken);
            return;
        }
        
        if (CreateInstance != null)
        {
            var middleware = CreateInstance(serviceProvider);
            ArgumentNullException.ThrowIfNull(middleware);
            await middleware.ExecuteAsync(context, processNext, cancellationToken);
            return;
        }

        // should never be reached
        throw new InvalidOperationException("Type or CreateInstance must be set.");
    }
}