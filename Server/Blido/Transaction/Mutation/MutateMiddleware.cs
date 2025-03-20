using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.JSInterop;

namespace Blido.Core.Transaction.Mutation;

public class MutateMiddleware : IBlidoMiddleware
{
    private readonly IJSRuntime _jsRuntime;

    public MutateMiddleware(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask ExecuteAsync(MutationContext context, ProcessNextDelegate next, CancellationToken cancellationToken)
    {
        if (context.Entities.Count == 1)
        {
            var entity = context.Entities.First();
            if (JsMethodNames.MaterializerMethodNames.TryGetValue(entity.StateMethodName, out var methodName))
            {
                entity.AfterChange = await _jsRuntime.InvokeAsync<object>(methodName, cancellationToken, entity);
            }
        }

        var afterContext = await _jsRuntime.InvokeAsync<MutationContext>(JsMethodNames.Mutate, cancellationToken, context);

        // TODO: sync changes
    }
}