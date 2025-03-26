using Blido.Core.Helpers;
using Blido.Core.Transaction.Mutation;

namespace Blido.Core.Options;

public class MutationConfiguration
{
    private readonly List<MutationMiddlewareItem> _middlewareTypes = new();
    public IReadOnlyList<MutationMiddlewareItem> MiddlewareTypes => _middlewareTypes.AsReadOnly();

    public void Use(Type type, params object[] arguments)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(arguments);
        ThrowHelper.ThrowIfNotAssignableTo<IBlidoMiddleware>(type);
        _middlewareTypes.Add(new MutationMiddlewareItem(type, arguments));
    }

    public void Use<TMiddleware>(params object[] arguments) where TMiddleware : IBlidoMiddleware
    {
        ArgumentNullException.ThrowIfNull(arguments);
        _middlewareTypes.Add(new MutationMiddlewareItem(typeof(TMiddleware), arguments));
    }

    public void Use(Func<IServiceProvider, IBlidoMiddleware> createInstance)
    {
        ArgumentNullException.ThrowIfNull(createInstance);
        _middlewareTypes.Add(new MutationMiddlewareItem(createInstance));
    }
}